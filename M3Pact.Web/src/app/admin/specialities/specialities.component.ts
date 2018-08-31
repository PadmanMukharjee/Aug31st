// Third party imports
import { Message } from 'primeng/components/common/api';
import { DropdownModule } from 'primeng/dropdown';
import * as _ from 'lodash';

// Angular imports
import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';

// Common File Imports
import { SpecialitiesService } from './specialities.service';
import { SpecialitiesModel } from '../specialities/specialities.model';
import { ADMIN_SPECIALITY, ADMIN_SHARED, SHARED } from '../../shared/utilities/resources/labels';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';

@Component({
    selector: 'specialities',
    templateUrl: './specialities.component.html',
    styleUrls: ['../../../assets/build/css/switch.css', './specialities.component.css'],
    providers: [SpecialitiesService]

})
export class SpecialitiesComponent implements OnInit, OnDestroy {

    /*------ region public properties ------*/
    public labels = ADMIN_SPECIALITY;
    public sharedLabels = ADMIN_SHARED;
    public validationConstants = VALIDATION_MESSAGES;
    public messageList: Message[] = [];
    public dialogHeader: string;
    public valid = true;
    public displayClients: boolean;
    public clientsOfSpecialty: any[] = [];
    public displayDialog: boolean;
    public specialities: SpecialitiesModel[];
    public speciality: SpecialitiesModel;
    public selectedSpeciality: any = {};
    public isNewSpeciality: boolean;
    public status = true;
    public canEdit = false;
    public editActions: any[] = [];
    public displayingClients: boolean;
    public growlMessage: Message[] = [];
    public paginationCount: any;
    public rowsIn: number = 10;
    public specialitiesFiltered: any;

    /*------ end region public properties ------*/


    /*---------- region constructor ----------*/

    constructor(private specialityService: SpecialitiesService, private userService: UserService,
        private ref: ChangeDetectorRef) {
    }

    /*-------- end region constructor --------*/

    /*------ region life cycle hooks ------*/

    ngOnInit(): void {
        this.specialities = [];
        this.getScreenActions();
        this.getSpecialities();
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------end region life cycle hooks ------*/

    /*------ region service calls------*/

    /**
     * Get Edit Actions of Specialties page
     */
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.SPECIALTIES)
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
     * Gets Specialities
     */
    getSpecialities() {
        this.specialityService.getSpecialities().subscribe(
            (response: any) => {
                if (response) {
                    if (response != null) {
                        this.specialities = response;
                        this.specialities.forEach(x => {
                            x.isActiveSpeciality = (x.recordStatus === this.sharedLabels.ACTIVE_CHAR);
                            x.clientsCount = x.clients.length;
                        });
                        this.specialities = this.specialities.reverse();
                        this.specialitiesFiltered = this.specialities;
                    } else {
                        this.growlMessage = [];
                        this.growlMessage.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                    }
                    let e = { first: 0, rows: this.rowsIn };
                    this.onTablePageChange(e);
                }
            }
        );
    }

    /**
     * Saves changes to DB, made to specialities
     * @param specialities
     */
    public saveSpecilities(specialities: SpecialitiesModel[]) {
        this.specialityService.saveSpecialities(specialities).subscribe(
            (success) => {
                if (success) {
                    this.getSpecialities();
                } else {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
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
        let end = (e.first + e.rows) > this.specialitiesFiltered.length ? this.specialitiesFiltered.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.specialitiesFiltered.length;
    }
    /**
     * Displays Dialog for Adding new speciality
     */
    public showDialogToAdd() {
        this.isNewSpeciality = true;
        this.speciality = new SpecialitiesModel();
        this.speciality.isActiveSpeciality = true;
        this.dialogHeader = this.sharedLabels.ADD + ' ' + this.labels.SPECIALITY;
        this.displayDialog = true;
        this.focusSpecialty();
    }

    /**
     * On Row selection
     * @param specialitySelected
     */
    public onRowSelect(specialitySelected: SpecialitiesModel) {
        this.isNewSpeciality = false;
        this.speciality = new SpecialitiesModel();
        this.speciality.specialityCode = specialitySelected.specialityCode;
        this.speciality.id = specialitySelected.id;
        this.speciality.isActiveSpeciality = specialitySelected.isActiveSpeciality;
        this.dialogHeader = this.sharedLabels.EDIT + ' ' + this.labels.SPECIALITY;
        this.displayDialog = true;
        this.focusSpecialty();
    }

    /**
     * On Toggle of Sepcialty
     * @param currentSpecialty
     */
    changeStateOfSpecialty(currentSpecialty: SpecialitiesModel) {
        this.selectedSpeciality = currentSpecialty;
        if (!currentSpecialty.isActiveSpeciality) {
            this.specialityService.getClientsAssociatedWithSpecialty(currentSpecialty.id).subscribe(
                data => {
                    if (data != null && data.length > 0) {
                        this.clientsOfSpecialty = data;
                        this.displayClients = true;
                    } else {
                        this.activeOrInactiveSpecialities(currentSpecialty);
                    }
                });
        } else {
            this.activeOrInactiveSpecialities(currentSpecialty);
        }
    }

    /**
     * Activate or Inactivate Specialties
     * @param specialty
     */
    activeOrInactiveSpecialities(specialty: SpecialitiesModel) {
        this.specialityService.activeOrInactiveSpecialities(specialty).subscribe(
            data => {
                if (!data) {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }

    /**
     * keep Active Specialty
     */
    keepActiveSpecialty() {
        this.displayClients = false;
        this.selectedSpeciality.isActiveSpeciality = true;
        this.clientsOfSpecialty = [];
    }

    /**
     * clear messages
     */
    clearMessages() {
        this.messageList = [];
        this.valid = true;
    }

    /**
     * Saves the changes made to specialites
     */
    public saveSpecialty() {
        this.messageList = [];
        if (this.speciality.specialityCode) {
            this.speciality.specialityCode = this.speciality.specialityCode.trim();
        }
        this.valid = (this.speciality.specialityCode && this.speciality.specialityCode.length > 0);
        if (!this.valid) {
            return;
        }
        let specialitiesObj: SpecialitiesModel[];
        if (!this.isNewSpeciality && this.specialities.some(val => _.toLower(val.specialityCode) == _.toLower(this.speciality.specialityCode) && val.id != this.speciality.id)) {
            this.messageList = [];
            this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SPECIALTY_EXISTS });
        } else if (this.isNewSpeciality && this.specialities.some(val => _.toLower(val.specialityCode) == _.toLower(this.speciality.specialityCode))) {
            this.messageList = [];
            this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SPECIALTY_EXISTS });
        } else {
            specialitiesObj = [];
            let specialityObject: SpecialitiesModel;

            if (this.isNewSpeciality) {
                specialityObject = new SpecialitiesModel();
                this.isNewSpeciality = false;
            } else {
                specialityObject = this.specialities.find(x => x.id == this.speciality.id);
            }

            specialityObject.id = this.speciality.id;
            specialityObject.specialityCode = this.speciality.specialityCode;
            specialityObject.specialityName = this.speciality.specialityCode;
            specialityObject.specialityDescription = this.speciality.specialityCode;
            specialityObject.isActiveSpeciality = this.speciality.isActiveSpeciality;
            specialityObject.recordStatus = specialityObject.isActiveSpeciality ? ADMIN_SHARED.ACTIVE_CHAR : ADMIN_SHARED.INACTIVE_CHAR;
            this.specialities.push(specialityObject);
            specialitiesObj.push(specialityObject);
            this.saveSpecilities(specialitiesObj);
            this.specialities = [];
            this.speciality = null;
            this.displayDialog = false;
        }
    }

    /**
     * focus on the input field after the popup is shown.
     */
    focusSpecialty() {
        this.ref.detectChanges();
        jQuery('.specialty').focus();
    }

    /**
     * On click of client count
     * @param speciality
     */
    onClientsColumnSelect(speciality) {
        this.displayingClients = true;
        this.speciality = speciality;
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
        this.specialitiesFiltered = event.filteredValue;
    }
    /*------ end region public methods------*/

}
