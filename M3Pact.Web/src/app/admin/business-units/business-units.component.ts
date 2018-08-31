// Angular imports
import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';

// Third party imports
import { DropdownModule } from 'primeng/dropdown';
import { Message } from 'primeng/components/common/api';
import * as _ from 'lodash';

// Common File Imports
import { BusinessUnitsService } from './busienss-units.service';
import { BusinessUnitViewModel } from './business-units.model';
import { ADMIN_BUSINESS_UNIT, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';

@Component({
    selector: 'app-businessunits',
    templateUrl: './business-units.component.html',
    styleUrls: ['../../../assets/build/css/switch.css', './business-units.component.css'],
    providers: [BusinessUnitsService]

})
export class BusinessUnitsComponent implements OnInit, OnDestroy {

    /*------ region public properties ------*/
    public labels = ADMIN_BUSINESS_UNIT;
    public sharedLabels = ADMIN_SHARED;
    public clientsOfBusinessUnit: any[] = [];
    public displayClients: boolean;
    public validationConstants = VALIDATION_MESSAGES;
    public msgs: Message[] = [];
    public messages: Message[] = [];
    public valid = true;
    public requiredMessage: string;
    public businessUnits: BusinessUnitViewModel[];
    public businessUnit: BusinessUnitViewModel;
    public selectedBusinessUnit: any;
    public cols: any[];
    public displayDialog: boolean;
    public isAddBusinessUnit: boolean;
    public popupHeading: string;
    public canEdit = false;
    public editActions: any[] = [];
    public displayingClients: boolean;
    public paginationCount: any;
    public rowsIn: number = 10;
    public businessUnitsFiltered: any;
    /*------ end region public properties ------*/


    /*------ region constructor ------*/

    constructor(private businessUnitsService: BusinessUnitsService, private userService: UserService,
        private ref: ChangeDetectorRef) {
    }

    /*------ end region constructor ------*/


    /*------ region life cycle hooks ------*/
   
  ngOnInit() {
        this.businessUnits = [];
        this.getScreenActions();
        this.getBusinessUnits();
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------ end region life cycle hooks ------*/


    /*------ region service calls------*/

    /**
     * gets the business units data from db
    **/
    getBusinessUnits() {
        this.businessUnitsService.getBusinessUnits().subscribe(
            (response: any) => {
                if (response != null) {
                    this.businessUnits = response;
                    this.businessUnits.forEach(x => {
                        x.isActive = (x.recordStatus === ADMIN_SHARED.ACTIVE_CHAR);
                        x.clientsCount = x.clients.length;
                    });
                    this.businessUnits = this.businessUnits.reverse();
                    this.businessUnitsFiltered = this.businessUnits;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            }
        );
    }
 
    /**
     * Get Edit Actions of Business Units page
     */
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.BUSINESSUNITS)
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
     * saves Business Units Changes to db
     * @param businessUnits
     */
    saveBusinessUnitsChanges(businessUnits: BusinessUnitViewModel[]) {
        this.businessUnitsService.saveBusinessUnits(businessUnits).subscribe(
            (success) => {
                if (success) {
                    this.getBusinessUnits();
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }

    /**
     * On Toggle of Busniess Unit
     * @param businessUnit
     */
    changeStateOfBusinessUnit(businessUnit: BusinessUnitViewModel) {
        this.selectedBusinessUnit = businessUnit;
        if (!businessUnit.isActive) {
            this.businessUnitsService.getClientsAssociatedWithBusinessUnit(businessUnit.id).subscribe(
                data => {
                    if (data != null && data.length > 0) {
                        this.clientsOfBusinessUnit = data;
                        this.displayClients = true;
                    } else {
                        this.activeOrInactiveBusinessUnit();
                    }
                }
            );
        } else {
            this.activeOrInactiveBusinessUnit();
        }
    }

    /**
     * Activate or Inactivate Business Unit
     */
    activeOrInactiveBusinessUnit() {
        this.businessUnitsService.activeOrInactiveBusinessUnit(this.selectedBusinessUnit).subscribe(
            (succes) => {
                if (!succes) {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_SAVE_MESSAGE });
                }
                this.displayClients = false;
                this.clientsOfBusinessUnit = [];
            }
        );
    }

    /*------ end region service calls------*/


    /*------ region public methods ------*/

     /**
     * Called when pagechange event occurs
     */
    onTablePageChange(e) {
            let start = this.businessUnitsFiltered.length >0 ? e.first + 1: e.first;
            let end = (e.first + e.rows) > this.businessUnitsFiltered.length ? this.businessUnitsFiltered.length : (e.first + e.rows);
           this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.businessUnitsFiltered.length;
    }

    /**
    * Called when filter event occurs
    */
    onTableFilter(event) {
        this.businessUnitsFiltered = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };
        this.onTablePageChange(e);
    }

    /**
     * displays a dialog on clicking add button
    **/
    showDialogToAdd() {
        this.isAddBusinessUnit = true;
        this.businessUnit = new BusinessUnitViewModel();
        this.businessUnit.isActive = true;
        this.popupHeading = this.labels.ADD + ' ' + this.labels.BUSINESS_UNIT;
        this.displayDialog = true;
        this.focusBusinessUnit();
    }

    /**
     * displays a dialog on selecting a row
     * @param unit
     */
    onRowSelect(unit: BusinessUnitViewModel) {
        this.businessUnit = new BusinessUnitViewModel();
        this.businessUnit.businessUnitCode = unit.businessUnitCode;
        this.businessUnit.id = unit.id;
        this.businessUnit.isActive = unit.isActive;
        this.popupHeading = this.labels.EDIT_BUSINESS_UNIT_DETAILS;
        this.displayDialog = true;
        this.focusBusinessUnit();
    }

    /**
     * Make BusinessUnit Active
     */
    keepActiveBusinessUnit() {
        this.displayClients = false;
        this.selectedBusinessUnit.isActive = true;
        this.clientsOfBusinessUnit = [];
    }

    /**
     * added/edited changes to busines units will be saved
    **/
    saveBusinessUnit() {
        if (this.businessUnit.businessUnitCode) {
            this.businessUnit.businessUnitCode = this.businessUnit.businessUnitCode.trim();
        }
        this.valid = (this.businessUnit.businessUnitCode && this.businessUnit.businessUnitCode.length > 0);
        if (!this.valid) {
            return;
        }
        let units = [...this.businessUnits];
        if (!this.isAddBusinessUnit && this.businessUnits.some(val => _.toLower(val.businessUnitCode) == _.toLower(this.businessUnit.businessUnitCode) && val.id != this.businessUnit.id)) {
            this.msgs = [];
            this.msgs.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.BUSINESSUNIT_EXISTS });
        } else if (this.isAddBusinessUnit && this.businessUnits.some(val => _.toLower(val.businessUnitCode) == _.toLower(this.businessUnit.businessUnitCode))) {
            this.msgs = [];
            this.msgs.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.BUSINESSUNIT_EXISTS });
        } else {
            let businessUnitsObject: BusinessUnitViewModel[];
            businessUnitsObject = [];
            let businessUnitObject: BusinessUnitViewModel;

            if (this.isAddBusinessUnit) {
                businessUnitObject = new BusinessUnitViewModel();
                this.isAddBusinessUnit = false;
            } else {
                businessUnitObject = this.businessUnits.find(x => x.id == this.businessUnit.id);
            }

            businessUnitObject.businessUnitCode = this.businessUnit.businessUnitCode;
            businessUnitObject.businessUnitName = this.businessUnit.businessUnitCode;
            businessUnitObject.businessUnitDescription = this.businessUnit.businessUnitCode;
            businessUnitObject.isActive = this.businessUnit.isActive;
            businessUnitObject.recordStatus = businessUnitObject.isActive ? this.sharedLabels.ACTIVE_CHAR : this.sharedLabels.INACTIVE_CHAR;
            businessUnitsObject.push(businessUnitObject);
            this.businessUnits = [];
            this.saveBusinessUnitsChanges(businessUnitsObject);
            this.businessUnits = units;
            this.businessUnit = null;
            this.displayDialog = false;
        }
    }

    /**
     * Clear all the messages
     */
    clearMessages() {
        this.msgs = [];
        this.valid = true;
    }

    /**
     * focus on input field after the popup is shown
     */
    focusBusinessUnit() {
        this.ref.detectChanges();
        jQuery('#businessUnit').focus();
    }

    /**
     * On click of client count
     * @param businessUnit
     */
    onClientsColumnSelect(businessUnit) {
        this.displayingClients = true;
        this.businessUnit = businessUnit;
    }

    /**
     * Close Clients Popup
     */
    closeClients() {
        this.displayingClients = false;
    }
    /*------ end region public methods ------*/

}
