// Third party imports
import { AutoComplete } from 'primeng/primeng';
import { Message } from 'primeng/components/common/api';

// Angular imports
import { Component, OnInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';

// Common File Imports
import { MthreeUsersService } from './m3-user.service';
import { USERS_M3USERS, ADMIN_SHARED } from '../../../shared/utilities/resources/labels';
import { AppConstants } from '../../../app.constants';

// Model Imports
import { M3UserModel } from './m3-user.model';
import { Observable } from 'rxjs/Rx';

@Component({
    selector: 'm3-user',
    templateUrl: './m3-user.component.html',
    styleUrls: ['../../../../assets/build/css/switch.css', './m3-user.component.css'],
    providers: [MthreeUsersService]
})

export class MthreeUsersComponent implements OnInit {
    /*------ region public properties ------*/
    public m3UserModel: M3UserModel;
    public labels = USERS_M3USERS;
    public sharedLabels = ADMIN_SHARED;
    public isEditOperation: boolean;
    public roles: any[];
    public clients: any[];
    public userNameSuggesstions: any[];
    public displayDialog: boolean;
    public displayLabel: string;
    public dialogueMsg: string;
    public canContinue: boolean;
    public showConfirmation: boolean;
    public confirmationMsg: string;
    public eligibleRoles: any[];
    public viewRoleOnly = false;
    public messages: Message[] = [];


    @ViewChild('autoCompleteObject') autoCompleteObject: AutoComplete;
    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(private m3userService: MthreeUsersService, private route: ActivatedRoute, private router: Router, private ref: ChangeDetectorRef) {
        this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
        };
        this.m3UserModel = new M3UserModel();
    }

    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit() {
        this.isEditOperation = false;
        this.getInitialData();
    }

    ngAfterViewInit() {
        this.autoCompleteObject.domHandler.findSingle(this.autoCompleteObject.el.nativeElement, 'input').focus();
    }
    /*------ end region life cycle hooks ------*/

    /*------ region service calls------*/



    /*------------------- to get m3userdata for given userid--------------------*/
    public getM3UserData(userID: string) {
        if (userID) {
            this.m3userService.getEmployeeDetails(userID).subscribe(
                (response: any) => {
                    if (response.success) {
                        this.m3UserModel = response;
                        this.setPermissionsOnRoles(response.user.roleCode);
                        let temp = response.clients;
                        this.m3UserModel.clients = [];
                        temp.forEach(selectedclient => {
                            this.clients.forEach(c => {
                                if (c.value == selectedclient) {
                                    this.m3UserModel.clients.push(selectedclient);
                                }
                            });
                        });

                    } else {
                        this.messages = [];
                        this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: 'Entered Value', detail: response.errorMessages[0] });
                    }
                }
            );

        } else {
            this.m3UserModel = new M3UserModel();
        }
    }

    /*------------------- fires for autofilling of last name search--------------------*/
    searchAutofillUserNames(event) {
        this.m3userService.getUserNamesForAutoFill(event.query).subscribe(
            (response: any) => {
                if (response) {
                    this.userNameSuggesstions = [];
                    this.userNameSuggesstions = response;
                }
            });
    }

    /*------------------- fires when user selected from autofill--------------------*/
    public userNameSelected(event) {
        this.getM3UserData(event.key);
    }

    /**
     * To save M3 user data
     */
    public SaveM3User(form: NgForm) {
        if (form.valid) {
            this.showConfirmationBox();
            if (this.canContinue) {
                this.saveUser();
            }
        } else {
            this.m3UserModel.user.isActive = true;
        }
    }

    /**
     * to save m3 user
     */
    public saveUser() {
        this.m3userService.saveUsers(this.m3UserModel).subscribe((responce: any) => {
            if (responce.success) {
                this.displayDialog = true;
                this.dialogueMsg = 'Saved Sucessfully';
                this.displayLabel = 'Sucess';
            } else {
                this.displayDialog = true;
                this.dialogueMsg = responce.errorMessages[0];
                this.displayLabel = 'Error';
            }
        });
    }
    /*------ end region service calls------*/


    /*------ region public methods ------*/
    /**
    * closes the dialog on clicking ok button
    **/
    public displayDialogclose() {
        this.displayDialog = false;
        this.router.navigateByUrl('/administration/users/allusers');
    }
    public showSaveButton() {
        let isEdit = this.m3UserModel.isUserExist;
        return (this.m3UserModel.user.userId && this.m3UserModel.isActiveEmployee && !this.viewRoleOnly
            && (((this.m3UserModel.user.isActive && !(isEdit)) || isEdit)
                && (this.m3UserModel.user.roleCode)));
    }

    getData() {
        return Observable.forkJoin(
            this.m3userService.getClients(),
            this.m3userService.getRoles()
        );
    }

    getInitialData() {
        this.getData().subscribe(
            data => {
                this.setClientDataValue(data[0]),
                    this.setRolesDataValues(data[1]),
                    this.getUser();
            }
        );
    }

    getUser() {
        let userID = this.route.snapshot.queryParams['userID'];
        if (userID) {
            this.isEditOperation = true;
            this.getM3UserData(userID);
        }
    }

    public setClientDataValue(response: any) {
        this.clients = [];
        if (response) {
            let resultClients: any[];
            resultClients = response;
            resultClients.forEach(x => {
                this.clients.push({
                    label: x.clientCode + ' - ' + x.name, value: x.clientCode
                });
            });
        }

    }

    public setRolesDataValues(response: any) {
        this.eligibleRoles = [];
        this.roles = [];
        if (response) {
            response.forEach(x => {
                this.roles.push({
                    label: x.roleCode, value: x.roleCode
                });
            });
            this.eligibleRoles = [...this.roles];
        }
    }

    // Edit/View permissions on Roles dropdown
    public setPermissionsOnRoles(roleCode) {
        if (roleCode) {
            let eligibleRole = this.eligibleRoles.find(function (element) {
                return element.value == roleCode;
            });
            if (!eligibleRole) {
                this.enableViewOnly(roleCode);
            } else {
                this.enableEdit();
            }
        } else {
            this.enableEdit();
        }
    }

    // Enabling Edit permission
    public enableEdit() {
        this.viewRoleOnly = false;
        this.roles = [...this.eligibleRoles];
    }

    // Enabling View permission
    public enableViewOnly(roleCode) {
        this.viewRoleOnly = true;
        this.roles.push({
            label: roleCode, value: roleCode
        });
    }

    /**
     * foucs on last name search
     */
    focusLastNameSearch() {
        this.ref.detectChanges();
        // jQuery('#lastNameSearch').addClass("ui-inputwrapper-focus'");
    }

    /**
 * shows popup on inactivating user
 */
    showConfirmationBox() {

        if (!this.m3UserModel.user.isActive) {
            this.showConfirmation = true;
            this.confirmationMsg = AppConstants.m3UserConstants.confirMationMsg;
        } else {
            this.canContinue = true;
        }

    }

    /**
     * click of yes or no on COnfirmation popup
     * @param yorn
     */
    confirmationClick(yorn) {
        if (yorn === 'n') {
            this.m3UserModel.user.isActive = true;
            this.canContinue = false;
        } else {
            this.saveUser();
        }
        this.showConfirmation = false;
    }

    /*------ end region public methods ------*/

}
