// Third party imports
import { Message } from 'primeng/components/common/api';

// Angular imports
import { Component, OnInit, EventEmitter, Output, Input, Inject } from '@angular/core';

// Common File Imports
import { CLIENT_LOG_SETUP, CLIENT_SHARED, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { PayerService } from '../../admin/payers/payers.service';
import { LogSetupViewModel } from './log-setup.model';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';

@Component({
    selector: 'log-setup',
    templateUrl: './log-setup.component.html',
    styleUrls: ['../../../assets/build/css/switch.css']
})
export class LogSetupComponent implements OnInit {

    /*-----region Input/Output bindings -----*/
    @Output('saveLogSetup') SaveLogSetup = new EventEmitter<any>();
    @Input('clientCode') clientCode: string;
    /*-----endregion Input/Output bindings -----*/

    /*------ region public properties ------*/
    public labels = CLIENT_LOG_SETUP;
    public sharedLabels = CLIENT_SHARED;
    public logSetups: LogSetupViewModel[];
    public logSetup: LogSetupViewModel;
    public payers: any[];
    public selectedPayers: any[];
    public payerCode: string;
    public displayDialog = false;
    public selectedLogSetup: any;
    public popupHeader: string;
    public isM3FeeColumnVisible: boolean;
    public canEdit = false;
    public editActions: any[] = [];
    public isEditPayer = false;
    public messages: Message[] = [];
    /*------ end region public properties ------*/



    /*------ region constructor ------*/
    constructor(private payerService: PayerService,
        private userService: UserService
    ) {
        this.getScreenActions();
        this.payers = [];
    }
    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit(): void {
        this.getClientPayers();
    }
    /*------ region life cycle hooks ------*/

    /*------ region Public methods ------*/

    /**
     * Get Active Payers To Assign For Client (to show in payers dropdown)
     * @param isEdit
     * @param clientPayerToEdit
     */
    getActivePayersToAssignForClient(isEdit: boolean = false, clientPayerToEdit: any = []) {
        this.payerService.getActivePayersToAssignForClient(this.clientCode).subscribe(
            (response: any) => {
                if (response.success === true) {
                    if (response) {
                        this.payers = [];
                    }
                    if (isEdit) {
                        this.payers.push({ label: clientPayerToEdit.payerName, value: clientPayerToEdit.payerCode });
                    }
                    for (let payer of response.listOfPayerViewModel) {
                        this.payers.push({ label: payer.payerName, value: payer.payerCode });
                    }
                } else {
                    this.messages = [];
                    this.messages.push({ severity: 'error', summary: ADMIN_SHARED.ERROR, detail: response.errorMessages[0] });
                }
            });
    }

    /**
     * Get all the payers assigned to a client
     */
    getClientPayers() {
        this.payerService.getClientPayers(this.clientCode).subscribe(
            (response) => {
                if (response.success === true) {
                    this.logSetups = response.listOfClientPayerViewModel;
                    this.logSetups.forEach(x => {
                        this.isM3FeeColumnVisible = x.isM3FeeExempt == null ? false : true;
                        x.isActive = (x.recordStatus === this.sharedLabels.ACTIVE_CHARACTER);
                    });
                } else {
                    this.messages = [];
                    this.messages.push({ severity: 'error', summary: ADMIN_SHARED.ERROR, detail: response.errorMessages[0] });
                }
            }
        );
    }

    /**
     *  display popup on click of add button
     */
    showDialogToAdd() {
        this.popupHeader = this.labels.ADD_NEW_LOG_SETUP;
        this.payers = [];
        this.selectedPayers = [];
        this.getActivePayersToAssignForClient();
        this.logSetup = new LogSetupViewModel();
        this.logSetup.isActive = true;
        this.logSetup.isM3FeeExempt = false;
        this.displayDialog = true;
    }

    /**
     * display popup on click of edit icon
     * @param clientPayer
     */
    onRowSelect(clientPayer: LogSetupViewModel) {
        this.popupHeader = this.labels.DELETE_LOG_SETUP;
        this.logSetup = new LogSetupViewModel();
        this.logSetup = clientPayer;
        // this.getActivePayersToAssignForClient(true, clientPayer);
        this.displayDialog = true;
        this.isEditPayer = true;
    }


    /**
     * add/edit client payers
     */
    saveClientPayer() {
        let clientPayers: LogSetupViewModel[] = [];
        if (this.isEditPayer) {
            let clientPayer = this.logSetup;
            if (!clientPayer.payerCode) {
                return;
            }
            clientPayer.recordStatus = clientPayer.canDelete ? this.sharedLabels.DELETE_CHARACTER : this.logSetup.isActive ? this.sharedLabels.ACTIVE_CHARACTER : this.sharedLabels.INACTIVE_CHARACTER;
            clientPayer.clientCode = this.clientCode;
            clientPayers.push(clientPayer);
            this.isEditPayer = false;
        } else if (this.selectedPayers != null) {
            this.selectedPayers.forEach(payer => {
                let clientPayer = new LogSetupViewModel();
                clientPayer.payerCode = payer;
                clientPayer.isActive = true;
                clientPayer.isM3FeeExempt = false;
                clientPayer.recordStatus = this.logSetup.isActive ? this.sharedLabels.ACTIVE_CHARACTER : this.sharedLabels.INACTIVE_CHARACTER;
                clientPayer.clientCode = this.clientCode;
                clientPayers.push(clientPayer);
            });
        }
        this.SaveLogSetup.emit(clientPayers);
        this.displayDialog = false;

    }

    /**
     * save client payers to db
     * @param logSetups
     */
    saveClientPayers(logSetups: LogSetupViewModel[]) {
        this.payerService.saveClientPayers(logSetups).subscribe(
            (success) => {
                if (success) {
                    this.getClientPayers();
                }
            }
        );
    }

    onToggle(clientPayer: LogSetupViewModel) {
        if (clientPayer) {
            let clientPayers: LogSetupViewModel[] = [];
            clientPayer.recordStatus = clientPayer.isActive ? this.sharedLabels.ACTIVE_CHARACTER : this.sharedLabels.INACTIVE_CHARACTER;
            clientPayers.push(clientPayer);
            this.saveClientPayers(clientPayers);
        }
    }


    onStatusToggle(clientPayer: LogSetupViewModel) {
        if (clientPayer) {
            clientPayer.recordStatus = clientPayer.isActive ? this.sharedLabels.ACTIVE_CHARACTER : this.sharedLabels.INACTIVE_CHARACTER;
            this.activateInactivatePayor(clientPayer);
        }
    }

    activateInactivatePayor(logSetup: LogSetupViewModel) {
        this.payerService.activeOrInactiveClientPayer(logSetup).subscribe(
            (data) => {
                if (data.success === true) {
                    this.getClientPayers();
                } else {
                    this.messages = [];
                    this.messages.push({ severity: 'error', summary: ADMIN_SHARED.ERROR, detail: data.errorMessages[0] });
                }
            },
            err => { }
        );
    }

    disbleEdit() {
        this.displayDialog = false;
        this.isEditPayer = false;
    }

    // Get Edit Actions of log setup page
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.LOGSETUP)
            .subscribe((actions) => {
                if (actions) {
                    if (actions.length === 0) {
                        this.canEdit = false;
                    } else {
                        this.canEdit = true;
                        this.editActions = actions;
                    }
                }
            });
    }

    /*------ end region Public methods ------*/
}
