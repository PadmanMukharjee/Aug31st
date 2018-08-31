// Angular imports
import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { Router, ActivatedRoute, Params } from '@angular/router';

// Common File Imports
import { ADMIN_KPI, ADMIN_SHARED } from '../../../shared/utilities/resources/labels';
import { VALIDATION_MESSAGES } from '../../../shared/utilities/resources/constants';
import { SCREEN_CODE } from '../../../shared/utilities/resources/screencode';
import { KPIViewModel } from './kpi.model';
import { MeasureViewModel } from './measure.model';
import { ValidationMessageComponent } from '../../../common/components/validation-message/validation-message.component';
import { KPIService } from './kpi.service';
import { CheckListType } from '../../models/checklist-type.model';
import { debug } from 'util';
import { AppConstants } from '../../../app.constants';
import { UserService } from '../../../shared/services/user.service';
import { Observable } from 'rxjs/Observable';
import { Message } from 'primeng/components/common/api';

@Component({
    selector: 'app-kpi',
    templateUrl: './kpi.component.html',
    styleUrls: ['./kpi.component.css'],
    providers: [KPIService, CurrencyPipe]
})

export class KPIComponent implements OnInit {

    /*-----region Input/Output bindings -----*/
    @Input('kpiID') kpiID: string;
    /*-----endregion Input/Output bindings -----*/

    /*------ region Public properties ------*/
    public labels = ADMIN_KPI;
    public sharedLabels = ADMIN_SHARED;
    public validationConstants = VALIDATION_MESSAGES;
    public sourceOptions: CheckListType[];
    public kpi: KPIViewModel;
    public measureWhen: any[];
    public displayDialog: boolean;
    public dialogueMsg: string;
    public displayLabel: string;
    public sendToErrorMsg: string;
    public isCreateOperation = false;
    public measures: MeasureViewModel[];
    public showConfirmation: boolean;
    public confirmationMsg: string;
    public ID: number;
    public after: string;
    public alert: string;
    public isSaveClicked: boolean;
    public ifError: boolean;
    public canEdit: boolean;
    public isKPIDescription: boolean;

    public alertLevel: any[];
    public afterDropdown: any[];
    public escalateLimt: string;
    public kpiHeatMapItems: Array<number>;
    public enteredStandardAmountValid = false;
    public enteredStandardDayValid = false;
    public enteredStandardPercentageValid = false;
    public showDefaultStandard: boolean;
    public showKpiAmountMeasure: boolean;
    public showKpiDaysMeasure: boolean;
    public showKpiPercentageMeasure: boolean;
    public showMeasureError = false;
    public showDefaultStandardError = false;
    public disableSave = false;
    public messages: Message[] = [];

    /*------ end region Public properties ------*/


    /*------ region constructor ------*/
    constructor(private kpiService: KPIService, private router: Router, private route: ActivatedRoute, private _userService: UserService, private currencyPipe: CurrencyPipe) {
        this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
        };
        this.kpi = new KPIViewModel();
        this.kpiID = this.route.snapshot.queryParams['kpiID'];
        this.showConfirmation = false;
        this.isSaveClicked = false;
        this.ifError = false;
        this.kpi.measure = new MeasureViewModel();
        this.showDefaultStandard = true;
    }
    /*------ end region constructor ------*/

    /*-----region lifecycle events -----*/

    ngOnInit() {

        this.alertLevel = [
            { label: '>=', value: '>=' },
            { label: '<=', value: '<=' },
            { label: '=', value: '=' },
            { label: '>', value: '>' },
            { label: '<', value: '<' }
        ];
        this.afterDropdown = [
            { label: 'Days', value: 'Days' },
            { label: 'Weeks', value: 'Weeks' },
            { label: 'Months', value: 'Months' },
        ];
        if (this.kpiID !== null && this.kpiID !== '' && this.kpiID != undefined) {
            this.isCreateOperation = false;
            this.getUserById();
        } else {
            this.isCreateOperation = true;
        }
        this.getScreenActions();
        this.kpi.source = new CheckListType();
        this.kpi.source.checkListTypeID = 3;
        this.kpi.source.checkListTypeCode = this.labels.CHECKLIST_TYPE_M3;
        this.getQuestions();
    }
    /*-----end region lifecycle events -----*/


    /*------ region public methods ------*/

    /*
     * Getting action based on role.
     * */
    getScreenActions() {
        this._userService.getUserScreenActions(SCREEN_CODE.VIEWEDITKPI)
            .subscribe((actions) => {
                if (actions) {
                    if (actions.length === 0 && !this.isCreateOperation) {
                        this.canEdit = false;
                    } else {
                        this.canEdit = true;
                    }
                }
            });
    }

    /**
     * service call to get CheckList Types
     */
    getChecklistType() {
        this.kpiService.getChecklistTypes().subscribe(
            (response: any) => {
                if (response != null) {
                    this.sourceOptions = response;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.SEVERITY_ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
            }
        );
    }

    /**
     * to get user by given id in url
     */
    public getUserById() {
        this.kpi.measure = new MeasureViewModel();
        this.kpiService.getKPIById(this.kpiID).subscribe(
            (response: any) => {
                if (response != null) {
                    this.kpi = response;
                    this.showControlBasedOnUnit(this.kpi.measure.measureUnit);
                    if (this.kpi.measure.measureUnit === this.labels.KPI_UNIT_AMOUNT || this.kpi.measure.measureUnit === this.labels.KPI_UNIT_DAYS || this.kpi.measure.measureUnit === this.labels.KPI_UNIT_PERCENTAGE) {
                        this.showDefaultStandard = false;
                    }
                    this.getQuestions();
                    if (this.kpi.escalateTriggerTime) {
                        let escalateTriggerTimeArray = this.kpi.escalateTriggerTime.split(',');
                        this.escalateLimt = escalateTriggerTimeArray[0];
                        this.after = escalateTriggerTimeArray[1];
                    }
                    if (this.kpi.alertLevel) {
                        let alertLevelArray = this.kpi.alertLevel.split(',');
                        this.alert = this.alertLevel.find(x => x.label === alertLevelArray[0]).value;
                        this.kpi.alertLevel = alertLevelArray[1];
                    }
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.SEVERITY_ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
            }
        );
    }


    showControlBasedOnUnit(measureUnit: string) {
        if (this.isCreateOperation) {
            this.showDefaultStandard = false;
        }
        switch (measureUnit) {
            case this.labels.KPI_UNIT_AMOUNT:
                if (!this.isCreateOperation) {
                    this.kpi.standard = this.transformAmountToCurrency(this.kpi.standard);
                }
                this.showKpiAmountMeasure = true;
                break;
            case this.labels.KPI_UNIT_DAYS:
                this.showKpiDaysMeasure = true;
                break;
            case this.labels.KPI_UNIT_PERCENTAGE:
                if (!this.isCreateOperation) {
                    this.kpi.standard = (parseFloat(this.kpi.standard) * 100).toString();
                    this.kpi.standard = this.kpi.standard + '%';
                }
                this.showKpiPercentageMeasure = true;
                break;
        }
    }

    /**
     * service call to get kpi question based on checkList Type and
     * service call to get kpi measure based on checkList Type
     */
    getQuestions() {
        this.after = null;
        this.kpiService.getKPIQuestionsBasedOnSource(this.kpi.source.checkListTypeCode).subscribe(
            (response: any) => {
                if (response != null) {
                    this.measures = response;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.SEVERITY_ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
            }
        );

        this.kpiService.getMeasureBasedOnClientTypeID(this.kpi.source.checkListTypeID).subscribe(
            (response: any) => {
                if (response != null) {
                    this.measureWhen = response;
                    if (this.measureWhen.length == 1) {
                        this.kpi.kpiMeasure = [];
                        this.kpi.kpiMeasure = this.measureWhen[0];
                    } else if (this.isCreateOperation) {
                        this.kpi.kpiMeasure = null;
                    }
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
            }
        );
        if (this.kpi.source.checkListTypeCode == 'WEEK') {
            this.after = 'Weeks';
        } else if (this.kpi.source.checkListTypeCode == 'MONTH') {
            this.after = 'Months';
        }
    }

    /**
     * assigning remining properties based on question selection
     */
    OnQuestionSelect() {
        this.showMeasureError = false;
        this.ID = this.measures.find(x => x.measureId == this.kpi.measure.measureId).kpiid;
        if (this.ID) {
            this.showConfirmation = true;
            this.confirmationMsg = AppConstants.kpiConstants.confirmationMsg;
        } else {
            let measure = this.kpi.measure;
            this.resetProperties();
            this.kpi.measure = measure;
            this.isCreateOperation = true;
            this.showControlBasedOnUnit(this.kpi.measure.measureUnit);
        }
    }

    /*
     * Setting HeatMapScore
     * */
    settingHeatMapScore() {
        if (this.kpi.isHeatMapItem) {
            this.kpi.isUniversal = true;
            this.kpi.heatMapScore = 20;
        } else {
            this.kpi.isUniversal = false;
            this.kpi.heatMapScore = 0;
            this.kpi.isHeatMapItem = false;
        }
    }

    /*
     * Adding validation to sendTO
     * */
    sendToSelection() {
        if (this.isSaveClicked && this.kpi.sendAlert && !this.kpi.sendToRelationshipManager && !this.kpi.sendToBillingManager) {
            this.sendToErrorMsg = this.validationConstants.SENDTO_CHECKBOX_ERROR;
            this.ifError = true;
        } else {
            this.sendToErrorMsg = '';
            this.ifError = false;
        }
    }

    /*
     *
     * */
    public noWhitespaceValidator() {
        let isWhitespace = (this.kpi.kpiDescription || '').trim().length === 0;
        let isValid = !isWhitespace;
        return isValid ? null : true;
    }

    /*
     * Checking in db wheather kpi description already exits.
     * */
    ifKPIDescriptionExists() {
        this.kpiService.getAllKPIs().subscribe(
            (response: any) => {
                if (response) {
                    this.isKPIDescription = response.find(x => x.kpiDescription.toLocaleLowerCase() == this.kpi.kpiDescription.toLocaleLowerCase().trim() && x.kpiid != this.kpiID);
                    if (!this.isKPIDescription) {
                        if (this.kpi.isHeatMapItem == false) {
                            this.getKpiHeatMapItems();
                        } else {
                            this.save();
                        }
                    }
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.SEVERITY_ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
            });
    }

    /*
     * Removing validation of KPI description text box on focus
     * */
    removeValidationMessage() {
        this.isKPIDescription = false;
    }

    /**
     * To check validation
     * @param kpiDetailsForm
     */
    saveKPIDetails(kpiDetailsForm: NgForm) {
        this.disableSave = true;
        this.isSaveClicked = true;
        let formValid = this.checkKpiMeasureAndStandardValid();
        this.sendToSelection();
        if (kpiDetailsForm.valid && !this.ifError && kpiDetailsForm.dirty && !this.enteredStandardAmountValid && !this.enteredStandardDayValid && !this.enteredStandardPercentageValid && formValid) {
            this.ifKPIDescriptionExists();
        } else {
            this.disableSave = false;
        }
    }

    checkKpiMeasureAndStandardValid() {
        let returnValid = true;
        if (this.kpi.measure.measureId === undefined || this.kpi.standard === null) {
            this.showMeasureError = true;
            returnValid = false;
        }
        if (this.kpi.standard === undefined || this.kpi.standard === null || this.kpi.standard === '') {
            this.showDefaultStandardError = true;
            returnValid = false;
        }
        return returnValid;
    }

    /*
     * Setting after text message.
     * */
    afterMessage() {
        if (this.kpi.kpiMeasure.kpimeasureId == 3) {
            this.after = 'days';
        } else if (this.kpi.kpiMeasure.kpimeasureId == 4) {
            this.after = 'weeks';
        } else if (this.kpi.kpiMeasure.kpimeasureId == 5) {
            this.after = 'months';
        }
    }

    /*
     * Api service call to save details.
     * */
    save() {
        if (this.kpi.escalateAlert) {
            this.kpi.escalateTriggerTime = this.escalateLimt + ',' + this.after;
        }
        this.kpi.alertLevel = this.alert + ',' + this.kpi.alertLevel;
        this.kpi.recordStatus = 'A';
        this.kpi.kpiDescription = this.kpi.kpiDescription.trim();
        if (this.kpi.measure.measureUnit === this.labels.KPI_UNIT_AMOUNT) {
            this.kpi.standard = this.transformToNumber(this.kpi.standard).toString();
        }
        if (this.kpi.measure.measureUnit === this.labels.KPI_UNIT_PERCENTAGE) {
            this.kpi.standard = this.kpi.standard.slice(0, -1);
            this.kpi.standard = (parseFloat(this.kpi.standard) / 100).toFixed(4);
        }
        if (this.kpi.measure.measureUnit === this.labels.KPI_UNIT_AMOUNT || this.kpi.measure.measureUnit === this.labels.KPI_UNIT_PERCENTAGE || this.kpi.measure.measureUnit === this.labels.KPI_UNIT_DAYS) {
            this.kpi.alertLevel = this.alert + ',' + this.kpi.standard;
        }
        this.settingHeatMapScore();
        this.kpiService.saveKPIDetails(this.kpi).subscribe((responce: any) => {
            if (responce) {
                this.displayDialog = true;
                this.dialogueMsg = 'KPI Details ' + (this.isCreateOperation ? this.sharedLabels.SUCCESSMESSAGE : this.sharedLabels.UPDATEMESSAGE);
            } else {
                this.displayDialog = true;
                this.dialogueMsg = this.sharedLabels.ERROR_SAVE_MESSAGE;
            }
        });
        this.disableSave = false;
    }

    /**
     * To get the heat map items - service call.
     */
    getHeatMapItems() {
        return Observable.forkJoin(
            this.kpiService.getKpiHeatMapItems()
        );
    }

    /**
     * Show message if the kpi is a heat map items.
     */
    getKpiHeatMapItems() {
        this.getHeatMapItems().subscribe(
            data => {
                this.kpiHeatMapItems = data[0];
                if (this.kpiHeatMapItems.includes(this.kpi.kpiid)) {
                    this.displayDialog = true;
                    this.dialogueMsg = this.labels.KPI_HEATMAP_MESSAGE;
                    this.disableSave = false;
                } else {
                    this.save();
                }
            });
    }

    /**
    * closes the dialog on clicking ok button
    **/
    displayDialogclose() {
        this.displayDialog = false;
        this.router.navigateByUrl('/administration/kpi/createKpi');
    }

    /*
     * Resets form
     * */
    reset() {
        this.kpi = new KPIViewModel();
        this.router.navigateByUrl('/administration/kpi/createKpi');
    }

    /**
     * click of yes or no on COnfirmation popup
     * @param yorn
     */
    confirmationClick(yorn) {
        if (yorn === 'y') {
            this.router.navigateByUrl('/administration/kpi/createKpi?kpiID=' + this.ID);
        } else {
            this.resetProperties();
        }
        this.showConfirmation = false;
    }

    /*
     * resetting properties BasedOnQuestionSelection
     * */
    resetProperties() {
        this.showKpiDaysMeasure = false;
        this.showKpiAmountMeasure = false;
        this.showKpiPercentageMeasure = false;
        this.showDefaultStandard = true;
        let source = this.kpi.source;
        this.kpi = new KPIViewModel();
        this.kpi.source = source;
        this.after = null;
        this.escalateLimt = null;
    }

    onBlur(element: any, index: number) {
        let number = this.transformToNumber(element.target.value);
        if (number > 0) {
            element.target.value = this.transformAmountToCurrency(number);
            this.kpi.standard = element.target.value;
        }
    }
    onfocus(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
        }
    }

    onkeyUp(element: any, index: number) {
        this.checkValidStandard();
        let regex = /^[+]?[0-9]{1,10}(?:\.[0-9]{1,2})?$/;
        if (regex.test(element.target.value.trim()) || element.target.value.trim() == '') {
            this.enteredStandardAmountValid = false;
        } else {
            this.enteredStandardAmountValid = true;
        }
    }

    transformToNumber(amount: any) {
        if (amount != null) {
            let number = parseFloat(amount.replace(/[^0-9-.]/g, ''));
            if (!isNaN(number)) {
                return number;
            }
        } else {
            return 0;
        }
    }

    transformAmountToCurrency(amount: any) {
        return this.currencyPipe.transform(amount, 'USD');
    }

    daysKeyUp(element: any, index: number) {
        this.checkValidStandard();
        if (this.kpi.measure.measureUnit === this.labels.KPI_UNIT_DAYS && element.target.value.trim() > 1000) {
            this.enteredStandardDayValid = true;
        } else {
            this.enteredStandardDayValid = false;
        }
    }

    percentageOnBlur(element: any) {
        if (element.target.value != '' && element.target.value.indexOf('%') < 0) {
            this.percentageOnkeyUp(element);
            element.target.value = element.target.value + '%';
            this.kpi.standard = element.target.value;
        }
    }

    percentageOnkeyUp(element: any) {
        if (element.target.value != '') {
            this.checkValidStandard();
            let regex = /^(?:100(?:\.00?)?|\d?\d(?:\.\d\d?)?)$/;
            if (regex.test(element.target.value) || element.target.value.trim() == '') {
                this.enteredStandardPercentageValid = false;
            } else {
                this.enteredStandardPercentageValid = true;
            }
        }
    }

    percentageOnfocus(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
            this.percentageOnkeyUp(element);
        }
    }

    checkValidStandard() {
        if (this.kpi.standard.toString().trim().length !== 0) {
            this.showDefaultStandardError = false;
        }
    }

    /*------ end region public methods ------*/
}
