// Angular Imports
import { Component, OnInit, ViewEncapsulation, ViewChild, Input, EventEmitter, Output, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { Router } from '@angular/router';

// File Imports
import { AllUsersViewModel } from '../../admin/users/all-users/models/all-users-viewmodel.model';
import { AssignClientUserService } from '../assign-client-user/assign-client-user.service';
import { ClientDataService } from '../client-data.service';
import { ClientViewModel } from '../../shared/models/DepositLog/client.model';
import { SpecialitiesModel } from '../../admin/specialities/specialities.model';
import { BusinessUnitViewModel } from '../../admin/business-units/business-units.model';
import { SystemsModel } from '../../admin/systems/systems.model';
import { SpecialitiesService } from '../../admin/specialities/specialities.service';
import { BusinessUnitsService } from '../../admin/business-units/busienss-units.service';
import { SystemsService } from '../../admin/systems/systems.service';
import { GlobalEventsManager } from '../../shared/utilities/global-events-manager';
import { CLIENT_DATA_STEP, SHARED } from '../../shared/utilities/resources/labels';
import { ValidationMessageComponent } from '../../common/components/validation-message/validation-message.component';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { CheckListService } from '../../admin/services/checklist.service';
import { SitesService } from '../../admin/sites/sites.service';
import { ValueLabel } from '../../admin/models/checklist-item-table.model';

// Primeng Imports
import { Message } from 'primeng/components/common/api';

// Other Imports
import { NgOption } from '@ng-select/ng-select';
import { Modal } from 'ngx-modal';
import { Observable } from 'rxjs/Rx';

@Component({
    selector: 'client-data',
    templateUrl: './client-data.component.html',
    styleUrls: ['./client-data.component.css'],
    providers: [ClientDataService, SpecialitiesService, BusinessUnitsService, SystemsService, CurrencyPipe]
})
export class ClientDataComponent implements OnInit {

    /*-----region Input/Output bindings -----*/
    @Input('clientCode') clientCode: string;
    @Output('saveClientData') SaveClientData = new EventEmitter<any>();
    /*-----endregion Input/Output bindings -----*/

    /*------ region public properties ------*/

    public clientData: ClientViewModel;
    public shallowClientData: string;
    public specialityOptions: SpecialitiesModel[];
    public businessUnitOptions: BusinessUnitViewModel[];
    public systemOptions: SystemsModel[];
    public siteOptions: ValueLabel[] = [];
    public msgs: Message[] = [];
    public fileToUpload: File = null;
    public labels = CLIENT_DATA_STEP;
    public pdfFileData: any;
    public clientName = '';
    public showErrorMsg = false;
    public errorMsg: string;
    public canEdit = false;
    public editActions: any[] = [];
    public dropDownSpecialityError = false;
    public dropDownSystemError = false;
    public dropDownBusinessUnitError = false;
    public isPercentageofCashValid = true;
    public isFlatFeeValid = true;
    public percentileCashSelected = true;
    public currentDate = new Date();
    public validationConstants = VALIDATION_MESSAGES;
    public speciality: SpecialitiesModel;
    public systemOption: SystemsModel;
    public businessUnitOption: BusinessUnitViewModel;
    public users: any[];
    public sendAlertUsers: any[];
    public sendAlerts: any[];
    public checklistTypeMessage: string;
    public showPdf: boolean;
    public fileNameOrLabel = 'Choose pdf file';
    @ViewChild('pdfPopup') public pdfPopupModal: Modal;
    @ViewChild('clientDataFormSubmit') public clientDataFormSubmit: ElementRef;
    @ViewChild('clientDataForm') public clientDataForm: NgForm;


    selectedCityId: any;
    public weeklyCheckListOptions: ValueLabel[] = [];
    public monthlyCheckListOptions: ValueLabel[] = [];
    public showWeeklyChecklistInfo = false;
    public weeklyCheckListInfo: string;
    public monthlyCheckListInfo: string;
    public showMonthlyChecklistInfo = false;
    public weeklyChecklistEffectiveDate: string;
    public monthlyChecklistEffectiveDate: string;
    public showChecklistInformation = false;

    /*------ end region public properties ------*/

    /*------ region private properties ------*/

    private selectedLink = '%Cash';
    private triggerChecklistCount = 1;

    /*------ end region private properties ------*/

    constructor(private _clientDataService: ClientDataService, private _specialitiesService: SpecialitiesService, private _businessUnitsService: BusinessUnitsService,
        private _systemService: SystemsService,
        private _userService: UserService,
        private _clientUserService: AssignClientUserService,
        private _globalEventsManager: GlobalEventsManager,
        private _checkListService: CheckListService,
        private _sitesService: SitesService,
        private currencyPipe: CurrencyPipe,
        private router: Router
    ) {
        this.clientData = new ClientViewModel();
        this.clientData.completedClient = false;
        this.sendAlerts = [];


        this.getAllClientData();
    }

    /*------ region life cycle hooks ------*/

    public ngOnInit(): void {
        this.getScreenActions();
    }

    /*------ region life cycle hooks ------*/



    /*------ region Public methods ------*/

    // Get Edit Actions of Client data step page
    getScreenActions() {
        this._userService.getUserScreenActions(SCREEN_CODE.CLIENTDATA)
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

    getClientData() {
        if (this.clientCode !== null && this.clientCode !== '' && this.clientCode != undefined) {
            this._clientDataService.getClientData(this.clientCode).subscribe(
                data => {
                    if (data != null) {
                        this.clientData = data;
                        this.clientData.clientExist = true;
                        this.clientName = this.clientData.name;
                        if (data.percentageOfCash != null) {
                            this.clientData.percentageOfCash = this.clientData.percentageOfCash + '%';
                        }
                        if (data.flatFee != null && data.flatFee != '' && data.flatFee != '0') {
                            this.clientData.flatFee = this.transformAmountToCurrency(this.clientData.flatFee);
                        }
                        this.clientData.contractStartDate = new Date(data.contractStartDate);
                        this.clientData.contractEndDate = new Date(data.contractEndDate);
                        if (this.clientData.flatFee != '0' && this.clientData.flatFee != null) {
                            this.selectedLink = 'FlatFee';
                            this.percentileCashSelected = false;
                        } else {
                            this.selectedLink = '%Cash';
                            this.percentileCashSelected = true;
                        }
                        if (this.router.url == this.labels.CLIENT_DATA_URL) {
                            if (this.clientData.isActive == 'A') {
                                this.clientData.clientActive = true;
                                this._globalEventsManager.setClientDropdown(true);

                            } else {
                                this.clientData.clientActive = false;
                                this._globalEventsManager.setClientDropdown(false);
                            }
                        }
                        if (this.clientData.contractFilePath != null && this.clientData.contractFilePath != '') {
                            this.formValidFileName();
                        }
                        this.clientData.completedClient = (this.clientData.isActive == 'A' || this.clientData.isActive == 'I');
                        this.clientData.speciality = this.specialityOptions.find(x => x.specialityCode == data.speciality.specialityCode);
                        this.clientData.system = this.systemOptions.find(x => x.systemCode == data.system.systemCode);
                        this.clientData.businessUnit = this.businessUnitOptions.find(x => x.businessUnitCode == data.businessUnit.businessUnitCode);
                        this.clientData.site = this.siteOptions.find(x => x.value == data.site.key) != undefined ? data.site.key : undefined;
                        this.shallowClientData = JSON.stringify(this.clientData);
                        this.clientData.monthlyChecklist = undefined;
                        this.clientData.weeklyChecklist = undefined;
                    } else {
                        this.msgs = [];
                        this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                    }
                }
            );
        } else {
            this.clientData = new ClientViewModel();
            this.clientData.speciality = null;
            this.clientData.system = null;
            this.clientData.site = null;
            this.clientData.businessUnit = null;
            this.clientData.weeklyChecklist = null;
            this.clientData.monthlyChecklist = null;
        }
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

    saveClientDataFromParent() {
        this.clientDataFormSubmit.nativeElement.click();
    }

    /**
     * To Save the ClientData
     */
    saveClientData(clientDataForm) {
        if (clientDataForm.valid && this.isPercentageofCashValid && this.isFlatFeeValid) {
            if (this.clientData.contractStartDate >= this.clientData.contractEndDate) {
                this.errorMsg = this.labels.MESSAGES.START_END_DATES_NOT_VALID;
                this.showErrorMsg = true;
            } else if (!this.isNoticePeriodValid()) {
                this.errorMsg = this.labels.MESSAGES.NOTICE_PERIOD_NOT_VALID;
                this.showErrorMsg = true;
            } else {
                this.showErrorMsg = false;
                if (this.clientData.percentageOfCash != null) {
                    this.clientData.percentageOfCash = this.clientData.percentageOfCash.toString().slice(0, -1);
                }
                if (this.clientData.flatFee != null && this.clientData.flatFee != '') {
                    this.clientData.flatFee = this.transformToNumber(this.clientData.flatFee).toString();
                }
                if (this.clientData.completedClient) {
                    if (this.clientData.clientActive) {
                        this.clientData.isActive = 'A';
                    } else {
                        this.clientData.isActive = 'I';
                    }
                }
                this.clientData.clientCode = this.clientData.clientCode.trim();

                let shallowClientData: ClientViewModel = JSON.parse(JSON.stringify(this.clientData));

                if (this.clientData.clientExist) {
                    if (typeof shallowClientData.site === 'string') {
                        shallowClientData.site = { key: shallowClientData.site, value: '' };
                    } else if (typeof shallowClientData.site === 'number') {
                        shallowClientData.site = { key: shallowClientData.site, value: '' };
                    }
                } else {
                    if (typeof shallowClientData.site === 'number') {
                        shallowClientData.site = { key: shallowClientData.site, value: '' };
                    }
                }

                let form = this.createFormData(shallowClientData);
                form.append('contract', this.fileToUpload);
                this.SaveClientData.emit(form);
                if (this.clientData.percentageOfCash != null) {
                    this.clientData.percentageOfCash = this.clientData.percentageOfCash + '%';
                }
                if (this.clientData.flatFee != null) {
                    this.clientData.flatFee = this.transformAmountToCurrency(this.clientData.flatFee);
                }
                this.clientDataForm.form.markAsPristine();
            }
        }
    }

    // Validation for Notice period
    isNoticePeriodValid() {
        let isValid = true;
        if (this.clientData && this.clientData.contractStartDate <= this.clientData.contractEndDate) {
            let millisecPerDay = 1000 * 60 * 60 * 24;
            let millisecDifference = this.clientData.contractEndDate.getTime() - this.clientData.contractStartDate.getTime();
            let days = (millisecDifference / millisecPerDay) + 1;
            if (this.clientData.noticePeriod > days) {
                isValid = false;
            }
        }
        return isValid;
    }

    redirect() {
        this.showErrorMsg = false;
    }

    closeChecklistMessage() {
        this.showChecklistInformation = false;
    }

    formValidFileName() {
        this.fileNameOrLabel = '';
        let fileName = this.clientData.contractFileName.split('_');
        if (fileName.length > 1) {
            fileName = fileName.splice(1);
            for (let i = 0; i < fileName.length; i++) {
                if (fileName.length - 1 == i) {
                    this.fileNameOrLabel += fileName[i];
                } else {
                    this.fileNameOrLabel += fileName[i] + '_';
                }
            }
        } else {
            this.fileNameOrLabel = this.clientData.contractFileName;
        }
    }

    /**
     * To create formData from the object
     * @param object
     * @param form
     * @param namespace
     */
    createFormData(object: Object, form?: FormData, namespace?: string): FormData {
        const formData = form || new FormData();
        for (let property in object) {
            if (!object.hasOwnProperty(property) || !object[property]) {
                continue;
            }
            const formKey = namespace ? `${namespace}[${property}]` : property;
            if (object[property] instanceof Date) {
                formData.append(formKey, object[property].toISOString());
            } else if (typeof object[property] === 'object' && !(object[property] instanceof File)) {
                this.createFormData(object[property], formData, formKey);
            } else {
                formData.append(formKey, object[property]);
            }
        }
        return formData;
    }

    /**
     * ToGet spectiality options for client
     */
    getSpecialityOptions(data: SpecialitiesModel[]) {
        if (data != null) {
            this.specialityOptions = data.filter(c => c.recordStatus === 'A');
        }
    }

    /**
     * To get the system values
     */
    getSystemOptions(data: SystemsModel[]) {
        this.systemOptions = data.filter(c => c.recordStatus === 'A');
        if (this.clientDataForm.controls && this.clientDataForm.controls['System']) {
            this.clientDataForm.controls['System'].valueChanges.subscribe(t => {
                this.getBothChecklistItems();
            });
        }
    }

    /**
     * to get active site list
     * @param  [] data
     */
    private getSiteOptions(data) {
        if (data != null) {
            this.siteOptions = [];
            let site = data.filter(c => c.recordStatus === 'A');
            if (site && site.length) {
                this.siteOptions = site.map(f => {
                    return {
                        label: f.siteName,
                        value: f.siteId
                    };
                });
            }
            if (this.clientDataForm.controls && this.clientDataForm.controls['Site']) {
                this.clientDataForm.controls['Site'].valueChanges.subscribe(t => {
                    this.getBothChecklistItems();
                });
            }
        }
    }

    /**
     * to load weekly and monthly checklist based on systemid and siteid
     */
    private getBothChecklistItems() {

        if (this.clientDataForm.controls['System'].value && this.clientDataForm.controls['Site'].value) {
            const systemId = this.clientDataForm.controls['System'].value.id;
            const siteId = this.clientDataForm.controls['Site'].value;
            if (systemId && siteId && systemId > 0 && siteId > 0) {
                const weekly = this._checkListService.getWeeklyCheckList(systemId, siteId).catch(error => {
                    return Observable.throw(error);
                });

                const monthly = this._checkListService.getMonthlyCheckList(systemId, siteId).catch(error => {
                    return Observable.throw(error);
                });

                Observable
                    .forkJoin(weekly, monthly)
                    .subscribe(([week, month]) => {

                        if (this.triggerChecklistCount > 1) {
                            this.clientDataForm.controls['WeeklyChecklist'].setValue(undefined);
                            this.clientDataForm.controls['MonthlyChecklist'].setValue(undefined);
                        } else if (this.shallowClientData) {
                            let copy = JSON.parse(this.shallowClientData);
                            this.clientData.monthlyChecklist = copy.monthlyChecklist == 0 ? undefined : copy.monthlyChecklist;
                            this.clientData.weeklyChecklist = copy.weeklyChecklist == 0 ? undefined : copy.weeklyChecklist;
                        }

                        // Condition to check to show info when weekly checklist is updated.
                        if (new Date(this.clientData.weeklyChecklistEffectiveDate) > this.currentDate) {
                            this.showWeeklyChecklistInfo = true;
                            let date = new Date(this.clientData.weeklyChecklistEffectiveDate);
                            let month = date.getMonth() + 1;
                            let day = date.getDate();
                            this.weeklyChecklistEffectiveDate = ((month < 10) ? '0' + month : month) + '/' + ((day < 10) ? '0' + day : day) + '/' + date.getFullYear();
                            this.weeklyCheckListInfo = this.labels.CHECKLIST_UPDATE_MSG + ' ' + this.weeklyChecklistEffectiveDate;
                        }

                        // Condition to check to show info when monthly checklist is updated.
                        if (new Date(this.clientData.monthlyChecklistEffectiveDate) > this.currentDate) {
                            this.showMonthlyChecklistInfo = true;
                            let date = new Date(this.clientData.monthlyChecklistEffectiveDate);
                            let month = date.getMonth() + 1;
                            let day = date.getDate();
                            this.monthlyChecklistEffectiveDate = ((month < 10) ? '0' + month : month) + '/' + ((day < 10) ? '0' + day : day) + '/' + date.getFullYear();
                            this.monthlyCheckListInfo = this.labels.CHECKLIST_UPDATE_MSG + ' ' + this.monthlyChecklistEffectiveDate;
                        }

                        this.weeklyCheckListOptions = [];
                        if (week.checklists && week.checklists.length) {
                            this.weeklyCheckListOptions = week.checklists.map(t => {
                                return {
                                    value: t.key,
                                    label: t.value
                                };
                            });
                        }

                        this.monthlyCheckListOptions = [];
                        if (month.checklists && month.checklists.length) {
                            this.monthlyCheckListOptions = month.checklists.map(t => {
                                return {
                                    value: t.key,
                                    label: t.value
                                };
                            });
                        }

                        if (!this.monthlyCheckListOptions.length && !this.weeklyCheckListOptions.length) {
                            this.errorMsg = this.labels.MESSAGES.BOTH_WEEKLY_MONTHLY_EMPTY_CHECKLIST;
                            this.showErrorMsg = true;
                        } else if (!this.monthlyCheckListOptions.length) {
                            this.errorMsg = this.labels.MESSAGES.EMPTY_MONTHLY_CHECKLIST;
                            this.showErrorMsg = true;
                        } else if (!this.weeklyCheckListOptions.length) {
                            this.errorMsg = this.labels.MESSAGES.EMPTY_WEEKLY_CHECKLIST;
                            this.showErrorMsg = true;
                        }

                        this.triggerChecklistCount = this.triggerChecklistCount + 1;

                    }, er => {

                    });


            }
        }
    }

    /**
     * ToGet BusinessUnits for the client
     */
    getBusinessUnitOptions(data: BusinessUnitViewModel[]) {
            this.businessUnitOptions = data.filter(c => c.recordStatus === 'A');
    }

    onFileSelect(event) {
        if (event.target.files.length == 0) {
            if (this.clientData.contractFilePath != null && this.clientData.contractFilePath != '') {
                this.formValidFileName();
            }
            else {
                this.fileNameOrLabel = 'Choose pdf file';
            }
        } else {
            this.fileNameOrLabel = event.target.files[0].name;
            this.fileToUpload = event.target.files.item(0);
        }
    }

    setradio(e: string): void {

        this.selectedLink = e;
        if (e == '%Cash') {
            this.percentileCashSelected = true;
            this.clientData.flatFee = null;
        } else {
            this.percentileCashSelected = false;
            this.clientData.percentageOfCash = null;
        }

    }

    isSelected(name: string): boolean {
        if (name == '%Cash') {
            this.clientData.flatFee = null;
        } else {
            this.clientData.percentageOfCash = null;
        }
        if (!this.selectedLink) { // if no radio button is selected, always return false so every nothing is shown
            return false;
        }

        return (this.selectedLink === name); // if current radio button is selected, return true, else return false
    }

    public openDocument() {
        this._clientDataService.getDocument(this.clientData.contractFilePath).subscribe(
            (response) => {
                this.pdfFileData = response.name;
                this.showPdf = true;
                this.pdfPopupModal.open();
            });
    }

    getAllData() {
        return Observable.forkJoin(
            this._businessUnitsService.getBusinessUnits(true),
            this._systemService.getAllSystems(true),
            this._specialitiesService.getSpecialities(true),
            this._clientUserService.getAllM3Users(),
            this._sitesService.getSitesDropdown(true)
        );
    }

    getAllClientData() {
        this.getAllData().subscribe(
            data => {
                this.getBusinessUnitOptions(data[0]);
                this.getSystemOptions(data[1]);
                this.getSpecialityOptions(data[2]);
                this.getAllUsers(data[3]);
                this.getClientData();
                this.getSiteOptions(data[4]);
            }
        );
    }

    // Check for client code exists or not while creating a new client.
    isClientCodeExists(clientCode) {
        if (clientCode !== undefined && clientCode !== '') {
            this._clientDataService.checkForExistingClientCode(clientCode.trim()).subscribe((isExist) => {
                if (isExist) {
                    this.errorMsg = this.labels.MESSAGES.CLIENT_CODE_EXISTS;
                    this.showErrorMsg = true;
                }
            });
        }
    }

    /* Formatting the percentage of Cash Property  */
    transformToNumber(amount: any) {
        let number = parseFloat(amount.replace(/[^0-9-.]/g, ''));
        if (!isNaN(number)) {
            return number;
        } else {
            return 0;
        }
    }

    transformAmountToCurrency(amount: any) {
        let currency = this.currencyPipe.transform(amount, 'USD');
        return currency;
    }

    onfocus(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
            this.onkeyup(element);
        }
    }

    onBlur(element: any) {
        if (element.target.value != '' && element.target.value.indexOf('%') < 0) {
            this.onkeyup(element);
            element.target.value = element.target.value + '%';
            this.clientData.percentageOfCash = element.target.value;
        }
    }

    onkeyup(element: any) {
        if (element.target.value != '') {
            let regex = /^(?:100(?:\.00?)?|\d?\d(?:\.\d\d?)?)$/;
            if (regex.test(element.target.value)) {
                this.isPercentageofCashValid = true;
            } else {
                this.isPercentageofCashValid = false;
            }
        }
    }

    onBlurFlatFee(element: any) {
        let number = this.transformToNumber(element.target.value);
        if (number > 0) {
            element.target.value = this.transformAmountToCurrency(number);
            this.clientData.flatFee = element.target.value;
            number = this.transformToNumber(element.target.value);
            let regex = /^[+]?[0-9]{1,8}(?:\.[0-9]{1,2})?$/;
            if (!regex.test(number.toString())) {
                this.isFlatFeeValid = false;
            } else {
                this.isFlatFeeValid = true;
            }
        }
    }

    onkeyupFlatFee(element: any) {
        let regex = /^[+]?[0-9]{1,8}(?:\.[0-9]{1,2})?$/;
        if (regex.test(element.target.value)) {
            this.isFlatFeeValid = true;
        } else {
            this.isFlatFeeValid = false;
        }
    }

    onfocusFlatFee(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
        }
    }

    /**
     * To get the M3Users and shou in dropdown
     * @param users
     */
    getAllUsers(users: AllUsersViewModel[]) {
        if (users) {
            this.users = [];
            this.users = users.map(user => { return { label: user.firstName + ' ' + user.lastName, value: user.email, email: user.email } })
            users = users.filter(c => c.role != this.labels.USER_ROLE);
            this.sendAlertUsers = users.map(user => { return { label: user.firstName + ' ' + user.lastName, value: user.email, email: user.email } })
        }
    }

    /**
     * Show message when checklist is updated.
     * @param checkListType
     */
    onChangeChecklist(checkListType : string) {
        if (this.clientData.clientExist) {
            if (checkListType === 'W') {
                this.checklistTypeMessage = ' week'
            } else {
                this.checklistTypeMessage = ' month'
            }
            this.showChecklistInformation = true;
        }
    }

    /*------ end region Public methods ------*/
}
