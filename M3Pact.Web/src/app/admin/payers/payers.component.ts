// Third party imports
import { Message } from 'primeng/components/common/api';
import * as _ from 'lodash';

// Angular imports
import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';

// Common File Imports
import { PayerService } from '../payers/payers.service';
import { PayerViewModel } from './payers.model';
import { ADMIN_PAYER, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { UserService } from '../../shared/services/user.service';

@Component({
    selector: 'app-payers',
    templateUrl: './payers.component.html',
    styleUrls: ['../../../assets/build/css/switch.css', './payers.component.css'],
    providers: [PayerService]
})
export class PayersComponent implements OnInit, OnDestroy {

    /*------ region public properties ------*/

    public labels = ADMIN_PAYER;
    public sharedLabels = ADMIN_SHARED;
    public displayPayers: boolean;
    public clientsOfPayer: any[] = [];
    public validationConstants = VALIDATION_MESSAGES;
    public messageList: Message[] = [];
    public dialogHeader: string;
    public valid = true;
    public payers: PayerViewModel[];
    public payer: PayerViewModel;
    public selectedpayer: any;
    public displayDialog: boolean;
    public isAddPayer: boolean;
    public canEdit = false;
    public editActions: any[] = [];
    public displayingClients: boolean;
    public messages: Message[] = [];
    public paginationCount: any;
    public rowsIn: number = 10;
    public payersFiltered: any;
    /*------ end region public properties ------*/


    /*------ region constructor ------*/

    constructor(private payerService: PayerService, private userService: UserService,
        private ref: ChangeDetectorRef) {
    }

    /*------ end region constructor ------*/


    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.getScreenActions();
        this.payers = [];
        this.getPayers();
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------ end region life cycle hooks ------*/


    /*------ region service calls------*/

    /**
     * gets the payers data from db
    **/
    getPayers() {
        this.payerService.getPayers().subscribe(
            (response) => {
                if (response.success === true) {
                    this.payers = response.listOfPayerViewModel;
                    this.payers.forEach(x => {
                        x.isActive = (x.recordStatus === this.sharedLabels.ACTIVE_CHAR);
                        x.clientsCount = x.clients.length;
                    });
                    this.payers = this.payers.reverse();
                    this.payersFiltered = this.payers;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: this.sharedLabels.SEVERITY_ERROR, summary: this.sharedLabels.ERROR, detail: response.errorMessages[0] });
                }
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            }
        );
    }

    /**
     * saves payers Changes to db
    **/
    savePayers(payers: PayerViewModel[]) {
        this.payerService.savePayers(payers).subscribe(
            (response) => {
                if (response.success === true) {
                    this.messages = [];
                    this.messages.push({ severity: this.sharedLabels.SEVERITY_SUCCESS, summary: this.sharedLabels.SEVERITY_SUCCESS, detail: response.successMessage });
                    this.getPayers();
                } else {
                    this.messages = [];
                    this.messages.push({ severity: this.sharedLabels.SEVERITY_ERROR, summary: this.sharedLabels.ERROR, detail: response.errorMessages[0] });
                }
            }
        );
    }

    /*------ end region service calls------*/


    /*------ region public methods ------*/

    /**
     * Called when pagechange event occurs
     */
    onTablePageChange(e) {
        let start = e.first + 1;
        let end = (e.first + e.rows) > this.payersFiltered.length ? this.payersFiltered.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.payersFiltered.length;
    }
    /**
     * displays a dialog on clicking add button
    **/
    showDialogToAdd() {
        this.isAddPayer = true;
        this.payer = new PayerViewModel();
        this.payer.isActive = true;
        this.dialogHeader = this.labels.ADD + ' ' + this.labels.PAYER;
        this.displayDialog = true;
        this.focusPayer();
    }

    /**
     * displays a dialog on selecting a row
    **/
    onRowSelect(payer: PayerViewModel) {
        this.isAddPayer = false;
        this.payer = new PayerViewModel();
        this.payer.payerCode = payer.payerCode;
        this.payer.id = payer.id;
        this.payer.isActive = payer.isActive;
        this.dialogHeader = this.labels.EDIT + ' ' + this.labels.PAYER;
        this.displayDialog = true;
        this.focusPayer();
    }

    /**
     * added/edited changes to payers will be saved
    **/
    save() {
        this.messageList = [];
        if (this.payer.payerCode == undefined || this.payer.payerCode.length == 0 || (this.payer.payerCode.length > 0 && this.payer.payerCode.trim().length == 0)) {
            this.valid = false;
            return;
        }
        let payersObject: PayerViewModel[];
        if (!this.isAddPayer && this.payers.some(val => _.toLower(val.payerCode) == _.toLower(this.payer.payerCode) && val.id != this.payer.id)) {
            this.messageList = [];
            this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.PAYER_EXISTS });
        } else if (this.isAddPayer && this.payers.some(val => _.toLower(val.payerCode) == _.toLower(this.payer.payerCode))) {
            this.messageList = [];
            this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.PAYER_EXISTS });
        } else {
            payersObject = [];
            let payerObject: PayerViewModel;

            if (this.isAddPayer) {
                payerObject = new PayerViewModel();
                this.isAddPayer = false;
            } else {
                payerObject = this.payers.find(x => x.id == this.payer.id);
            }

            payerObject.id = this.payer.id;
            payerObject.payerCode = this.payer.payerCode;
            payerObject.payerName = this.payer.payerCode;
            payerObject.payerDescription = this.payer.payerCode;
            payerObject.isActive = this.payer.isActive;
            payerObject.recordStatus = payerObject.isActive ? this.sharedLabels.ACTIVE_CHAR : this.sharedLabels.INACTIVE_CHAR;
            this.payers.push(payerObject);
            payersObject.push(payerObject);
            this.savePayers(payersObject);
            this.payers = [];
            this.payer = null;
            this.displayDialog = false;
        }
    }

    changeStateOfPayer(payer: PayerViewModel) {
        this.selectedpayer = payer;
        this.clientsOfPayer = [];
        if (!payer.isActive) {
            this.payerService.GetClientsAssignedtoPayer(payer.payerCode).subscribe(
                (data) => {
                    if (data.success === true) {
                        if (data != null && data.length > 0) {
                            this.clientsOfPayer = data;
                            this.displayPayers = true;
                        } else {
                            this.activeOrInactivePayer();
                        }
                    } else {
                        this.messages = [];
                        this.messages.push({ severity: this.sharedLabels.SEVERITY_ERROR, summary: this.sharedLabels.ERROR, detail: data.errorMessages[0] });
                    }
                }
            );
        } else {
            this.activeOrInactivePayer();
        }
    }
    /**
     * To active or inactive payers.
     */
    activeOrInactivePayer() {
        this.payerService.activeOrInactivePayers(this.selectedpayer).subscribe(
            (data) => {
                if (data.success === true) {
                    this.displayPayers = false;
                    this.clientsOfPayer = [];
                } else {
                    this.messages = [];
                    this.messages.push({ severity: this.sharedLabels.SEVERITY_ERROR, summary: this.sharedLabels.ERROR, detail: data.errorMessages[0] });
                }
            }
        );
    }

    keepActivePayer() {
        this.displayPayers = false;
        this.selectedpayer.isActive = true;
        this.clientsOfPayer = [];
    }

    clearMessages() {
        this.messageList = [];
        this.valid = true;
    }

    /**
     * focus on the input field after the popup is shown.
     */
    focusPayer() {
        this.ref.detectChanges();
        jQuery('#payer').focus();
    }

    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.PAYERS)
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

    onClientsColumnSelect(payer) {
        this.displayingClients = true;
        this.payer = payer;
    }

    closeDialog() {
        this.displayingClients = false;
    }

    /**
    * Called when filter event occurs
    */
    onTableFilter(event) {
        this.payersFiltered = event.filteredValue;
    }
    /*------ end region public methods ------*/

}
