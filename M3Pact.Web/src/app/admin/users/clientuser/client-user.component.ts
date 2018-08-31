// Angular imports
import { Component, OnInit, Input, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { NgForm } from '@angular/forms';

// Third party imports
import { SelectItem } from 'primeng/components/common/selectitem';
import { Message } from 'primeng/components/common/api';

// Common File Imports
import { ClientUsersService } from './client-user.service';
import { USERS_CLIENTUSERS, ADMIN_SHARED, ADMIN_CLIENTUSER, SHARED } from '../../../shared/utilities/resources/labels';
import { GlobalEventsManager } from '../../../shared/utilities/global-events-manager';
import { AppConstants } from '../../../app.constants';
import { UserLogin } from '../../../shared/models/user-login';
import { ClientViewModel } from '../../../shared/models/DepositLog/client.model';
import { ValidationMessageComponent } from '../../../common/components/validation-message/validation-message.component';
import { SelectedClients } from '../all-users/models/selected-clients.model';

@Component({
    selector: 'client-user',
    templateUrl: './client-user.component.html',
    styleUrls: ['../../../../assets/build/css/switch.css', './client-user.component.css'],
    providers: [ClientUsersService]
})

export class ClientUsersComponent implements OnInit, OnDestroy {

    /*-----region Input/Output bindings -----*/
    @Input('userId') userID: string;
    /*-----endregion Input/Output bindings -----*/

    /*------ region public properties ------*/

    public labels = USERS_CLIENTUSERS;
    public sharedLabels = ADMIN_SHARED;
    public clientUsersLables = ADMIN_CLIENTUSER;
    public clients: SelectItem[];
    public clientUser: UserLogin = new UserLogin();
    public userId: number;
    public displayDialog: boolean;
    public showConfirmation: boolean;
    public displayLabel: string;
    public dialogueMsg: string;
    public isEditOperation = false;
    public confirmationMsg: string;
    public canContinue: boolean;
    public messages: Message[] = [];

    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(private clientUserService: ClientUsersService, private _globalEventsManager: GlobalEventsManager,
        private route: ActivatedRoute, private router: Router, private ref: ChangeDetectorRef) {
        this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
        };
        this.userID = this.route.snapshot.queryParams['userID'];
    }

    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.clients = [];
        this.showConfirmation = false;
        if (this.userID !== null && this.userID !== '' && this.userID != undefined) {
            this.isEditOperation = true;
        } else {
            this.isEditOperation = false;
            this.clientUser.isActive = true;
        }
        this.getClients();
        this.focusFirstName();
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------ end region life cycle hooks ------*/

    /*------ region service calls------*/

    /**
     * to get clients mapped by logged in user
     */
    public getClients() {
        this.clientUserService.getClients().subscribe(
            (response: any) => {
                if (response.length > 0) {
                    this.clients = [];
                    response.forEach(x => {
                        this.clients.push({
                            label: x.name,
                            value: x.clientCode
                        });
                    });
                } else {
                    this.messages = [];
                    this.messages.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }
                // Calling to get user details and to preselect existing cleint user clients.
                if (this.isEditOperation) {
                    this.getUserById();
                }
            }
        );
    }

    /**
     * to get user by given id in url
     */
    public getUserById() {
        this.clientUserService.getUserById(this.userID).subscribe(
            (response: any) => {
                if (response.success) {
                    this.clientUser = response;
                    let temp = response.clients;
                    this.clientUser.selectedClientData = [];
                    temp.forEach(selectedclient => {
                        this.clients.forEach(c => {
                            if (c.value == selectedclient.clientCode) {
                                this.clientUser.selectedClientData.push(c.value);
                            }
                        });
                    });
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: 'Entered Value', detail: response.errorMessages[0] });
                }
            }
        );
    }

    /*------ end region service calls------*/


    /*------ region public methods ------*/

    /**
     * validates added client form and send it to server
     * @param form
     */
    public addClientUser(form: NgForm) {
        if (form.valid) {
            this.showConfirmationBox();
            if (this.canContinue) {
                this.saveClientUser();
            }
        } else {
            this.clientUser.isActive = true;
        }
    }

    /**
     * to save client user
     */
    public saveClientUser() {
        this.clientUser.clients = [];
        if (this.clientUser.selectedClientData.length > 0) {
            this.clientUser.selectedClientData.forEach(client => {
                this.clients.forEach(c => {
                    if (c.value == client) {
                        let clientMap = new ClientViewModel();
                        clientMap.clientCode = c.value;
                        this.clientUser.clients.push(clientMap);
                    }
                });
            });
        }
        this.clientUser.roleCode = AppConstants.clientUseConstants.client;
        this.clientUser.roleName = AppConstants.clientUseConstants.client;
        this.clientUser.isMeridianUser = false;
        this.clientUserService.saveUsers(this.clientUser).subscribe((responce: any) => {
            if (responce) {
                if (responce.success) {
                    this.displayDialog = true;
                    this.dialogueMsg = AppConstants.clientUseConstants.sucessMsg;
                    this.displayLabel = this.clientUsersLables.SUCCESS;
                } else {
                    this.displayDialog = true;
                    this.dialogueMsg = responce.errorMessages[0];
                    this.displayLabel = this.clientUsersLables.ERROR;
                }
            } else {
                this.displayDialog = true;
                this.dialogueMsg = AppConstants.clientUseConstants.errorMsg;
                this.displayLabel = this.clientUsersLables.ERROR;
            }
        });
    }

    /**
     * closes the dialog on clicking ok button
    **/
    closeDialog() {
        this.displayDialog = false;
        this.router.navigateByUrl('/administration/users/allusers');
    }

    /**
     * shows popup on inactivating user
     */
    showConfirmationBox() {
        if (!this.clientUser.isActive) {
            this.showConfirmation = true;
            this.confirmationMsg = AppConstants.clientUseConstants.confirMationMsg;
        } else {
            this.canContinue = true;
        }
    }

    /**
     * click of yes or no on Confirmation popup
     * @param isNo
     */
    confirmationClick(isNo) {
        if (isNo === 'n') {
            this.clientUser.isActive = true;
            this.canContinue = false;
        } else {
            this.saveClientUser();
        }
        this.showConfirmation = false;
    }


    /**
     * foucs on first name
     */
    focusFirstName() {
        this.ref.detectChanges();
        jQuery('#firstName').focus();
    }

    /*------ end region public methods ------*/
}
