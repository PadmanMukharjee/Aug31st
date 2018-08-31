// Angular imports
import { Component, OnInit, EventEmitter, Output, Input, ElementRef, ViewChild } from '@angular/core';

// Common File Imports
import { CLIENT_ASSIGN_USER, CLIENT_SHARED, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { AssignClientUserService } from './assign-client-user.service';
import { ClientUsersModel } from './assign-client-user.model';
import { AllUsersViewModel } from '../../admin/users/all-users/models/all-users-viewmodel.model';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { Router } from '@angular/router';
import { DualListComponent } from 'angular-dual-listbox';
import { Observable } from 'rxjs/Rx';
import { Message } from 'primeng/components/common/api';


@Component({
    selector: 'assign-user',
    templateUrl: './assign-client-user.component.html',
    providers: [AssignClientUserService]
})
export class AssignClientUsersComponent implements OnInit {


    /*------ region public properties ------*/

    public keepSorted = true;
    public key: string;
    public display: any;
    public filter = true;
    public disabled = false;
    public format: any = DualListComponent.DEFAULT_FORMAT;
    public labels = CLIENT_ASSIGN_USER;
    public sharedLabels = CLIENT_SHARED;
    public allUsers: AllUsersViewModel[];
    public selectedUsers: AllUsersViewModel[];
    public usersToSave: ClientUsersModel;
    public messages: Message[] = [];
    @ViewChild('clientUserFormSubmit') public clientUserFormSubmit: ElementRef;

    /*------ end region public properties ------*/

    /*-----region Input/Output bindings -----*/
    @Output('saveAssignedUsers') SaveAssignedUsers = new EventEmitter<any>();
    @Input('clientCode') clientCode: string;

    /*-----endregion Input/Output bindings -----*/


    /*------ region constructor ------*/

    constructor(private _assignClientUserService: AssignClientUserService) {
        this.allUsers = new Array<AllUsersViewModel>();
        this.selectedUsers = new Array<AllUsersViewModel>();
        this.usersToSave = new ClientUsersModel();
        this.format = {
            add: 'Add', remove: 'Remove', all: 'All', none: 'None', draggable: true, locale: 'en'
        };
    }

    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit(): void {
        this.getAllClientUserData();
    }
    /*------ region life cycle hooks ------*/

    /*------ region Public methods ------*/
    /**
     * To save the assigned users to client
     */
    saveAssignedUsers() {
        this.usersToSave.clientCode = this.clientCode;
        this.usersToSave.clientUsers = this.selectedUsers;
        this.SaveAssignedUsers.emit(this.usersToSave);
    }

    /**
     * To get all the users and assigned users for the Client
     */
    getAllUsersData() {
        return Observable.forkJoin(
            this._assignClientUserService.getAllUsers(),
            this._assignClientUserService.getClientUsers(this.clientCode),
        );
    }

    /**
     * To get all the required data for assigning users
     */
    getAllClientUserData() {
        this.getAllUsersData().subscribe(
            data => {
                if (data[0] != null) {
                    this.allUsers = data[0];
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
                if (data[1].success) {
                    this.selectedUsers = data[1].clientUsers;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
                this.key = 'userId';
                this.display = this.userLabel;
            }
        );
    }

    saveClientUserDataFromParent() {
        this.clientUserFormSubmit.nativeElement.click();
    }


    /*------ end region Public methods ------*/


    /*------ end region Private methods ------*/

    private userLabel(user: AllUsersViewModel) {
        return user.firstName + ' ' + user.lastName + ', ' + user.role;
    }

    /*------ end region private methods ------*/
}
