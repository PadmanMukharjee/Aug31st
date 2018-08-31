// Angular imports
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

// Common File Imports
import { ADMIN_CHECKLIST, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { CheckListService } from '../services/checklist.service';
import { ViewAllChecklistModel, SelectedClients, SelectedSites, SelectedSystems } from '../models/view-all-checklist.model';

@Component({
    selector: 'view-all-checklist',
    templateUrl: './view-all-checklist.component.html',
    providers: [UserService],
    styles: [``
    ]
})
export class ViewAllChecklistComponent implements OnInit {

    /*------ region public properties ------*/

    public sharedLabels = ADMIN_SHARED;
    public labels = ADMIN_CHECKLIST;
    public canEdit = false;
    public editActions: any[] = [];
    public allChecklists: ViewAllChecklistModel[];

    public checklist: ViewAllChecklistModel;
    public selectedClients: SelectedClients[];
    public selectedSites: SelectedSites[];
    public selectedSystems: SelectedSystems[];

    public displayClientDialog: boolean;
    public displaySystemDialog: boolean;
    public displaySiteDialog: boolean;

    public paginationCount: any;
    public rowsIn: number = 10;
    public checklistfiltered: any;

    /*------ end region public properties ------*/


    /*---------- region constructor ----------*/

    constructor(private router: Router, private userService: UserService, private _checklistService: CheckListService) {
        this.getScreenActions();
    }

    /*-------- end region constructor --------*/

    /*------ region life cycle hooks ------*/

     ngOnInit(): void {
        this.getChecklists();
    }

    /*------end region life cycle hooks ------*/

    /*------ region service calls------*/

    /**
     * Get Edit Actions of ViewAllChecklist page
     */
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.VIEWALLCHECKLIST)
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

    /**
    * To get all the Checklists
    */
    getChecklists() {
        this._checklistService.getViewAllChecklists().subscribe(
            data => {
                this.allChecklists = data;
                let e = { first: 0, rows: this.rowsIn };
                this.checklistfiltered = this.allChecklists;
                this.onTablePageChange(e);               
            },
            err => { }          
        );       
    }

    /*------ end region service calls ------*/

    /*-------- region public methods --------*/

 /**
 * Called when table's pagechange event occurs
 */
    onTablePageChange(e) {
        let start = this.checklistfiltered.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.checklistfiltered.length ? this.checklistfiltered.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.checklistfiltered.length;
    }
    /**
     * On edit will be redirected to edit checklist page
     * @param rowData
     */
    onRowSelect(rowData) {
        this.router.navigateByUrl('/administration/checklist/' + rowData.id);
    }

    /**
     * To popup selected clients for checklist
     * @param event
     */
    onSelectClients(event) {
        this.checklist = event;
        this.selectedClients = this.checklist.selectedClients;
        this.displayClientDialog = true;
    }
    /**
     *  To popup selected systems for checklist
     * @param event
     */
    onSelectSystems(event) {
        this.checklist = event;
        this.selectedSystems = this.checklist.selectedSystems;
        this.displaySystemDialog = true;
    }

    /**
     *  To popup selected sites for checklist
     * @param event
     */
    onSelectSites(event) {
        this.checklist = event;
        this.selectedSites = this.checklist.selectedSites;
        this.displaySiteDialog = true;
    }

    /**
     * To close client dialog
     */
    closeClientDialog() {
        this.displayClientDialog = false;
    }

    /**
     * To close System dialog
     */
    closeSystemDialog() {
        this.displaySystemDialog = false;
    }

    /**
     *  To close Site dialog
     */
    closeSiteDialog() {
        this.displaySiteDialog = false;
    }

     /**
     * Called when filter event occurs
     */
    onTableFilter(event) {
        this.checklistfiltered = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };       
        this.onTablePageChange(e);  
       }

    /*------ end region public methods------*/

}
