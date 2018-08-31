// Angular imports
import { Component, OnInit, ViewChild, NgZone, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Form } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { FormControl } from '@angular/forms';
import 'core-js';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs';
import 'rxjs/add/observable/forkJoin';

// Common file imports
import { DepositLogService } from './deposit-log.service';
import { DepositLogClientData, ExportDepositViewModel } from './models/deposit-log-client-data.model';
import { PayerData } from './models/payer-data.model';
import { DEPOSIT_LOG, ERROR_MESSAGE, ADMIN_SHARED, SHARED } from '../shared/utilities/resources/labels';
import { DepositLogMTDViewModel } from './models/deposit-log-mtd.model';
import { DepositLogProjectionViewModel } from './models/deposit-log-projection.model';
import { DepositLogAttributeViewModel } from './models/deposit-log-attribute.model';
import { DepositLogClientDataAmountViewModel } from './models/deposit-log-client-data-amount.model';
import { DepositLogConstants } from './deposit-log-constants';
import { KeyValuePair } from '../shared/models/DepositLog/KeyValuepair';
import { ValidationMessageComponent } from '../common/components/validation-message/validation-message.component';
import { ValidationResponseViewModel } from '../common/models/validation.model';
import { Helper } from '../common/common.helper';
import { UserService } from '../shared/services/user.service';
import { SCREEN_CODE } from '../shared/utilities/resources/screencode';
import { MonthDepositViewModel } from './models/month-deposit.model';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { depositLogAnimation } from './deposi-log.animations';

// Third party imports

import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { } from 'linq';
import { NgxGaugeModule } from 'ngx-gauge';
import { ChartModule } from 'primeng/chart';
import { IMonth } from 'ng2-date-picker';
import { MomentModule } from 'angular2-moment';
import * as moment from 'moment';
import { DatePickerDirective } from 'ng2-date-picker';
import { DpDatePickerModule } from 'ng2-date-picker';
import { DatePickerComponent } from 'ng2-date-picker';
import { Message } from 'primeng/components/common/api';
import * as _ from 'lodash';

@Component({
    selector: 'app-deposit-log',
    templateUrl: './deposit-log.component.html',
    styleUrls: ['./deposit-log.component.css'],
    providers: [DepositLogService, UserService, CurrencyPipe],
    animations: [depositLogAnimation]
})

export class DepositLogComponent implements OnInit, OnDestroy {

    /*------------ region public properties -------------*/

    public depositLogConstants: DepositLogConstants;
    public theme = 'dp-material';
    public client: any;
    public clientCode: string;
    public clientName: string;
    public enteredDepositNotValid: boolean[] = [];
    public expectedTotal: any = 0;
    public expectedTotalCurrencyFormat: string;
    public enteredExpectedAmountNotValid = false;
    public showMonthDepositsOfAYear = false;
    public monthDepositsDataOfAYear: MonthDepositViewModel[];
    public Difference: any;
    public depositYear: string;
    public closeMonthSuccessMessage: string;
    public closeMonthTitle: string;
    public isAddDepositVisible: boolean = true;
    public submittedDepositDates: string[];
    public isTodayHoliday = false;
    public holidayDatesInString: string[];
    public isCurrentDateNotValid = false;

    public exportDepositLog: boolean = false;
    public selectedMonthArray: any[] = [];
    public exportdata: ExportDepositViewModel;
    public isEnable: boolean = true;
    public calendarYear: number;
    public calendarYears: any[];
    public months: any[];
    public monthsToSelect: any[];
    public selectedMonths: any[];

    /*------ combo graph properties ------*/

    view: any[];
    width = 900;
    height = 700;
    fitContainer = false;
    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = false;
    legendTitle = 'Legend';
    showXAxisLabel = true;
    tooltipDisabled = false;
    xAxisLabel = 'Number of Last Business Days';
    showYAxisLabel = true;
    yAxisLabel = 'Projected Cash';
    showGridLines = false;
    innerPadding = '10%';
    roundDomains = false;
    maxRadius = 20;
    minRadius = 3;
    showSeriesOnHover = true;
    roundEdges = true;
    animations = true;
    xScaleMin: any;
    xScaleMax: any;
    yScaleMin: number;
    yScaleMaxValue = 0;
    barChart: any[] = [];
    lineChartSeries: any[] = [
        {
            name: 'Target Cash',
            series: []
        }
    ];
    lineChartScheme = {
        name: 'coolthree',
        selectable: true,
        group: 'linear',
        domain: [
            '#4CAF50', '#01579b'
        ]
    };
    comboBarScheme = {
        name: 'singleLightBlue',
        selectable: true,
        group: 'linear',
        domain: [
            '#01579b'
        ]
    };
    showRightYAxisLabel = false;
    yAxisLabelRight = '';

    /*------ end combo graph properties ------*/

    /*------- bar chart properties ---------*/

    barChartProperties = {
        view: [],
        showXAxis: true,
        showYAxis: true,
        gradient: false,
        showLegend: false,
        showXAxisLabel: true,
        xAxisLabel: 'Payers',
        showYAxisLabel: true,
        yAxisLabel: 'Deposit Amount',
        schemeType: 'linear',
        data: [],
        colorScheme: {
            domain: ['#01579b']
        },
        barPadding: 25,
    };

    /*------- end bar chart properties ---------*/


    /*----- message related properties ----------*/

    public displayProjectionDaysMessage = false;
    public displayProjectionWeeksMessage = false;
    public showProjectedCashVsDays = true;
    public showNodataMessage = false;
    public showNodataMessageWeekly = false;
    public showDepositLogEntry: boolean;
    public messages: Message[] = [];
    public saveMessages: Message[] = [];
    public display = false;

    /*----- end message related properties ----------*/


    /*------ date related properties -------*/

    public selectedMonthYear: string;
    public oldSelectedMonthYear: string;
    public selectedMonth: number;
    public selectedYear: number;
    public currentDate = new Date();
    public depositDate: Date;
    public today: Date;

    /*---------- end date related properties ----------*/

    public isSliderDisabled = false;
    public previousDays: number;
    public tempNumberOfWeeks: number;
    public actualLastWorkingDays: number;
    public actualLastNumberOfWeeks: number;
    public lastWorkingDays = 30;
    public maxWorkingDays: number;
    public minWorkingDays = 1;
    public minRangeBusinessDays = 1;
    public numberOfBars = 10;
    public minRange = 21;
    public lastWorkingDaysProjectedCash: any[];
    public lastWorkingDaysPayments: any[];
    public tot_business_days: number;
    public curr_business_days: number;


    /*------ error messages properties ----------*/

    public errorMessageMTD = false;
    public errorMessageMonthly = false;
    public errorMessageProgressBar = false;
    public errorMessageDepositLog = false;
    public errorMessageTopPayers = false;
    public errorMessageCashProjectionPreviousDays = false;

    /*------ end error messages properties ----------*/


    public depositLogMTD: DepositLogMTDViewModel;
    public depositLogProjection: DepositLogProjectionViewModel;
    public depositLogClientDataAmount: DepositLogClientDataAmountViewModel;
    public depositLogMTDMaxValue: any;
    public depositLogMTDValue: any;
    public MTDPercentMet: number;
    public MTDDeficit: number;
    public depositeLogClientData: DepositLogClientData;
    public depositLogAttribute: DepositLogAttributeViewModel;
    public payerChartData: Array<PayerData>;
    public dataset: any;
    public depositLogDataAmount: any[];
    public depositLogPayers: string[];
    public MTDdata: any;
    public showDepositLogGraph = true;
    public showDepositLogGrid = false;
    public projectedCashData: any;
    public projectedCashAmount: any;
    public projectedCashAmountMaxValue: any;
    public projectedCashAmountPercentMet: number;
    public projectedToCollectAmount: any;
    public labels = DEPOSIT_LOG;
    public errorLabels = ERROR_MESSAGE;
    public sharedLabels = SHARED;
    public savedLastNumberOfWeeks: string = null;
    public savedLastNumberOfBusienssDays: string = null;
    public numberOfLastWorkingDays: number;
    public trackNumberOfLastWorkingDays: number = this.numberOfLastWorkingDays;
    public numberOfWeeks = 3;
    public trackNumberOfWeeks: number = this.numberOfWeeks;
    public lastNumberOfDepositDays: number;
    public lastNumberOfDepositWeeks: number;
    public lastRow = {};
    public numberOfRows: number;
    public onMonthChangeCalled = false;
    public showSimpleBusinessDays = true;
    public validationResponse: ValidationResponseViewModel;
    public datePickerConfig = {
        max: '',
        enableMonthSelector: true,
        isMonthDisabledCallback: false,
    };


    /* ----------  Action properties based on role ------- */

    public canEdit = false;
    public editActions: any[] = [];
    public holidayDates: Array<Date>;

    /* ---------- end action properties ------------*/

    /*------------end region public properties -------------*/

    /*------ region private properties ------*/
    private clientCodeSubscriber: Subscription;
    /*------ end region private properties ------*/

    /*---------- view child properties --------------*/

    @ViewChild('depositLog') depositLog: FormControl;
    @ViewChild('dateDirectivePicker') datePickerDirective: DatePickerDirective;
    @ViewChild('dayPicker') datePicker: DatePickerComponent;

    /*---------- end view child properties --------------*/

    /*------ region Constructor ------*/

    constructor(private depositLogService: DepositLogService,
        private _globalEventsManager: GlobalEventsManager,
        private _service: DepositLogService,
        private _ngZone: NgZone,
        private ref: ChangeDetectorRef,
        private userService: UserService,
        private currencyPipe: CurrencyPipe) {  
        this._globalEventsManager.showCloseMonth.subscribe(
            (resp) => {
                this.showMonthDepositsOfAYear = resp;
            }); 
    }

    /*------ end region Constructor ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        let presentDate = new Date();
        this.messages = [];
        this.depositLogConstants = new DepositLogConstants();
        let month = this.getMonthShortName(this.currentDate);
        let year = this.currentDate.getFullYear();
        this.datePickerConfig.max = month + ',' + year;
        this.selectedYear = year;
        this.depositYear = year.toString();
        this.selectedMonthYear = month + ', ' + this.selectedYear;
        this.setMonthAndYear();
        this.getHolidays();
        this.depositeLogClientData = new DepositLogClientData();
        this.depositDate = presentDate;
        this.today = presentDate;
        this.holidayDates = new Array<Date>();
        this._globalEventsManager.setClientDropdown(true);
        this.clientCodeSubscriber = this._globalEventsManager.getGlobalClientCode.subscribe((globalClientCode) => {
            this.showDepositLogEntry = false;
            this.client = globalClientCode;
            if (this.client != null) {
                this.submittedDepositDates = [];
                this.oldSelectedMonthYear = '';
                this.clientCode = this.client.value;
                this.clientName = this.client.label;
                this.savedLastNumberOfWeeks = null;
                this.savedLastNumberOfBusienssDays = null;
                this.getDatesOfEnteredDepositsForClient(this.clientCode);
                this.getOnPageLoadScreenAction();
            }
        });                   
    }   

    ngOnDestroy() {
        if (this.clientCodeSubscriber) {
            this.clientCodeSubscriber.unsubscribe();            
        }      
        this.oldSelectedMonthYear = '';
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------ end region life cycle hooks ------*/

    /*------ region event called methods ------*/

    canDeactivate(): Observable<boolean> | boolean {       
        this._globalEventsManager.showCloseMonthPage(false);
        return true;
    }	

    close() {
        this.datePickerDirective.api.close();
    }

    open() {
        this.datePickerDirective.api.open();
    }

    OnMonthChange() {
        if (this.clientCode != undefined && this.clientCode != null && this.oldSelectedMonthYear != this.selectedMonthYear) {
            this.setMonthAndYear();
            this.oldSelectedMonthYear = this.selectedMonthYear;
            this.getDaysForProgress();
            this.getDepositLogMTD(this.clientCode, this.selectedMonth, this.selectedYear);
            this.getDepositLog(this.clientCode, this.selectedMonth, this.selectedYear);
            this.getBarChart(this.clientCode, this.selectedMonth, this.selectedYear);
            this.onMonthChangeCalled = true;
            this.getNumberOfDepositDaysOfClient(this.clientCode, this.selectedMonth, this.selectedYear);
            this.getLastSavedAttributeData();
            this.showProjectedCashVsBusinessDays();
            if (this.showMonthDepositsOfAYear) {
                this.getDepositLogInAYearForClient(false);
            }
            if (this.editActions.includes('ViewAddDepositSection') && !(this.depositDate.getMonth() + 1 === this.selectedMonth && this.depositDate.getFullYear() === this.selectedYear)) {
                this.setDepositLogDateAndGetDepositData();
            }
        }
    }

    OnDateChange(event) {
        this.messages = [];
        if (event) {
            this.depositLogService.getDepositLogForClientDate(this.clientCode, this.depositDate.toLocaleDateString()).subscribe(
                data => {
                    if (data) {
                        this.depositeLogClientData = data;
                        this.isCurrentDateNotValid = (this.isTodayHoliday && this.IsTodayADepositDate());
                        this.enteredDepositNotValid.length = this.depositeLogClientData.payers.length;
                        this.enteredDepositNotValid.fill(false);
                        this.expectedTotal = undefined;
                        this.expectedTotalCurrencyFormat = undefined;
                        this.Difference = undefined;
                        this.formatePayerAnnualAmount();
                        this.selectedMonthYear = this.getMonthShortName(this.depositDate) + ', ' + this.depositDate.getFullYear();
                        if (this.showMonthDepositsOfAYear && Number(this.depositYear) !== this.depositDate.getFullYear()) {
                            this.depositYear = this.depositDate.getFullYear().toString();
                            this.getDepositLogInAYearForClient(false);
                        }
                    }
                }
            );
        }
    }

    /*------end region event called methods ------*/


    reload() {
        this.getDaysForProgress();
        this.getNumberOfDepositDaysOfClient(this.clientCode, this.selectedMonth, this.selectedYear);
        this.getBarChart(this.clientCode, this.selectedMonth, this.selectedYear);
        this.getDepositLogMTD(this.clientCode, this.selectedMonth, this.selectedYear);
        this.getDepositLog(this.clientCode, this.selectedMonth, this.selectedYear);
        this.getLastSavedAttributeData();
    }

    setMonthAndYear() {
        let date = new Date('1 ' + this.selectedMonthYear);
        this.selectedMonth = date.getMonth() + 1;
        this.selectedYear = date.getFullYear();
    }

    /*--------- region service calls -------------*/

    /**
      * Gets the Payers for the client
      * @param clientcode
      */
    getPayersForClient(clientcode: string) {
        this.depositLogService.getPayersForClient(clientcode).subscribe(
            data => {
                if (this.depositDate) {
                    this.OnDateChange(this.depositDate);
                } else {
                    this.depositeLogClientData = data;
                    this.enteredDepositNotValid.length = this.depositeLogClientData.payers.length;
                    this.enteredDepositNotValid.fill(false);
                    this.formatePayerAnnualAmount();
                }

            },
            err => { }
        );
    }

    /**
     * Gets Business Days of given month and year
     * @param year
     * @param month
     */
    private getTotBusinessDays(year, month): number {
        let days = -1;
        this.depositLogService.getTotalBusinessDaysofAMonthInYear(year, month).subscribe(
            data => {
                if (data != null) {
                    this.errorMessageProgressBar = false;
                    this.tot_business_days = data;
                    return days;
                } else {
                    this.errorMessageProgressBar = true;
                }
            },
            err => { }
        );
        return days;
    }

    /**
     * To get the number of deposit days of a client for a given month.
     * @param year
     * @param month
     */
    private getNumberOfDepositDaysOfClientForGivenMonth(year: number, month: number): number {
        let days = -1;
        this.depositLogService.getNumberOfDepositDaysOfClientForGivenMonth(this.clientCode, year, month).subscribe(
            data => {
                if (data != null) {
                    this.curr_business_days = data;
                    return days;
                } else {
                    this.saveMessages = [];
                    this.saveMessages.push({ severity: 'error', summary: '', detail: this.sharedLabels.ERROR_GET_DETAILS });
                }
            },
            err => { }
        );
        return days;
    }

    /**
     * Get the deposit log Month To Date Amount.
     * @param clientcode
     * @param month
     * @param year
     */
    getDepositLogMTD(clientcode: string, month: Number, year: Number) {
        this.depositLogMTD = new DepositLogMTDViewModel();
        this.depositLogService.getDepositLogMTDData(this.clientCode, month, year).subscribe(
            data => {
                this.depositLogMTD = data;
                if (this.depositLogMTD != null) {
                    this.errorMessageMTD = false;
                    if (Math.round(this.depositLogMTD.coveredMTDValueTillDate) < Math.round(this.depositLogMTD.maxMTDValueTillDate)) {
                        setTimeout(() =>
                            this.MTDdata = {
                                labels: [this.labels.MTD_PROGRESS, this.labels.MTD_DEFICIT],
                                datasets: [
                                    {
                                        data: [Math.round(this.depositLogMTD.coveredMTDValueTillDate), Math.round(this.depositLogMTD.maxMTDValueTillDate) - Math.round(this.depositLogMTD.coveredMTDValueTillDate)],
                                        backgroundColor: [
                                            this.depositLogConstants.projectedCashColor,
                                            this.depositLogConstants.maxProjectedCashColor
                                        ],
                                        hoverBackgroundColor: [
                                            this.depositLogConstants.projectedCashColor,
                                            this.depositLogConstants.maxProjectedCashColor
                                        ]
                                    }]
                            }, 100),
                            this.MTDDeficit = Math.round(this.depositLogMTD.maxMTDValueTillDate) - Math.round(this.depositLogMTD.coveredMTDValueTillDate);

                    } else {
                        setTimeout(() =>
                            this.MTDdata = {
                                labels: [this.labels.MTD_PROGRESS],
                                datasets: [
                                    {
                                        data: [Math.round(this.depositLogMTD.coveredMTDValueTillDate)],
                                        backgroundColor: [
                                            this.depositLogConstants.projectedCashColor,
                                        ],
                                        hoverBackgroundColor: [
                                            this.depositLogConstants.projectedCashColor,
                                        ]
                                    }]
                            }, 100);
                        this.MTDDeficit = 0;
                    }
                    this.depositLogMTDValue = Math.round(this.depositLogMTD.coveredMTDValueTillDate);
                    this.depositLogMTDMaxValue = Math.round(this.depositLogMTD.maxMTDValueTillDate);
                    if (this.depositLogMTD.maxMTDValueTillDate > 0) {
                        this.MTDPercentMet = Math.round(this.depositLogMTD.coveredMTDValueTillDate * 100 / this.depositLogMTD.maxMTDValueTillDate);
                    } else {
                        this.MTDPercentMet = 0;
                    }
                } else {
                    this.errorMessageMTD = true;
                }
            },
            err => { }
        );
    }

    /**
    * Gets the deposit Log Data
    * @param clientCode
    * @param month
    * @param year
    */
    getDepositLog(clientCode: string, month: Number, year: Number) {
        this.depositLogClientDataAmount = new DepositLogClientDataAmountViewModel();
        this.depositLogService.getDepositLog(this.clientCode, month, year).subscribe(
            data => {
                if (data != null) {
                    this.lastRow = {};
                    this.depositLogClientDataAmount = data;
                    this.depositLogPayers = new Array();
                    this.depositLogDataAmount = new Array();
                    if (this.depositLogClientDataAmount.depositLogData.length > 1) {
                        this.errorMessageDepositLog = false;
                        this.depositLogDataAmount = this.depositLogClientDataAmount.depositLogData;
                        this.numberOfRows = this.depositLogDataAmount.length - 1;
                        this.lastRow = this.depositLogDataAmount[this.depositLogDataAmount.length - 1];
                        this.depositLogDataAmount.pop();
                        this.depositLogPayers = this.depositLogClientDataAmount.depositLogPayers;
                    } else {
                        this.errorMessageDepositLog = true;
                        this.depositLogPayers = this.depositLogClientDataAmount.depositLogPayers;
                    }
                } else {
                    this.saveMessages = [];
                    this.saveMessages.push({ severity: 'error', summary: '', detail: this.sharedLabels.ERROR_GET_DETAILS });

                }
            },
            err => { }
        );
    }

    /**
     * Add the deposit log entry.
     * @param depositLog
     */
    saveDepositLog(depositLog) {
        if (this.expectedTotal != 0 && this.enteredExpectedAmountNotValid) {
            this.messages = [];
            this.messages.push({ severity: 'error', summary: 'Deposit Log', detail: 'Expected Total is not valid' });
            return;
        }
        if (!this.enteredDepositNotValid.every(x => x === false)) {
            this.messages = [];
            this.messages.push({ severity: 'error', summary: 'Deposit Log', detail: 'Cannot add deposits' });
            return;
        }
        depositLog.value.date.setHours(0, 0, 0, 0);
        if (depositLog.value.date <= this.currentDate) {
            if (depositLog.valid) {
                this.calulateTotal();
                let that = this;
                this.depositeLogClientData.payers.forEach(function (element) {
                    element.amount = that.transformToNumber(element.amountCurrencyFormat) || 0;
                });
                let sum = this.getDepositSum();
                if (this.expectedTotal !== 0 && this.expectedTotal != undefined && this.expectedTotal != sum) {
                    this.messages = [];
                    this.messages.push({ severity: 'error', summary: 'Deposit Log', detail: 'Sum of deposits should be equal to Expected Total' });
                    return;
                }
                this.depositeLogClientData.savedLastNumberOfBusinessDays = parseInt(this.savedLastNumberOfBusienssDays);
                this.depositLogService.saveDepositLogData(this.depositeLogClientData).subscribe(
                    (resp) => {
                        if (resp) {
                            this.validationResponse = resp;
                            if (this.validationResponse.success) {
                                this.messages = [];
                                this.messages.push({ severity: 'success', summary: 'Deposit Log', detail: 'saved successfully' });
                                this.getPayersForClient(this.clientCode);
                                this.addSelectedDepositDateToSubmittedDepositDates(this.depositeLogClientData.date);
                                this.reload();
                            } else {
                                this.messages = [];
                                this.messages.push({ severity: 'error', summary: 'Deposit Log', detail: 'Error occured while saving data' });
                            }
                            if (this.showMonthDepositsOfAYear) {
                                this.depositYear = depositLog.value.date.getFullYear();
                                this.getDepositLogInAYearForClient(false, false);
                            }
                        }
                    },
                    error => { }
                );
            }
        } else {
            this.messages = [];
            this.messages.push({ severity: 'error', summary: 'Deposit Log', detail: 'Cannot add deposits for future dates.' });
        }
    }

    /**
     * Gets the simple business days projected cash.
     * @param clientCode
     * @param numberOfLastWorkingDays
     * @param month
     * @param year
     */
    getSimpleBusinessProjectedCash(clientCode: string, numberOfLastWorkingDays: number, month: number, year: number, savedLastNumberOfBusienssDays : number) {
        this.depositLogProjection = new DepositLogProjectionViewModel();
        this.depositLogService.getProjectedCashFromSimpleBusinessDays(this.clientCode, numberOfLastWorkingDays, month, year, savedLastNumberOfBusienssDays).subscribe(
            data => {
                this.depositLogProjection = data;
                if (this.depositLogProjection != null) {
                    this.showNodataMessage = false;
                    this.errorMessageMonthly = false;
                    if (this.numberOfLastWorkingDays !== this.depositLogProjection.numberOfLastWorkingDaysOrWeeks) {
                        this.setLastWorkingDays();
                    }
                    this.onMonthChangeCalled = false;
                    if (Math.round(this.depositLogProjection.depositLogAmount) < Math.round(this.depositLogProjection.payments)) {
                        this.projectedCashData = {
                            labels: [this.labels.PROJECTED_CASH, this.labels.MONTHLY_DEFICIT],
                            datasets: [
                                {
                                    data: [Math.round(this.depositLogProjection.depositLogAmount), Math.round(this.depositLogProjection.payments) - Math.round(this.depositLogProjection.depositLogAmount)],
                                    backgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                        this.depositLogConstants.maxProjectedCashColor
                                    ],
                                    hoverBackgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                        this.depositLogConstants.maxProjectedCashColor
                                    ]
                                }]
                        };
                    } else {
                        this.projectedCashData = {
                            labels: [this.labels.PROJECTED_CASH],
                            datasets: [
                                {
                                    data: [Math.round(this.depositLogProjection.depositLogAmount)],
                                    backgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                    ],
                                    hoverBackgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                    ]
                                }]
                        };
                    }
                    this.projectedCashAmount = Math.round(this.depositLogProjection.depositLogAmount);
                    this.calculateProjectedToCollect();
                    this.projectedCashAmountMaxValue = Math.round(this.depositLogProjection.payments);
                    this.projectedCashAmountPercentMet = Math.round(this.depositLogProjection.depositLogAmount * 100 / this.depositLogProjection.payments);
                } else {
                    this.errorMessageMonthly = true;
                }
            },
            err => { }
        );
    }

    /**
     * To get the projected Cash Of previous Weeks when previous number of weeks are given.
     * @param clientCode
     * @param month
     * @param year
     * @param numberOfWeeks
     */

    getProjectedCashOfPreviousWeeks(clientCode: string, month: number, year: number, numberOfWeeks: number) {
        this.depositLogProjection = new DepositLogProjectionViewModel();
        this.depositLogService.GetProjectedCashOfPreviousWeeks(this.clientCode, month, year, numberOfWeeks).subscribe(
            data => {
                this.depositLogProjection = data;
                if (this.depositLogProjection.depositLogAmount != null && this.depositLogProjection.payments != null) {
                    this.showNodataMessage = false;
                    this.errorMessageMonthly = false;
                    if (this.numberOfWeeks !== this.depositLogProjection.numberOfLastWorkingDaysOrWeeks) {
                        this.setLastWeekDays();
                    }
                    this.onMonthChangeCalled = false;
                    if (Math.round(this.depositLogProjection.depositLogAmount) < Math.round(this.depositLogProjection.payments)) {
                        this.projectedCashData = {
                            labels: [this.labels.PROJECTED_CASH, this.labels.MONTHLY_DEFICIT],
                            datasets: [
                                {
                                    data: [Math.round(this.depositLogProjection.depositLogAmount), Math.round(this.depositLogProjection.payments) - Math.round(this.depositLogProjection.depositLogAmount)],
                                    backgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                        this.depositLogConstants.maxProjectedCashColor
                                    ],
                                    hoverBackgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                        this.depositLogConstants.maxProjectedCashColor
                                    ]
                                }]
                        };
                    } else {
                        this.projectedCashData = {
                            labels: [this.labels.PROJECTED_CASH],
                            datasets: [
                                {
                                    data: [Math.round(this.depositLogProjection.depositLogAmount)],
                                    backgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                    ],
                                    hoverBackgroundColor: [
                                        this.depositLogConstants.projectedCashColor,
                                    ]
                                }]
                        };
                    }
                    this.projectedCashAmount = Math.round(this.depositLogProjection.depositLogAmount);
                    this.calculateProjectedToCollect();
                    this.projectedCashAmountMaxValue = Math.round(this.depositLogProjection.payments);
                    this.projectedCashAmountPercentMet = Math.round(this.depositLogProjection.depositLogAmount * 100 / this.depositLogProjection.payments);
                } else {
                    this.errorMessageMonthly = true;
                }
            },
            err => { }
        );
    }

    /**
     * Gets the top 10 payers of a client.
     * @param clientCode
     * @param month
     * @param year
     */
    getBarChart(clientCode: string, month: Number, year: Number) {
        this.payerChartData = new Array<PayerData>();
        this.depositLogService.getPayersChartData(this.clientCode, month, year).subscribe(
            data => {
                if (data.success === true) {
                    if (data.listOfPayerDataViewModel.length > 0) {
                        this.errorMessageTopPayers = false;
                        let tempData: any[];
                        tempData = new Array();
                        setTimeout(() => {
                            for (let i = 0; i < data.listOfPayerDataViewModel.length; i++) {
                                let keyValue = new KeyValuePair();
                                keyValue.name = data.listOfPayerDataViewModel[i].payerName;
                                keyValue.value = data.listOfPayerDataViewModel[i].amount;
                                tempData.push(keyValue);
                            }
                            this.barChartProperties.data = tempData;
                        }, 0);
                    } else {
                        this.errorMessageTopPayers = true;
                    }
                } else {
                    this.messages = [];
                    this.messages.push({ severity: ADMIN_SHARED.ERROR, summary: ADMIN_SHARED.ERROR, detail: data.errorMessages[0] });
                }
            },
            err => { }
        );
    }

    /**
    * Get Projected Cash For Last Working Days
    * @param clientCode
    * @param month
    * @param year
    * @param numberOfLastWorkingDays
    */
    getProjectedCashOfLastWorkingDays(clientCode: string, month: number, year: number, numberOfLastWorkingDays: number) {
        this.depositLogService.getProjectedCashOfLastWorkingDays(this.clientCode, month, year, numberOfLastWorkingDays).subscribe(
            data => {
                if (data != null) {
                    this.errorMessageCashProjectionPreviousDays = false;
                    this.lastWorkingDaysProjectedCash = data.depositLogProjectedCashViewModelList;
                    this.lastWorkingDaysPayments = data.payments;
                    this.lastWorkingDays = data.depositLogProjectedCashViewModelList.length;
                    if (data.depositLogProjectedCashViewModelList.length <= 10 && this.maxWorkingDays <= 10) {
                        this.isSliderDisabled = true;
                    } else {
                        this.isSliderDisabled = false;
                    }
                    this.rearrangeProjectedCashData();
                } else {
                    this.errorMessageCashProjectionPreviousDays = true;
                }
            },
            err => { }
        );
    }

    getNumberOfDepositDaysOfClient(clientCode: string, month: number, year: number) {
        this.depositLogService.getNumberOfDepositDaysOfClient(clientCode, month, year).subscribe(
            data => {
                this.lastNumberOfDepositDays = data[0];
                this.lastNumberOfDepositWeeks = data[1];
                if (data[0] == 0) {
                    this.errorMessageCashProjectionPreviousDays = true;
                } else if (data[0] <= 10) {
                    this.lastWorkingDays = data[0];
                    this.minWorkingDays = 1;
                    this.maxWorkingDays = data[0];
                    this.getProjectedCashOfLastWorkingDays(this.clientCode, this.currentDate.getMonth() + 1, this.currentDate.getFullYear(), this.lastWorkingDays);
                } else if (data[0] > 60) {
                    this.maxWorkingDays = 60;
                    this.minWorkingDays = 10;
                    this.lastWorkingDays = 30;
                    this.getProjectedCashOfLastWorkingDays(this.clientCode, this.currentDate.getMonth() + 1, this.currentDate.getFullYear(), this.lastWorkingDays);
                } else {
                    this.maxWorkingDays = data[0];
                    this.minWorkingDays = 10;
                    this.lastWorkingDays = Math.round(data[0] / 2);
                    this.getProjectedCashOfLastWorkingDays(this.clientCode, this.currentDate.getMonth() + 1, this.currentDate.getFullYear(), this.lastWorkingDays);
                }
            },
            err => { }
        );
    }

    /**
     * Close or Reopen month
     * @param monthId
     * @param monthStatus
     */
    openOrCloseMonth(monthId, monthStatus) {
        let isCloseMonth = monthStatus === this.labels.CLOSED_STATUS ? false : true;
        this.depositLogService.openOrCloseMonthOfAYear(this.clientCode, monthId, this.depositYear, isCloseMonth).subscribe(
            (resp) => {
                this.validationResponse = resp;
                if (this.validationResponse.success) {
                    this.closeMonthSuccessMessage = this.validationResponse.infoMessages[0];
                    let depositLogDate = new Date(this.depositeLogClientData.date);
                    if (isCloseMonth) {
                        this.getDatesOfEnteredDepositsForClient(this.clientCode);
                    }
                    if (depositLogDate && (depositLogDate.getMonth() + 1 === monthId) && (depositLogDate.getFullYear() === Number(this.depositYear))) {
                        this.depositeLogClientData.isMonthClosed = isCloseMonth;
                    }
                    this.getDepositLogInAYearForClient(false, true);
                } else {
                    this.saveMessages = [];
                    this.saveMessages.push({ severity: 'error', summary: 'Selected Month', detail: this.validationResponse.errorMessages[0] });
                }
            }
        );
    }

    getScreenActions() {
        return Observable.forkJoin(
            this.userService.getUserScreenActions(SCREEN_CODE.DEPOSITLOG)
        );
    }

    getOnPageLoadScreenAction() {
        this.getScreenActions().subscribe(
            data => {
                if (data[0].length === 0) {
                    this.OnMonthChange();
                    this.canEdit = false;
                } else {
                    this.canEdit = true;
                    this.editActions = data[0];
                    this.OnMonthChange();
                    if (this.editActions.includes('ViewAddDepositSection')) {
                        this.getPayersForClient(this.clientCode);
                    }
                }
            }
        );
    }

    /**
    * To get the meridian holiday list.
    */
    getHolidays() {
        this.depositLogService.getHolidays().subscribe(
            data => {
                if (data && data.length > 0) {
                    this.holidayDates = data.map(x => new Date(x));
                    this.holidayDatesInString = this.holidayDates.map(date => new Date(date).toLocaleDateString());
                    this.isTodayHoliday = this.isCurrentDayAHoliday(this.holidayDatesInString);
                }
            }
        );
    }

    /**
     * Get All Dates Of Entered Deposits For Client
     * @param clientCode
     */
    getDatesOfEnteredDepositsForClient(clientCode) {
        this.depositLogService.getEnteredDepositDatesForClient(clientCode).subscribe(
            dates => {
                if (dates && dates.length > 0) {
                    this.submittedDepositDates = [];
                    let self = this;
                    dates.forEach(function (element) {
                        self.submittedDepositDates.push(new Date(element).toLocaleDateString());
                    });
                }
            }
        );
    }

    /**
     * Get all the months' deposits in a year for a client
     * @param isInitYearPicker
     * @param showMessage
     */
    getDepositLogInAYearForClient(isInitYearPicker, showMessage = false) {
        this.monthDepositsDataOfAYear = [];
        this.depositLogService.getClientDepositLogForAYear(this.clientCode, this.depositYear).subscribe(
            (data) => {
                if (data) {
                    this.monthDepositsDataOfAYear = data;
                    this.constructMonthDepositsInYearData();
                    this.showMonthDepositsOfAYear = true;
                    this.showDepositLogGraph = false;
                    this.showDepositLogGrid = false;
                    this.ref.detectChanges();
                    if (isInitYearPicker) {
                        this.initializeYearpicker();
                    }
                    if (showMessage) {
                        this.saveMessages = [];
                        this.saveMessages.push({ severity: 'success', summary: 'Selected Month', detail: this.closeMonthSuccessMessage });
                    }
                }
            }
        );
    }


    /*------ end region service calls ------*/


    /*-------------- region public methods -----------------*/


    /**
     * To set the actualLastWorkingDays , trackNumberOfLastWorkingDays and
     */
    setLastWorkingDays() {
        if (this.numberOfLastWorkingDays > this.lastNumberOfDepositDays) {
            this.displayProjectionDaysMessage = true;
        }
        if (this.onMonthChangeCalled) {
            if (parseInt(this.savedLastNumberOfBusienssDays) > this.depositLogProjection.numberOfLastWorkingDaysOrWeeks) {
                this.displayProjectionDaysMessage = true;
            }
        }
        this.actualLastWorkingDays = this.depositLogProjection.numberOfLastWorkingDaysOrWeeks;
        if (this.actualLastWorkingDays == 0) {
            this.showNodataMessage = true;
            this.displayProjectionDaysMessage = true;
        }
        this.previousDays = this.numberOfLastWorkingDays;
        this.numberOfLastWorkingDays = this.depositLogProjection.numberOfLastWorkingDaysOrWeeks;
        this.trackNumberOfLastWorkingDays = this.numberOfLastWorkingDays;
        this.onMonthChangeCalled = false;
    }


    setLastWeekDays() {
        if (this.numberOfWeeks > this.lastNumberOfDepositWeeks) {
            this.displayProjectionWeeksMessage = true;
        }
        if (this.onMonthChangeCalled) {
            if (parseInt(this.savedLastNumberOfWeeks) > this.depositLogProjection.numberOfLastWorkingDaysOrWeeks) {
                this.displayProjectionWeeksMessage = true;
            }
        }
        if (this.depositLogProjection.numberOfLastWorkingDaysOrWeeks == 0) {
            this.displayProjectionWeeksMessage = true;
            this.showNodataMessageWeekly = true;
        }
        this.actualLastNumberOfWeeks = this.depositLogProjection.numberOfLastWorkingDaysOrWeeks;
        this.tempNumberOfWeeks = this.numberOfWeeks;
        this.numberOfWeeks = this.depositLogProjection.numberOfLastWorkingDaysOrWeeks;
        this.trackNumberOfWeeks = this.numberOfWeeks;
        this.onMonthChangeCalled = false;
    }

    convert(str) {
        let mnths = {
            Jan: '01', Feb: '02', Mar: '03', Apr: '04', May: '05', Jun: '06',
            Jul: '07', Aug: '08', Sep: '09', Oct: '10', Nov: '11', Dec: '12'
        },
            date = str.split(' ');
        return new Date([date[3], mnths[date[1]], date[2]].join('-'));
    }

    calulateTotal() {
        this.messages = [];
        this.depositeLogClientData.total = 0;
        for (let i = 0; i < this.depositeLogClientData.payers.length; i++) {
            if (this.depositeLogClientData.payers[i].amount != undefined) {
                this.depositeLogClientData.total = this.depositeLogClientData.total + this.depositeLogClientData.payers[i].amount;
                if (this.depositeLogClientData.payers[i].amount > 100000000) {
                    this.messages = [];
                    this.messages.push({ severity: 'error', summary: 'Deposit Log', detail: 'Maximum 8 digits allowed' });
                }
            }
        }
    }

    changedChart() {
        this.dataset = {
            labels: this.payerChartData.map(a => a.payerName),
            datasets: [
                {
                    label: 'My First dataset',
                    backgroundColor: '#42A5F5',
                    data: this.payerChartData.map(a => a.amount)
                }
            ]
        };
    }

    reset(depositLog: any) {
        this.messages = [];
        this.depositeLogClientData.payers.forEach(c => c.amount = null);
        this.depositeLogClientData.total = null;
        // this.depositeLogClientData.date = new Date(null);
        let selectedDate = this.depositDate;
        this.depositLog.reset();
        this.depositDate = selectedDate != null ? new Date(selectedDate) : new Date();
        this.enteredDepositNotValid = this.enteredDepositNotValid.map(e => false);
        this.enteredExpectedAmountNotValid = false;
        this.Difference = undefined;
        depositLog.submitted = false;
    }

    showDepositLogTableView() {
        this.getDepositLog(this.clientCode, this.selectedMonth, this.selectedYear);
        this.showDepositLogGrid = true;
        this.showDepositLogGraph = false;
        this.showMonthDepositsOfAYear = false;
    }

    showDepositLogGraphView() {
        this.getBarChart(this.clientCode, this.selectedMonth, this.selectedYear);
        this.showDepositLogGraph = true;
        this.showDepositLogGrid = false;
        this.showMonthDepositsOfAYear = false;
    }


    /**
     * Gets ProgressValues
     */
    public getProgress() {
        if (this.curr_business_days && this.tot_business_days) {
            return ((this.curr_business_days / this.tot_business_days) * 100).toString() + '%';
        }
    }

    updateProjectedCash() {
        if (this.showSimpleBusinessDays) {
            if (this.numberOfLastWorkingDays > 0 && this.numberOfLastWorkingDays < this.labels.MAX_LAST_BUSINESS_DAYS) {
                this.trackNumberOfLastWorkingDays = this.numberOfLastWorkingDays;
                this.getSimpleBusinessProjectedCash(this.clientCode, this.numberOfLastWorkingDays, this.selectedMonth, this.selectedYear, parseInt(this.savedLastNumberOfBusienssDays));
            } else {
                this.numberOfLastWorkingDays = this.trackNumberOfLastWorkingDays;
            }
        } else {
            if (this.numberOfWeeks > 0 && this.numberOfWeeks < this.labels.MAX_LAST_BUSINESS_DAYS) {
                this.trackNumberOfWeeks = this.numberOfWeeks;
                this.getProjectedCashOfPreviousWeeks(this.clientCode, this.selectedMonth, this.selectedYear, this.numberOfWeeks);
            } else {
                this.numberOfWeeks = this.trackNumberOfWeeks;
            }
        }
    }

    getDataBasedOnDays() {
        this.manageNumberOfLastWorkingDaysOrWeeks();
        this.getSimpleBusinessProjectedCash(this.clientCode, this.numberOfLastWorkingDays, this.selectedMonth, this.selectedYear, parseInt(this.savedLastNumberOfBusienssDays));
        this.showSimpleBusinessDays = true;
    }

    getDataBasedOnWeeks() {
        this.manageNumberOfLastWorkingDaysOrWeeks();
        this.getProjectedCashOfPreviousWeeks(this.clientCode, this.selectedMonth, this.selectedYear, this.numberOfWeeks);
        this.showSimpleBusinessDays = false;
    }

    rearrangeProjectedCashData() {
        let tempProjectedArray = new Array();
        let tempPaymentsArray = new Array();
        this.yScaleMaxValue = 0;
        let paymentValue = 0;
        if (this.lastWorkingDays >= 10) {
            this.minRange = this.lastWorkingDays - this.numberOfBars + 1;
            for (let i = this.minRange; i <= this.lastWorkingDays; i++) {
                tempProjectedArray.push(this.lastWorkingDaysProjectedCash[i - 1]);
                tempPaymentsArray.push(this.lastWorkingDaysPayments[i - 1]);
                paymentValue = this.lastWorkingDaysPayments[i - 1].value;
                this.setYScaleMaxValue(this.lastWorkingDaysProjectedCash[i - 1].value, this.yScaleMaxValue);
            }
        } else {
            this.minRange = 1;
            for (let i = 0; i < this.lastWorkingDays; i++) {
                tempProjectedArray.push(this.lastWorkingDaysProjectedCash[i]);
                tempPaymentsArray.push(this.lastWorkingDaysPayments[i]);
                paymentValue = this.lastWorkingDaysPayments[i].value;
                this.setYScaleMaxValue(this.lastWorkingDaysProjectedCash[i].value, this.yScaleMaxValue);
            }
        }
        if (this.yScaleMaxValue < paymentValue) {
            this.yScaleMaxValue = paymentValue;
        }
        this.barChart = tempProjectedArray;
        this.lineChartSeries[0].series = tempPaymentsArray;
    }

    updateLastWorkingDaysProjectedCash() {
        this.getProjectedCashOfLastWorkingDays(this.clientCode, this.currentDate.getMonth() + 1, this.currentDate.getFullYear(), this.lastWorkingDays);
    }

    setYScaleMaxValue(amount, scaleMaxValue) {
        if (amount > scaleMaxValue) {
            this.yScaleMaxValue = amount;
        }

    }

    showProjectedCashVsBusinessDays() {
        if (this.selectedMonth == this.currentDate.getMonth() + 1) {
            this.showProjectedCashVsDays = true;
        } else {
            this.showProjectedCashVsDays = false;
        }
    }

    closedisplayProjectionDayMessage() {
        this.displayProjectionDaysMessage = false;
    }

    closedisplayProjectionWeekMessage() {
        this.displayProjectionWeeksMessage = false;
    }

    /* Formatting Annual amount */
    transformAmountToCurrency(amount: any) {
        return this.currencyPipe.transform(amount, 'USD');
    }

    formatePayerAnnualAmount() {
        let that = this;
        this.depositeLogClientData.payers.forEach(function (element) {
            // if (element.amount >= 0) {
            element.amountCurrencyFormat = that.transformAmountToCurrency(element.amount);
            // }
        });
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

    onBlur(element: any, index?: number) {
        let number = this.transformToNumber(element.target.value);
        // if (number >0) {
        element.target.value = this.transformAmountToCurrency(number);
        number = this.transformToNumber(element.target.value);
        if (index == undefined) {
            this.expectedTotalCurrencyFormat = element.target.value;
            this.expectedTotal = number;
        } else {
            this.depositeLogClientData.payers[index].amount = number;
            this.calulateTotal();
        }
        if (this.expectedTotal != 0) {
            this.Difference = this.transformAmountToCurrency(this.expectedTotal - this.getDepositSum());
        }
    }

    onfocus(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
        }
    }

    onkeyup(element: any, index?: number) {
        if (index == undefined) {
            this.validateExpectedTotal(element);
        } else {
            let regex = /^[+-]?[0-9]{1,8}(?:\.[0-9]{1,2})?$/;
            if (regex.test(element.target.value.trim()) || element.target.value.trim() == '') {
                this.enteredDepositNotValid[index] = false;
            } else {
                this.enteredDepositNotValid[index] = true;
            }
        }
    }

    validateExpectedTotal(element: any) {
        let regex = /^[+-]?[0-9]{1,10}(?:\.[0-9]{1,2})?$/;
        if (regex.test(element.target.value.trim()) || element.target.value.trim() == '') {
            this.enteredExpectedAmountNotValid = false;
        } else {
            this.enteredExpectedAmountNotValid = true;
        }
    }

    getDepositSum() {
        let sum = this.depositeLogClientData.payers.reduce(function (prev, cur) {
            return prev + cur.amount;
        }, 0);
        return sum;
    }

    /**
     * Triggers on click of "Close Month" button which is next to "Grid View" button
     */
    showMonthWiseDepositsInAYear() {
        this.depositYear = this.depositDate.getFullYear().toString();
        this.getDepositLogInAYearForClient(true);
    }

    /**
     * Frame all the 12 months' data details
     */
    constructMonthDepositsInYearData() {
        let currentYear = this.today.getFullYear();
        let currentMonth = this.today.getMonth() + 1;
        let canReopenMonth = this.canEdit && this.editActions.includes(this.labels.ROLE_ACTION.OPEN_CLOSED_MONTH);
        let canCloseMonth = this.canEdit && this.editActions.includes(this.labels.ROLE_ACTION.VIEW_CLOSE_MONTH);
        this.setRoleBasedCloseMonthTitle(canReopenMonth, canCloseMonth);
        let self = this;
        this.monthDepositsDataOfAYear.forEach(function (element) {
            if (element.monthStatus === self.labels.CLOSED_STATUS) {
                if (canReopenMonth) {
                    element.isButtonVisible = true;
                    element.elementText = self.labels.REOPEN;
                } else {
                    element.isButtonVisible = false;
                    element.elementText = self.labels.CLOSED;
                    element.textColorClass = 'green-color';
                }
            } else if (element.monthStatus === self.labels.PENDING_STATUS || element.monthStatus === self.labels.REOPEN_STATUS
                || (element.monthStatus == null && ((Number(self.depositYear) == currentYear && element.monthID <= currentMonth) || Number(self.depositYear) < currentYear))) {
                if (canCloseMonth) {
                    element.isButtonVisible = true;
                    element.elementText = self.labels.CLOSE;
                    if (Number(self.depositYear) == currentYear && element.monthID == currentMonth) {
                        element.isButtonDisabled = true;
                    } else {
                        element.isButtonDisabled = false;
                    }
                } else {
                    element.isButtonVisible = false;
                    if (Number(self.depositYear) == currentYear && element.monthID == currentMonth) {
                        element.elementText = '';
                        element.textColorClass = '';
                    } else {
                        element.elementText = self.labels.YET_TO_CLOSE;
                        element.textColorClass = 'red-color';
                    }
                }
            }
        });
    }

    /**
     * Set Close Month Page title based on logged-in user's role
     * @param canReopenMonth
     * @param canCloseMonth
     */
    setRoleBasedCloseMonthTitle(canReopenMonth, canCloseMonth) {
        if (canReopenMonth) {
            this.closeMonthTitle = this.labels.CLOSE_REOPEN_MONTHS;
        } else if (canCloseMonth) {
            this.closeMonthTitle = this.labels.CLOSE_MONTHS;
        } else {
            this.closeMonthTitle = this.labels.MONTH_STATUS;
        }
    }

    /**
     * Initializes year picker in Close Month View
     */
    initializeYearpicker() {
        let self = this;
        jQuery('#yearPicker').datepicker({
            format: 'yyyy',
            viewMode: 'years',
            minViewMode: 'years',
            autoclose: true
        }).change(function (e) {
            self.depositYear = e.currentTarget.value;
            self.getDepositLogInAYearForClient(false);
            }).on('changeDate', function (e) {
                self.depositYear = e.date.getFullYear();
            }).on('show', function (e) {
                jQuery('.datepicker-years td').children().removeClass('active focused');
                jQuery(`.datepicker-years td span:contains("${self.depositYear}")`).addClass("active focused");
            }).on('hide', function (e) {
                jQuery('#yearPicker').val(self.depositYear);
            });
    }

    // Show Year picker on click of calendar icon
    showYearPicker() {
        jQuery('#yearPicker').datepicker('show');
    }

    /**
    * save the last entered business days or weeks for a client
    */
    saveLastEnteredBusinessDaysOrWeeks() {
        this.depositLogAttribute = new DepositLogAttributeViewModel();
        this.depositLogAttribute.clientCode = this.clientCode;
        if (this.showSimpleBusinessDays) {
            this.depositLogAttribute.businessDaysOrWeeksValue = this.numberOfLastWorkingDays;
            this.depositLogAttribute.attributeName = this.labels.BUSINESS_DAYS_ATTRIBUTE;
        } else {
            this.depositLogAttribute.businessDaysOrWeeksValue = this.numberOfWeeks;
            this.depositLogAttribute.attributeName = this.labels.WEEKS_ATTRIBUTE;
        }
        this.depositLogService.saveLastEnteredBusinessDaysOrWeeks(this.depositLogAttribute).subscribe(
            (resp) => {
                if (resp) {
                    this.validationResponse = resp;
                    if (this.validationResponse.success) {
                        this.saveMessages = [];
                        this.saveMessages.push({ severity: 'success', summary: 'Entered Value', detail: 'saved successfully' });
                        this.getSavedLastNumberOfBusinessDaysOrWeeks();
                    } else {
                        this.saveMessages = [];
                        this.saveMessages.push({ severity: 'error', summary: 'Entered Value', detail: 'Error occured while saving data' });
                    }
                }
            },
            error => { }
        );
    }

    getLastSavedAttributeData() {
        if (this.savedLastNumberOfBusienssDays === null || this.savedLastNumberOfWeeks === null) {
            this.getSavedLastNumberOfBusinessDaysOrWeeks();
        } else {
            if (this.onMonthChangeCalled) {
                this.numberOfLastWorkingDays = parseInt(this.savedLastNumberOfBusienssDays);
                this.numberOfWeeks = parseInt(this.savedLastNumberOfWeeks);
            }
            this.updateProjectedCash();
        }
    }

    /**
    * To get last saved attributes data.
    */
    getSavedLastNumberOfBusinessDaysOrWeeks() {
        this.depositLogService.getSavedLastNumberOfBusinessDaysOrWeeks(this.clientCode).subscribe(
            (resp) => {
                if (resp != null) {
                    this.savedLastNumberOfBusienssDays = resp.LastEnteredBusinessDays;
                    this.savedLastNumberOfWeeks = resp.LastEnteredWeeks;
                    this.manageNumberOfLastWorkingDaysOrWeeks();
                    this.updateProjectedCash();
                } else {
                    this.saveMessages = [];
                    this.saveMessages.push({ severity: 'error', summary: '', detail: this.sharedLabels.ERROR_GET_DETAILS });
                }
            }
        );
    }

    manageNumberOfLastWorkingDaysOrWeeks() {
        if (this.savedLastNumberOfBusienssDays === null) {
            this.savedLastNumberOfBusienssDays = this.labels.DEFAULT_LAST_BUSINESS_DAYS.toString();
            this.numberOfLastWorkingDays = this.labels.DEFAULT_LAST_BUSINESS_DAYS;
        } else {
            this.numberOfLastWorkingDays = parseInt(this.savedLastNumberOfBusienssDays);
        }
        if (this.savedLastNumberOfWeeks === null) {
            this.savedLastNumberOfWeeks = this.labels.DEFAULT_LAST_WEEKS.toString();
            this.numberOfWeeks = this.labels.DEFAULT_LAST_WEEKS;
        } else {
            this.numberOfWeeks = parseInt(this.savedLastNumberOfWeeks);
        }
    }

    showDepositLogExport() {
        let minDepositDate = new Date().getFullYear();
        this.depositLogService.getClientMinimumDepositYear(this.clientCode).subscribe(
            (data) => {
                if (data != 0) {
                    if (minDepositDate != data) {
                        this.calendarYears = _.range(data, minDepositDate + 1).map(year => {
                            return {
                                label: year,
                                value: year
                            };
                        });
                    } else {
                        this.calendarYears = [minDepositDate].map(year => {
                            return {
                                label: year,
                                value: year
                            };
                        });
                    }
                    console.log(this.calendarYear);
                }
                else {
                    this.calendarYears = [minDepositDate].map(year => {
                        return {
                            label: year,
                            value: year
                        };
                    });
                }
            })

        this.monthsToSelect = moment.months();
        this.months = new Array<string>();
        this.selectedMonths = [];
        let i = 0;
        for (let month of this.monthsToSelect) {
            this.months.push({ label: month, value: i });
            i++;
        }
        this.exportDepositLog = true;
    }

    export() {
        this.selectedMonths.sort(function (a, b) { return a - b });
        this.selectedMonthArray = [];
        this.selectedMonthArray = this.selectedMonths.map(x => new Date(this.calendarYear, x, 1).toLocaleDateString());
        this.exportdata = new ExportDepositViewModel();
        this.exportdata.clientCode = this.clientCode;
        this.exportdata.exportMonths = this.selectedMonthArray
        this.depositLogService.exportDepositData(this.exportdata)
            .subscribe(data => this.downloadFile(data)),
            error => {
                this.saveMessages = [];
                this.saveMessages.push({ severity: 'error', summary: 'Entered Value', detail: 'Error downloading the file.' })
                console.log("Error downloading the file.")
            },
            () => {
                this.exportDepositLog = false;
                this.saveMessages = [];
                this.saveMessages.push({ severity: 'success', summary: 'Entered Value', detail: 'Completed file download.' })
                console.log('Completed file download.');
            }
    }

    downloadFile(data) {
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(data);
        link.download = this.clientCode + '_' + this.today.toLocaleString() + '_' + 'Report.xlsx';

        link.click();
        this.exportDepositLog = false;
    }

    /**
     *check whether a value is a number
     * @param val
     */
    isNumber(val) {
        return isNaN(Number(val));
    }

    /**
     * hide or show Add Deposit section
     */
    hideOrShowAddDeposit() {
        this.isAddDepositVisible = !this.isAddDepositVisible;
    }

    /**
     * To edit deposits on particular date
     * @param depositDate
     */
    onDepositEdit(depositDate: Date) {
        this.isAddDepositVisible = true;       
        jQuery('#addDepositSectionInner').addClass('highlight-addDeposit');
        this.depositDate = new Date(depositDate);
        this.OnDateChange(this.depositDate);   
        setTimeout(() => {    
            jQuery('#addDepositSectionInner').removeClass('highlight-addDeposit');
        }, 2000);
    }

    /**
     * Highlight Entered Deposit Dates
     */
    highlightDates() {
        jQuery('.deposit-exist').parent().css({ 'background-color': '#c2f0c2', 'border': '1px solid #ffffff' });
        let isDepositExist = jQuery('.ui-state-active') && jQuery('.ui-state-active').children().hasClass('deposit-exist');
        let styleClass = isDepositExist ? 'datePicker-color shadow' : 'date-transparent shadow';
        jQuery('.ui-datepicker.ui-widget .ui-datepicker-calendar td a.ui-state-active').addClass(styleClass);
    }

    /* ------ end region public methods -----------------*/

    /*------- region private methods --------*/

    /**
     * Gets total and deposit days for progress bar calculation
     */
    private getDaysForProgress() {
        this.getTotBusinessDays(this.selectedYear, this.selectedMonth);
        this.getNumberOfDepositDaysOfClientForGivenMonth(this.selectedYear, this.selectedMonth);
    }

    /**
     * Check if deposit is entered for a date
     * @param date
     */
    private isDepositEnteredForDate(date) {
        let calendarMonth = date.month + 1;
        let calendarDate = new Date(date.year, date.month, date.day).toLocaleDateString();
        return this.submittedDepositDates.includes(calendarDate);
    }

    /**
     * Add Submitted Deposits' Date To SubmittedDepositDates Array if not exists
     * @param depositDate
     */
    private addSelectedDepositDateToSubmittedDepositDates(depositDate) {
        let enteredDepositDate = new Date(depositDate).toLocaleDateString();
        if (!this.submittedDepositDates.includes(enteredDepositDate)) {
            this.submittedDepositDates.push(enteredDepositDate);
        }
    }

    /**
     * Get Month Short Name from Date (javascript Date())
     * @param date
     */
    private getMonthShortName(date) {
        return date.toLocaleString('en-us', { month: 'short' });;
    }

    /**
     * Set Deposit Date to first non-holiday date on change of month
     */
    private setDepositLogDateAndGetDepositData() {
        let firstApplicableDayInMonth = this.getFirstApplicableDateInAMonth(this.selectedYear, this.selectedMonth - 1);
        if (firstApplicableDayInMonth !== null) {
            let isFutureDate = (firstApplicableDayInMonth > this.today);
            let isPresentMonthYear = (firstApplicableDayInMonth.getMonth() === this.today.getMonth() && firstApplicableDayInMonth.getFullYear() === this.today.getFullYear());
            this.depositDate = isFutureDate || isPresentMonthYear ? this.today : firstApplicableDayInMonth;
            this.OnDateChange(this.depositDate);
        }
    }

    /**
     * Get first non-holiday date (this date should not be present in business holidays)
     * @param year
     * @param monthID
     */
    private getFirstApplicableDateInAMonth(year, monthID) {
        let applicableDate = null;
        if (this.holidayDatesInString) {
            applicableDate = new Date(year, monthID, 1);
            while (this.holidayDatesInString.includes(applicableDate.toLocaleDateString())) {
                applicableDate.setDate(applicableDate.getDate() + 1);
            }
        }
        return applicableDate;
    }

    /**
     * Check if today is a holiday
     * @param holidays
     */
    private isCurrentDayAHoliday(holidays) {
        return holidays.includes(this.today.toLocaleDateString());
    }

    /**
     * Check if deposit date is today.
     */
    private IsTodayADepositDate() {
        return (this.depositDate.toLocaleDateString() === this.today.toLocaleDateString());
    }

    /**
     * Increase Deposit Date
     */
    private incrementDate() {
        let nextApplicableDay = this.getDepositDateDuringIncrement();
        if (nextApplicableDay > this.today) {
            return;
        }
        this.depositDate = nextApplicableDay;
        this.OnDateChange(this.depositDate);
    }

    /**
     * Decrease Deposit Date
     */
    private decrementDate() {
        this.depositDate = this.getDepositDateDuringDecrement();
        this.OnDateChange(this.depositDate);
    }

    /**
     * Get DepositDate During Increment. Skip the date if holiday and proceed the same.
     */
    private getDepositDateDuringIncrement() {
        let applicableDay = new Date(this.depositDate);
        applicableDay.setDate(this.depositDate.getDate() + 1);
        while (this.holidayDatesInString.includes(applicableDay.toLocaleDateString())) {
            applicableDay.setDate(applicableDay.getDate() + 1);
        }
        return applicableDay;
    }

    /**
     * Get DepositDate During Decrement. Skip the date if holiday and proceed the same.
     */
    private getDepositDateDuringDecrement() {
        let applicableDay = new Date(this.depositDate);
        applicableDay.setDate(this.depositDate.getDate() - 1);
        while (this.holidayDatesInString.includes(applicableDay.toLocaleDateString())) {
            applicableDay.setDate(applicableDay.getDate() - 1);
        }
        return applicableDay;
    }

    /**
     * Calculate the projected to collect amount.
     */
    private calculateProjectedToCollect() {
        if (this.depositLogMTDValue < this.projectedCashAmount) {
            this.projectedToCollectAmount = this.projectedCashAmount - this.depositLogMTDValue;
        } else {
            this.projectedToCollectAmount = 0;
        }
    }

    /*------- end region private methods --------*/

}





