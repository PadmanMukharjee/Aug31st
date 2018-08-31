import { AssignClientUserService } from '../assign-client-user/assign-client-user.service';
import { KPIService } from '../../admin/kpi/create-kpi/kpi.service';
// Angular imports
import { Component, OnInit, EventEmitter, Output, style, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { Message } from 'primeng/components/common/api';

// Common File Imports
import { CLIENT_KPI_SETUP, CLIENT_SHARED } from '../../shared/utilities/resources/labels';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { ClientKPISetupViewModel, KPISetupViewModel, KPIViewModel } from './kpi-setup.model';
import { AllUsersViewModel } from '../../admin/users/all-users/models/all-users-viewmodel.model';
import { ValidationMessageComponent } from '../../common/components/validation-message/validation-message.component';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { ADMIN_KPI, ADMIN_SHARED, SHARED } from '../../shared/utilities/resources/labels';


@Component({
    selector: 'kpi-setup',
    templateUrl: './kpi-setup.component.html',
    styleUrls: ['./kpi-setup.component.css'],
    providers: [CurrencyPipe],
})
export class KPISetupComponent implements OnInit {

    /*-----region Input/Output bindings -----*/
    @Output('saveKPISetup') SaveKPISetup = new EventEmitter<any>();
    @Output('saveKPISetupStatusDetail') SaveKPISetupStatusDetail = new EventEmitter<any>();
    @Input('clientCode') clientCode: string;
    /*-----endregion Input/Output bindings -----*/

    /*------ region public properties ------*/
    public labels = CLIENT_KPI_SETUP;
    public adminLabels = ADMIN_KPI;
    public sharedLabels = CLIENT_SHARED;
    public canEdit = false;
    public editActions: any[] = [];
    public popupHeader: string;
    public displayDialog = false;
    public isUniversal = false;
    public clientAssignedKpi: ClientKPISetupViewModel;
    public clientKpi: ClientKPISetupViewModel;
    public kpiQuestion: KPISetupViewModel;
    public alertLevel: any[];
    public alert: string;
    public kpiQuestionOptions: KPIViewModel[];
    public clientKpiQuestionOptions: KPIViewModel[];
    public users: any[];
    public selectedKpi: KPISetupViewModel;
    public selectedUsers: AllUsersViewModel[];
    public displayKpiUsers: boolean;
    public validationConstants = VALIDATION_MESSAGES;
    public valid = true;
    public displayNoKPIToAdd = false;
    public enteredStandardAmountValid = false;
    public enteredStandardDayValid = false;
    public enteredStandardPercentageValid = false;
    public showDefaultStandard: boolean;
    public showKpiAmountMeasure = false;
    public showKpiDaysMeasure = false;
    public showKpiPercentageMeasure = false;
    public showDefaultStandardError = false;
    public messages: Message[] = [];
    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(private router: Router, private userService: UserService, private _KPIService: KPIService, private _clientUserService: AssignClientUserService, private currencyPipe: CurrencyPipe) {
        this.getScreenActions();
        this.clientKpi = new ClientKPISetupViewModel();
        this.clientAssignedKpi = new ClientKPISetupViewModel();
        this.kpiQuestion = new KPISetupViewModel();
        this.kpiQuestionOptions = new Array<KPIViewModel>();
        this.users = new Array<AllUsersViewModel>();
    }

    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit(): void {
        if (this.clientCode !== null && this.clientCode !== '' && this.clientCode != undefined) {
            this.getAssignedKPIsForClient();
            this.saveKPIStepAsCompletedWhenNotCompleted();
        }

        this.alertLevel = [
            { label: '>=', value: '>=' },
            { label: '<=', value: '<=' },
            { label: '=', value: '=' },
            { label: '>', value: '>' },
            { label: '<', value: '<' }
        ];
    }
    /*------ region life cycle hooks ------*/

    /*------ region Public methods ------*/

    // Save KPI Step as Completed in db (as this step has universal kpis always)
    saveKPIStepAsCompletedWhenNotCompleted() {
        let currentStep = jQuery('.progress-indicator li.active');
        if (currentStep) {
            let stepID = currentStep.attr('id');
            let isCompletedStep = currentStep.hasClass('completed');
            if (stepID == this.labels.CONSTANT.KPI_STEP_ID && !isCompletedStep) {
                this.SaveKPISetupStatusDetail.emit();
            }
        }
    }

    // Get Edit Actions of KPi page
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.KPISETUP)
            .subscribe((actions) => {
                if (actions) {
                    if (actions.length === 0) {
                        this.canEdit = false;
                    } else {
                        this.canEdit = true;
                        this.editActions = actions;
                        if (this.clientCode !== null && this.clientCode !== '' && this.clientCode != undefined) {
                            this.getKPIQuestions();
                        }
                    }
                }
            });
    }

    getAssignedKPIsForClient() {
        this._KPIService.getAssignedKPIsForClient(this.clientCode).subscribe(
            data => {
                if (data !== null) {
                    for (let j = 0; j < data.kpiQuestions.length; j++) {
                        if (data.kpiQuestions[j].kpi.checklistTypeViewModel.checkListTypeCode === 'M3' && data.kpiQuestions[j].kpi.m3MeasureViewModel !== null) {
                            if (data.kpiQuestions[j].kpi.m3MeasureViewModel.measureUnit !== null) {
                                this.formClientKpiGridData(data , j);
                            }
                        }
                    }
                    this.clientAssignedKpi = data;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: SHARED.SEVERITY_ERROR, summary: 'KPI Assign', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }



    formClientKpiGridData(data: any , j: number) {
        if (data.kpiQuestions[j].kpi.m3MeasureViewModel.measureUnit === this.adminLabels.KPI_UNIT_PERCENTAGE) {
            if (data.kpiQuestions[j].clientStandard === '' || data.kpiQuestions[j].clientStandard === null) {
                data.kpiQuestions[j].clientStandard = '';
            } else {
                let tempClientStanadard = data.kpiQuestions[j].clientStandard.split(' ');
                data.kpiQuestions[j].clientStandard = tempClientStanadard[0] + ' ' + ((parseFloat(tempClientStanadard[1]) * 100).toFixed(3).slice(0, -1)).toString();
            }
            if (data.kpiQuestions[j].alertValue === '' || data.kpiQuestions[j].alertValue === null) {
                data.kpiQuestions[j].alertValue = '';
            } else {
                data.kpiQuestions[j].alertValue = ((parseFloat(data.kpiQuestions[j].alertValue) * 100).toFixed(3).slice(0, -1)).toString();
            }
            if (data.kpiQuestions[j].kpi.companyStandard === '' || data.kpiQuestions[j].kpi.companyStandard === null) {
                data.kpiQuestions[j].kpi.companyStandard = '';
            } else {
                let tempCompanyStandard = data.kpiQuestions[j].kpi.companyStandard.split(' ');
                data.kpiQuestions[j].kpi.companyStandard = tempCompanyStandard[0] + ' ' + ((parseFloat(tempCompanyStandard[1]) * 100).toFixed(3).slice(0, -1)).toString();
            }
        }
        if (data.kpiQuestions[j].kpi.m3MeasureViewModel.measureUnit === this.adminLabels.KPI_UNIT_AMOUNT) {
            if (data.kpiQuestions[j].clientStandard !== '' || data.kpiQuestions[j].clientStandard !== null) {
                let clientStandard = data.kpiQuestions[j].clientStandard.split(' ');
                let currencyClientValue = this.transformAmountToCurrency(parseFloat(clientStandard[1]));
                if (currencyClientValue !== null) {
                    data.kpiQuestions[j].clientStandard = clientStandard[0] + ' ' + currencyClientValue.toString();
                }
            }
            if (data.kpiQuestions[j].kpi.companyStandard !== '' || data.kpiQuestions[j].kpi.companyStandard !== null) {
                let companyStandard = data.kpiQuestions[j].kpi.companyStandard.split(' ');
                let currencyCompanyValue = this.transformAmountToCurrency(parseFloat(companyStandard[1]));
                if (currencyCompanyValue !== null) {
                    data.kpiQuestions[j].kpi.companyStandard = companyStandard[0] + ' ' + currencyCompanyValue.toString();
                }
            }
        }
    }


    /**
     * To add new KPI
     */
    showDialogToAdd() {
        this.popupHeader = this.labels.ADD_NEW_KPI_SETUP;
        this.kpiQuestion = new KPISetupViewModel();
        this.kpiQuestion.kpi = null;
        this.kpiQuestionOptions = this.clientKpiQuestionOptions;
        for (let j = 0; j < this.clientAssignedKpi.kpiQuestions.length; j++) {
            this.kpiQuestionOptions = this.kpiQuestionOptions.filter(c => c.kpiId != this.clientAssignedKpi.kpiQuestions[j].kpi.kpiId);
        }
        if (this.kpiQuestionOptions.length > 0) {
            this.displayDialog = true;
        } else {
            this.displayNoKPIToAdd = true;
        }
    }

    OnKpiSelect() {
        if (this.kpiQuestion.kpi.checklistTypeViewModel.checkListTypeCode === 'M3') {
            this.resetStandard();
            this.showStandardBasedOnMeasureUnit(this.kpiQuestion.kpi.m3MeasureViewModel.measureUnit);
        } else {
            this.resetStandard();
            this.showDefaultStandard = true;
        }
    }

    /**
     * To Edit KPI
     * @param editKpiQuestion
     */
    onRowSelect(editKpiQuestion: KPISetupViewModel) {
        this.resetStandard();
        this.popupHeader = this.labels.EDIT_KPI_SETUP;
        this.kpiQuestion = new KPISetupViewModel();
        this.kpiQuestion = Object.assign({}, editKpiQuestion);

        this.kpiQuestionOptions = this.clientKpiQuestionOptions;
        for (let i = 0; i < this.clientAssignedKpi.kpiQuestions.length; i++) {
            if (this.kpiQuestion.kpi.kpiId != this.clientAssignedKpi.kpiQuestions[i].kpi.kpiId) {
                this.kpiQuestionOptions = this.kpiQuestionOptions.filter(c => c.kpiId != this.clientAssignedKpi.kpiQuestions[i].kpi.kpiId);
            }
        }
        if (this.kpiQuestion.kpi.checklistTypeViewModel.checkListTypeCode === 'M3') {
            if (this.kpiQuestion.kpi.m3MeasureViewModel !== null) {
                if (this.kpiQuestion.kpi.m3MeasureViewModel.measureUnit !== null) {
                    this.showStandardBasedOnMeasureUnit(this.kpiQuestion.kpi.m3MeasureViewModel.measureUnit);
                }
            } else {
                this.showDefaultStandard = true;
            }
        }
        this.isUniversal = this.kpiQuestion.kpi.isUniversal;
        this.displayDialog = true;
    }

    disbleEdit() {
        this.displayDialog = false;
        this.valid = true;
    }


    resetStandard() {
        this.showDefaultStandard = false;
        this.showDefaultStandardError = false;
        this.showKpiDaysMeasure = false;
        this.showKpiAmountMeasure = false;
        this.showKpiPercentageMeasure = false;
        this.enteredStandardAmountValid = false;
        this.enteredStandardDayValid = false;
        this.enteredStandardPercentageValid = false;
    }

    showStandardBasedOnMeasureUnit(measure: string) {
        switch (measure) {
            case this.adminLabels.KPI_UNIT_AMOUNT:
                this.showKpiAmountMeasure = true;
                this.kpiQuestion.alertValue = this.transformAmountToCurrency(this.kpiQuestion.alertValue);
                break;
            case this.adminLabels.KPI_UNIT_DAYS:
                this.showKpiDaysMeasure = true;
                break;
            case this.adminLabels.KPI_UNIT_PERCENTAGE:
                this.showKpiPercentageMeasure = true;
                if (this.kpiQuestion.alertValue !== '') {
                    this.kpiQuestion.alertValue = this.kpiQuestion.alertValue + '%';
                }
                break;
            case null:
                this.showDefaultStandard = true;
        }
    }

    /**
     * To get all the KPIs which are related to Client Weekly and Monthly checklist and M3Metrics
     * @param clientCode
     */
    getKPIQuestions() {
        this._KPIService.getKPIQuestionsForClient(this.clientCode).subscribe(
            data => {
                if (data != null) {
                    this.kpiQuestionOptions = data;
                    this.clientKpiQuestionOptions = data;
                    this.getM3UsersToAssign();
                } else {
                    this.messages = [];
                    this.messages.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }
            }
        );
    }

    /**
     * Get M3 Users to assign
     */
    getM3UsersToAssign() {
        this._clientUserService.getAllM3Users().subscribe(
            users => {
                if (users != null) {
                    for (let user of users) {
                        this.users.push({ label: user.lastName + ' ' + user.firstName, value: user });
                    }
                } else {
                    this.messages = [];
                    this.messages.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }
            }
        );
    }

    /**
     * Save kpi to client
     */
    saveKPISetup(kpiForm) {
            if (this.kpiQuestion.kpi == null) {
                this.valid = false;
                return;
             }
            if (this.kpiQuestion.kpi.checklistTypeViewModel.checkListTypeCode == 'M3') {
                if (this.checkValidClientStandard()) {
                    this.valid = false;
                    return;
                }
            }
            this.clientKpi = new ClientKPISetupViewModel();
            this.clientKpi.clientCode = this.clientCode;
            if (this.kpiQuestion.kpi.checklistTypeViewModel.checkListTypeCode == 'M3') {
                if ((this.kpiQuestion.alertLevel !== null && this.kpiQuestion.alertLevel !== '' && this.kpiQuestion.alertLevel != undefined) &&
                    (this.kpiQuestion.alertValue !== null && this.kpiQuestion.alertValue !== '' && this.kpiQuestion.alertValue != undefined)) {
                    this.manageClientAlertData(this.kpiQuestion.kpi.m3MeasureViewModel.measureUnit);
                    this.kpiQuestion.clientStandard = this.kpiQuestion.alertLevel + ',' + this.kpiQuestion.alertValue;
                } else {
                    this.kpiQuestion.clientStandard = '';

                }
            } else {
                this.kpiQuestion.clientStandard = null;
                this.kpiQuestion.sla = null;
            }
            this.clientKpi.kpiQuestions.push(this.kpiQuestion);
            this.SaveKPISetup.emit(this.clientKpi);
    }


     /**
     * Manages ClientAlertData
     */
    manageClientAlertData(measure: string) {
        switch (measure) {
            case this.adminLabels.KPI_UNIT_AMOUNT:
                this.kpiQuestion.alertValue = this.transformToNumber(this.kpiQuestion.alertValue).toString();
                break;
            case this.adminLabels.KPI_UNIT_PERCENTAGE:
                this.kpiQuestion.alertValue = this.kpiQuestion.alertValue.slice(0, -1);
                this.kpiQuestion.alertValue = (parseFloat(this.kpiQuestion.alertValue) / 100).toFixed(4);
                break;
        }
    }

    checkValidClientStandard() {
        if (this.enteredStandardAmountValid || this.enteredStandardDayValid || this.enteredStandardPercentageValid) {
            return true;
        }
        return false;
    }

    onSelectUsers(event) {
        this.selectedKpi = event;
        this.selectedUsers = this.selectedKpi.sendTo;
        this.displayKpiUsers = true;
    }

    closeUserDialog() {
        this.displayKpiUsers = false;
    }

    kpiSelected() {
        this.valid = true;
    }

    displayNoKPIToAddDialog() {
        this.displayNoKPIToAdd = false;
    }

   public restrictNumeric(e, cntrlVal: string) {
        let isValidChar = false;
        let idx_decimal = -1;
        let input = String.fromCharCode(e.which);

        if (cntrlVal) {
            idx_decimal = cntrlVal.indexOf('.');
        }
        if (e.key == '.') {
            if ((cntrlVal && idx_decimal == -1)) {
                isValidChar = true;
            }
        } else {
            let exp = /^[0-9]$/;
            isValidChar = exp.test(input);
            if (isValidChar) {
                if (cntrlVal && (idx_decimal > 0) && ((cntrlVal.length - (idx_decimal + 1)) > 0)) {
                    let digits_aftr_decimal: string = cntrlVal.substring(idx_decimal);
                    if (digits_aftr_decimal.length > 2) {
                        isValidChar = false;
                    }
                }
            }
        }
        return isValidChar;
    }


    onBlur(element: any, index: number) {
        let number = this.transformToNumber(element.target.value);
        if (number > 0) {
            element.target.value = this.transformAmountToCurrency(number);
            this.kpiQuestion.alertValue = element.target.value;
            number = this.transformToNumber(element.target.value);
        }
    }
    onfocus(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
        }
    }

    onkeyup(element: any, index: number) {
        // this.checkValidStandard();
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
       // this.checkValidStandard();
        if (this.kpiQuestion.kpi.m3MeasureViewModel.measureUnit === this.adminLabels.KPI_UNIT_DAYS && element.target.value.trim() > 1000) {
            this.enteredStandardDayValid = true;
        } else {
            this.enteredStandardDayValid = false;
        }
    }

    percentageOnBlur(element: any) {
        if (element.target.value != '' && element.target.value.indexOf('%') < 0) {
            this.percentageOnkeyup(element);
            element.target.value = element.target.value + '%';
            this.kpiQuestion.alertValue = element.target.value;
        }
    }

    percentageOnkeyup(element: any) {
        if (element.target.value != '') {
            // this.checkValidStandard();
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
            this.percentageOnkeyup(element);
        }
    }

    checkValidStandard() {
        if (this.kpiQuestion.alertValue.toString().trim().length !== 0) {
            this.showDefaultStandardError = false;
        }
    }
    /*------ end region Public methods ------*/
}
