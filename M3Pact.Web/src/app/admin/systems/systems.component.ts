// Third party imports
import { Message } from 'primeng/components/common/api';
import { DropdownModule } from 'primeng/dropdown';
import * as _ from 'lodash';

// Angular imports
import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

// Common File Imports
import { SystemsService } from './systems.service';
import { SystemsModel } from '../systems/systems.model';
import { ADMIN_SYSTEMS, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';

@Component({
    selector: 'systems',
    templateUrl: './systems.component.html',
    styleUrls: ['../../../assets/build/css/switch.css'],
    providers: [SystemsService],

})
export class SystemsComponent implements OnInit, OnDestroy {

    /*------ region public properties ------*/

    public labels = ADMIN_SYSTEMS;
    public sharedLabels = ADMIN_SHARED;
    public displayDialog: boolean;
    public systems: SystemsModel[];
    public system: SystemsModel;
    public selectedSystem: any = {};
    public isNewSystem: boolean;
    public dialogHeader: string;
    public displayingClients: boolean;
    public displayClients: boolean;
    public clientsofSystem: any[] = [];
    public validationConstants = VALIDATION_MESSAGES;
    public valid = true;
    public messageList: Message[] = [];
    public canEdit = false;
    public editActions: any[] = [];
    public growlMessage: Message[] = [];
    public paginationCount: any;
    public rowsIn: number = 10;
    public systemsFiltered: any;

    /*------ end region public properties ------*/


    /*---------- region constructor ----------*/

    constructor(private systemsService: SystemsService, private router: Router,
        private userService: UserService, private ref: ChangeDetectorRef) {
    }

    /*-------- end region constructor --------*/


    /*------ region life cycle hooks ------*/
  
    ngOnInit(): void {
        this.getScreenActions();
        this.systems = [];
        this.getAllSystems();
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------end region life cycle hooks ------*/


    /*------ region service calls------*/

    /**
     * Gets Specialities
     */
    getAllSystems() {
        this.systemsService.getAllSystems().subscribe(
            (response: any) => {
                if (response != null) {
                    this.systems = response;
                    this.systems.forEach(x => {
                        x.isActive = (x.recordStatus === this.sharedLabels.ACTIVE_CHAR);
                        x.clientsCount = x.clients.length;
                    });
                    this.systems = this.systems.reverse();
                    this.systemsFiltered = this.systems;
                } else {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: ADMIN_SHARED.SEVERITY_ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            }
        );
    }

    /**
     * Saves changes to DB, made to systems
     * @param systems
     */
    public saveSystems(systems: SystemsModel[]) {
        this.systemsService.saveSystems(systems).subscribe(
            (success) => {
                if (success) {
                    this.getAllSystems();
                } else {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: ADMIN_SHARED.SEVERITY_ERROR, summary: '', detail: ADMIN_SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }

    /**
     * Get Edit Actions of Systems page
     */
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.SYSTEMS)
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
     * Activate or Inactivate System
     */
    activeOrInactiveSystem() {
        this.systemsService.activateOrInactivateSystems(this.selectedSystem).subscribe(
            data => {
                if (!data) {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_SAVE_MESSAGE });
                }
                this.displayClients = false;
                this.router.navigateByUrl(this.router.url);
            }
        );
    }

    /*------ end region service calls ------*/


    /*-------- region public methods --------*/

    /**
   * Called when pagechange event occurs
   */
    onTablePageChange(e) {
        let start = e.first + 1;
        let end = (e.first + e.rows) > this.systemsFiltered.length ? this.systemsFiltered.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.systemsFiltered.length;
    }

    /**
     * Displays Dialog for Adding new System
     */
    public showDialogToAdd() {
        this.isNewSystem = true;
        this.system = new SystemsModel();
        this.system.isActive = true;
        this.displayDialog = true;
        this.dialogHeader = this.sharedLabels.ADD + ' ' + this.labels.SYSTEM;
        this.focusSystem();
    }

    /**
     * focus on input field after the popup is shown
     */
    focusSystem() {
        this.ref.detectChanges();
        jQuery('.system').focus();
    }

    /**
     * displays a dialog on selecting a row
    **/
    onRowSelect(system: SystemsModel) {
        this.system = new SystemsModel();
        this.system.systemCode = system.systemCode;
        this.system.id = system.id;
        this.system.isActive = system.isActive;
        this.dialogHeader = this.sharedLabels.EDIT + ' ' + this.labels.SYSTEM;
        this.displayDialog = true;
        this.focusSystem();
    }

    /**
     * On Toggle of System
     * @param system
     */
    changeStateOfSystem(system: SystemsModel) {
        this.selectedSystem = system;
        if (!system.isActive) {
            this.systemsService.getClientsAssociatedWithSystem(system.id).subscribe(
                data => {
                    if (data != null && data.length > 0) {
                        this.clientsofSystem = data;
                        this.displayClients = true;
                    } else {
                        this.activeOrInactiveSystem();
                    }
                },
                err => {
                    console.log(err);
                }
            );
        } else {
            this.activeOrInactiveSystem();
        }
    }

    /**
     * keep Active System
     */
    keepActiveSystem() {
        this.displayClients = false;
        this.selectedSystem.isActive = true;
        this.clientsofSystem = [];
    }

    /**
     * Clear all the messages
     */
    clearMessages() {
        this.messageList = [];
        this.valid = true;
    }

    /**
     * Saves the System changes to db
     */
    saveSystemsChanges(systems: SystemsModel[]) {
        this.systemsService.saveSystems(systems).subscribe(
            (success) => {
                if (success) {
                    this.getAllSystems();
                } else {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );

    }

    /**
     * Add or Edit System
     */
    saveSystem() {
        if (this.system.systemCode) {
            this.system.systemCode = this.system.systemCode.trim();
        }
        this.valid = (this.system.systemCode && this.system.systemCode.length > 0);
        if (!this.valid) {
            return;
        }
       if (!this.isNewSystem && this.systems.some(val => _.toLower(val.systemCode) == _.toLower(this.system.systemCode) && val.id != this.system.id)) {
            this.messageList = [];
            this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SYSTEM_EXISTS });
        } else if (this.isNewSystem && this.systems.some(val => _.toLower(val.systemCode) == _.toLower(this.system.systemCode))) {
            this.messageList = [];
            this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SYSTEM_EXISTS });
        } else {
            let systemsObject: SystemsModel[];
            systemsObject = [];
            let systemObject: SystemsModel;

            if (this.isNewSystem) {
                systemObject = new SystemsModel();
                this.isNewSystem = false;
            } else {
                systemObject = this.systems.find(x => x.id == this.system.id);
            }
            systemObject.systemCode = this.system.systemCode;
            systemObject.systemName = this.system.systemCode;
            systemObject.systemDescription = this.system.systemCode;
            systemObject.isActive = this.system.isActive;
            systemObject.recordStatus = systemObject.isActive ? ADMIN_SHARED.ACTIVE_CHAR : ADMIN_SHARED.INACTIVE_CHAR;
            systemsObject.push(systemObject);
            this.systems = [];
            this.saveSystemsChanges(systemsObject);
            this.system = null;
            this.displayDialog = false;
        }
    }

    /**
     * On click of client count
     * @param system
     */
    onClientsColumnSelect(system) {
        this.displayingClients = true;
        this.system = system;
    }

    /**
     * Close the popup
     */
    closeDialog() {
        this.displayingClients = false;
    }

    /**
     * Called when filter event occurs
     */
    onTableFilter(event) {
        this.systemsFiltered = event.filteredValue;
    }
    /*------ end region public methods------*/

}
