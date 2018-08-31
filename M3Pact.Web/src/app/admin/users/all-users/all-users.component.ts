// Angular imports
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

// Third party import
import { SortEvent } from 'primeng/components/common/api';
import { Message } from 'primeng/components/common/api';

// Common File Imports
import { AllUsersService } from './all-users.service';
import { USERS_ALLUSERS, ADMIN_SHARED } from '../../../shared/utilities/resources/labels';
import { AllUsersModel } from './models/all-users.model';
import { AllUsersViewModel } from './models/all-users-viewmodel.model';
import { SelectedClients } from './models/selected-clients.model';

@Component({
    selector: 'all-users',
    templateUrl: './all-users.component.html',
    styleUrls: ['../../../../assets/build/css/switch.css'],
    providers: [AllUsersService]
})

export class AllUsersComponent implements OnInit {

    /*------ region public properties ------*/
    public errormsg = false;
    public labels = USERS_ALLUSERS;
    public sharedLabels = ADMIN_SHARED;
    public selectedColumns: any[];
    public tableColumns: any[];
    public displaycolumns: AllUsersModel;
    public displayParams: any[];
    public users: AllUsersViewModel[];
    public user: AllUsersViewModel;
    public displayDialog: boolean;
    public isAddUser: boolean;
    public cols: any[];
    public filteredCols: any[];
    public displayCols: any[];
    public displayFilterdCols: any[];
    public selectedClients: SelectedClients[];
    public messages: Message[] = [];
    public filteredUserData: any[];
    public paginationCount: any;
    public rowsIn: number = 20;

    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(private usersService: AllUsersService, private router: Router) {
    }

    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit() {
        this.users = [];
        this.tableColumns = [];
        this.displaycolumns = new AllUsersModel();
        this.displayParams = new AllUsersModel().displayParams;
        this.displayParams.forEach(col => {
            this.tableColumns.push({ columnName: col.field, columnText: col.header });
        });
        this.getUsers();
        this.selectedColumns = this.tableColumns;
        this.cols = [
            { field: 'reportsTo', header: 'Reports To' },
            { field: 'businessUnit', header: 'Business Unit' },
            { field: 'site', header: 'Site' },
            { field: 'title', header: 'Title' },
            { field: 'role', header: 'Role' },
            { field: 'Status', header: 'Status' }
        ];
        this.filteredCols = [
            { field: 'userId' },
            { field: 'firstName' },
            { field: 'lastName' },
            { field: 'email' },
            { field: 'reportsTo' },
            { field: 'businessUnit' },
            { field: 'site' },
            { field: 'title' },
            { field: 'role' },
            { field: 'Status' }
        ];
        this.displayCols = this.cols;
        this.displayFilterdCols = this.filteredCols;
    }
    /*------ end region life cycle hooks ------*/

    /*------ region service calls------*/

    /**
     * gets the users data from db
    **/
    getUsers() {
        this.usersService.getAllUsers().subscribe(
            (response: any) => {
                if (response != null) {
                    this.users = response;
                    this.users.forEach(x => {
                        if (x.recordStatus == this.sharedLabels.ACTIVE_CHAR) {
                            x.isActive = true;
                            x.Status = this.sharedLabels.ACTIVE;
                        } else {
                            x.isActive = false;
                            x.Status = this.sharedLabels.INACTIVE;
                        }
                        x.selectedClientsCount = x.selectedClients.length;
                    });
                    this.users = this.users.reverse();
                    this.filteredUserData = this.users;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: ' ', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }

                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            }
        );
    }

    /*------ end region service calls------*/


    /*------ region public methods ------*/

   /**
 * Called when pagechange event occurs
 */
    onTablePageChange(e) {
        let start = this.filteredUserData.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.filteredUserData.length ? this.filteredUserData.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.filteredUserData.length;
    }

    changeStatus() {
        this.cols = [];
        this.filteredCols = [
            { field: 'userId' },
            { field: 'lastName' }
        ];

        Object.keys(this.displaycolumns).forEach(col => {
            this.displaycolumns[col] = false;
        });
        Object.keys(this.displaycolumns).forEach(col => {
            this.selectedColumns.forEach(tabCol => {
                if (tabCol.columnName == col) {
                    this.displaycolumns[col] = true;
                }
            });
        });
        Object.keys(this.displayCols).forEach(col => {
            this.selectedColumns.forEach(tabCol => {
                if (tabCol.columnName == this.displayCols[col].field) {
                    this.cols.push(this.displayCols[col]);
                }
            });
        });
        Object.keys(this.displayFilterdCols).forEach(col => {
            this.selectedColumns.forEach(tabCol => {
                if (tabCol.columnName == this.displayFilterdCols[col].field) {
                    this.filteredCols.push(this.displayFilterdCols[col]);
                }
            });
        });
    }

    /**
     * Redirecting to respective page based on user.
    **/
    onRowSelect(user: AllUsersViewModel) {
        this.user = new AllUsersViewModel();
        if (user.isMeridianUser) {
            this.router.navigateByUrl('/administration/users/m3users?userID=' + user.userId);
        } else {
            this.router.navigateByUrl('/administration/users/clientusers?userID=' + user.userId);
        }
    }

    /**
     * displays a dialog on clicking selectedClients column
    **/
    onSelectClientsColumnSelect(event) {
        this.user = event;
        this.selectedClients = this.user.selectedClients;
        this.displayDialog = true;
    }

    /**
     * closes the dialog on clicking ok button
    **/
    closeDialog() {
        this.displayDialog = false;
    }

     /**
     * Called when filter event occurs
     */
    onTableFilter(event) {
        this.filteredUserData = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };
        this.onTablePageChange(e);  
        }
    
    /*------ end region public methods ------*/
}
