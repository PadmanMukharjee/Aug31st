import { Component, OnInit, ViewEncapsulation, ViewChild, Input, Output, AfterViewInit } from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { TARGETS, SHARED } from '../../shared/utilities/resources/labels';
import { MonthlyTarget } from './models/monthly-target.model';
import { BusinessDays } from './models/business-days.model';
import { TargetService } from './targets.service';
import { Column } from 'primeng/primeng';
import { combineAll } from 'rxjs/operator/combineAll';
import { CurrencyPipe, PercentPipe, DecimalPipe, getLocaleNumberFormat, NumberFormatStyle, NumberSymbol } from '@angular/common';
import { ManuallyEditedTargets } from './models/manually-edited-targets.model';
import { ValidationMessageComponent } from '../../common/components/validation-message/validation-message.component';
import { Form } from '@angular/forms';
import { FormControl, NgForm } from '@angular/forms';
import { EventEmitter, ElementRef } from '@angular/core';
import { ValidationResponseViewModel } from '../../common/models/validation.model';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { debounce } from 'rxjs/operator/debounce';

// Primeng Imports
import { Message } from 'primeng/components/common/api';

@Component({
    selector: 'monthly-targets',
    templateUrl: './targets.component.html',
    styleUrls: ['./targets.component.css'],
    providers: [CurrencyPipe, PercentPipe]
})

export class TargetComponent implements OnInit, AfterViewInit {

    /*------ region public properties ------*/

    labels = TARGETS;
    calendarYears: number[];
    monthlyTarget: MonthlyTarget;
    CalendarYear: number;
    AnnualCharges: any;
    GrossCollection: any;
    TotalCharges = 0;
    TotalPayments = 0;
    TotalRevenue = 0;
    graphView = true;
    tableView = false;
    // tableData: any[] = [];
    selectedRow: any;
    manuallyEditedTargets: ManuallyEditedTargets;
    businessDays: BusinessDays[] = [];
    public msgs: Message[] = [];
    businessDaysInYear: any;
    public validationResponse: ValidationResponseViewModel;
    public displayMonthlyTargetsErrorMessage = false;
    public validateMonthlyTargetsData = true;
    public isAnnualChargesValid = true;
    public isGrossCollectionValid = true;

    columns = [
        { field: 'month', header: 'Months' },
        { field: 'businessDays', header: 'Business Days' },
        { field: 'charges', header: 'Charges' },
        { field: 'payments', header: 'Payments' },
        { field: 'revenue', header: 'Revenue' }
    ];

    charges: any[] = [
        { 'name': 'Jan', 'value': 0 },
        { 'name': 'Feb', 'value': 0 },
        { 'name': 'Mar', 'value': 0 },
        { 'name': 'Apr', 'value': 0 },
        { 'name': 'May', 'value': 0 },
        { 'name': 'Jun', 'value': 0 },
        { 'name': 'Jul', 'value': 0 },
        { 'name': 'Aug', 'value': 0 },
        { 'name': 'Sep', 'value': 0 },
        { 'name': 'Oct', 'value': 0 },
        { 'name': 'Nov', 'value': 0 },
        { 'name': 'Dec', 'value': 0 },
    ];
    payments: any[] = [
        { 'name': 'Jan', 'value': 0 },
        { 'name': 'Feb', 'value': 0 },
        { 'name': 'Mar', 'value': 0 },
        { 'name': 'Apr', 'value': 0 },
        { 'name': 'May', 'value': 0 },
        { 'name': 'Jun', 'value': 0 },
        { 'name': 'Jul', 'value': 0 },
        { 'name': 'Aug', 'value': 0 },
        { 'name': 'Sep', 'value': 0 },
        { 'name': 'Oct', 'value': 0 },
        { 'name': 'Nov', 'value': 0 },
        { 'name': 'Dec', 'value': 0 },
    ];
    revenue: any[] = [
        { 'name': 'Jan', 'value': 0 },
        { 'name': 'Feb', 'value': 0 },
        { 'name': 'Mar', 'value': 0 },
        { 'name': 'Apr', 'value': 0 },
        { 'name': 'May', 'value': 0 },
        { 'name': 'Jun', 'value': 0 },
        { 'name': 'Jul', 'value': 0 },
        { 'name': 'Aug', 'value': 0 },
        { 'name': 'Sep', 'value': 0 },
        { 'name': 'Oct', 'value': 0 },
        { 'name': 'Nov', 'value': 0 },
        { 'name': 'Dec', 'value': 0 },
    ];
    months: any[] = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    tableData: any[] = [
        { 'month': this.months[0], 'businessDays': 21, 'charges': this.charges[0].value, 'payments': this.payments[0].value, 'revenue': this.revenue[0].value },
        { 'month': this.months[1], 'businessDays': 21, 'charges': this.charges[1].value, 'payments': this.payments[1].value, 'revenue': this.revenue[1].value },
        { 'month': this.months[2], 'businessDays': 21, 'charges': this.charges[2].value, 'payments': this.payments[2].value, 'revenue': this.revenue[2].value },
        { 'month': this.months[3], 'businessDays': 21, 'charges': this.charges[3].value, 'payments': this.payments[3].value, 'revenue': this.revenue[3].value },
        { 'month': this.months[4], 'businessDays': 21, 'charges': this.charges[4].value, 'payments': this.payments[4].value, 'revenue': this.revenue[4].value },
        { 'month': this.months[5], 'businessDays': 21, 'charges': this.charges[5].value, 'payments': this.payments[5].value, 'revenue': this.revenue[5].value },
        { 'month': this.months[6], 'businessDays': 21, 'charges': this.charges[6].value, 'payments': this.payments[6].value, 'revenue': this.revenue[6].value },
        { 'month': this.months[7], 'businessDays': 21, 'charges': this.charges[7].value, 'payments': this.payments[7].value, 'revenue': this.revenue[7].value },
        { 'month': this.months[8], 'businessDays': 21, 'charges': this.charges[8].value, 'payments': this.payments[8].value, 'revenue': this.revenue[8].value },
        { 'month': this.months[9], 'businessDays': 21, 'charges': this.charges[9].value, 'payments': this.payments[9].value, 'revenue': this.revenue[9].value },
        { 'month': this.months[10], 'businessDays': 21, 'charges': this.charges[10].value, 'payments': this.payments[10].value, 'revenue': this.revenue[10].value },
        { 'month': this.months[11], 'businessDays': 21, 'charges': this.charges[11].value, 'payments': this.payments[11].value, 'revenue': this.revenue[11].value },
    ];


    targetData: any = [
        {
            'name': 'Charges',
            'series': [
            ]
        },
        {
            'name': 'Payments',
            'series': [
            ]
        },
        {
            'name': 'Revenue',
            'series': [
                {
                    'name': '',
                    'value': Number
                }
            ]
        },
    ];

    view: any[];

    // options for Line chart
    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = true;
    showXAxisLabel = true;
    xAxisLabel = this.labels.ChartXAxisLabel;
    showYAxisLabel = true;
    yAxisLabel = this.labels.ChartYAxisLabel;
    legendTitle = 'Legend';

    colorScheme = {
        domain: ['#FF3333', '#3364FF', '#50AF54', '#AAAAAA']
    };

    public canEdit = false;
    public editActions: any[] = [];

    // line, area
    autoScale = false;

    /*------ end region public properties ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.getTargetData(new Date().getFullYear());
        this.getBusinessDays();
    }


    ngAfterViewInit(): void {
        setTimeout(_ => {
            window.dispatchEvent(new Event('resize'));
        }); // BUGFIX:
    }

    /*------end  region life cycle hooks ------*/

    /*-----region Input/Output bindings -----*/

    @Input() clientCode: string;
    @Input('isSaveAndExit') isSaveAndExit: boolean;
    @Output('saveTargetStepStatus') SaveTargetStepStatus = new EventEmitter();
    @ViewChild("targetsDataFormSubmit") public targetsDataFormSubmit: ElementRef;
    @ViewChild("monthlyTarget") public monthlyTargetForm: NgForm;

    /*-----endregion Input/Output bindings -----*/



    /*------ region constructor ------*/
    constructor(private _targetService: TargetService,
        private userService: UserService, private currencyPipe: CurrencyPipe, private percentPipe: PercentPipe
    ) {
        this.getScreenActions();
        this.calendarYears = [new Date().getFullYear(), new Date().getFullYear() + 1];
        this.monthlyTarget = new MonthlyTarget();
        this.manuallyEditedTargets = new ManuallyEditedTargets();
        this.plotGraph();
        // this.getTargetData(new Date().getFullYear());
    }

    /*------ end region constructor ------*/


    /*------ region Public methods ------*/

    plotGraph() {
        this.targetData[0].series = [...this.charges];
        this.targetData[1].series = [...this.payments];
        this.targetData[2].series = [...this.revenue];
        this.targetData = [...this.targetData];
    }

    onDropdownChange(value) {
        this.getTargetData(this.CalendarYear);
        this.updateBusinessDays();
    }

    changeToGraphView() {
        this.graphView = true;
        this.tableView = false;
    }
    changeToTableView() {
        if (this.TotalCharges != 0 || this.TotalPayments != 0) {
            this.graphView = false;
            this.tableView = true;
        }
    }

    // Validating the fields before saving Targets
    validateData(monthlyTarget) {
        if (this.AnnualCharges == 0 || this.AnnualCharges == undefined ||
            this.GrossCollection == 0 || this.GrossCollection == undefined ||
            this.CalendarYear == 0 || this.CalendarYear == undefined) {
            return false;
        } else {
            return true;
        }

    }

    // Method to save Targets through Quick method
    saveTargetData(monthlyTarget) {
        this.TotalCharges = 0;
        this.TotalPayments = 0;
        this.TotalRevenue = 0;
        if (monthlyTarget.valid && this.isAnnualChargesValid && this.isGrossCollectionValid) {
            this.monthlyTarget = new MonthlyTarget();
            this.monthlyTarget.ClientCode = this.clientCode;
            this.monthlyTarget.AnnualCharges = this.transformToNumber(this.AnnualCharges);
            this.monthlyTarget.GrossCollectionRate = this.GrossCollection.slice(0, -1);
            this.monthlyTarget.CalendarYear = this.CalendarYear;
            this.validationResponse = new ValidationResponseViewModel();
            this._targetService.saveTargets(this.monthlyTarget).subscribe(
                data => {
                    this.validationResponse = data.validationViewModel;
                    if (this.validationResponse.success) {
                        let clientTargetData = data.clientTargetViewModels;
                        if (clientTargetData != null && clientTargetData.length > 0) {
                            this.dataDistribution(clientTargetData);
                            this.plotGraph();
                            this.validationResponse.success = true;
                            this.monthlyTargetForm.form.markAsPristine();
                            this.SaveTargetStepStatus.emit(this.isSaveAndExit);
                        }
                    }
                },
                err => {
                    this.validationResponse.success = false;
                }
            );
        }
    }

    // Method to get the Client Target data
    getTargetData(year) {
        this.TotalCharges = 0;
        this.TotalPayments = 0;
        this.TotalRevenue = 0;
        this._targetService.getTargets(this.clientCode, year).subscribe(
            data => {
                if (data != null && data.length > 0) {
                    this.CalendarYear = data[0].calendarYear;
                    this.AnnualCharges = this.transformAmountToCurrency(data[0].annualCharges);
                    this.GrossCollection = data[0].grossCollectionRate + '%';
                } else if (data == null) {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }
                this.dataDistribution(data);
                this.targetDataforTable();
                this.plotGraph();
            },
            err => { },
            () => { }
        );
    }

    dataDistribution(data: any) {
        let length = data.length;
        if (length > 0) {
            for (let i in data) {
                this.charges[i].value = data[i].charges;
                this.TotalCharges = this.TotalCharges + data[i].charges;
                this.payments[i].value = data[i].payments;
                this.TotalPayments = this.TotalPayments + data[i].payments;
                this.revenue[i].value = data[i].revenue;
                this.TotalRevenue = this.TotalRevenue + data[i].revenue;
            }
            this.targetDataforTable();
        } else {
            for (let i = 0; i < 12; i++) {
                this.charges[i].value = 0;
                this.payments[i].value = 0;
                this.revenue[i].value = 0;
            }
            this.AnnualCharges = '';
            this.GrossCollection = '';
            this.TotalCharges = 0;
            this.TotalPayments = 0;
            this.TotalRevenue = 0;
        }
    }

    targetDataforTable() {
        for (let i in this.months) {
            this.tableData[i].charges = this.charges[i].value;
            this.tableData[i].payments = this.payments[i].value;
            this.tableData[i].revenue = this.revenue[i].value;
        }
    }

    onCellEdit(event) {
        this.TotalCharges = 0;
        this.TotalPayments = 0;
        this.TotalRevenue = 0;
        this.dataDistribution(this.tableData);
    }

    // To Save the targets that are edited in Grid view
    saveManuallyEditedTargets() {
        this.validateMonthlyTargetsData = true;
        this.validatingManuallyEditedTargets(this.charges);
        this.validatingManuallyEditedTargets(this.payments);
        this.validatingManuallyEditedTargets(this.revenue);
        this.manuallyEditedTargets.charges = this.charges;
        this.manuallyEditedTargets.payments = this.payments;
        this.manuallyEditedTargets.revenue = this.revenue;
        this.manuallyEditedTargets.clientCode = this.clientCode;
        this.manuallyEditedTargets.year = this.CalendarYear;
        if (this.validateMonthlyTargetsData) {
            this.validationResponse = new ValidationResponseViewModel();
            this._targetService.saveManuallyEditedTargets(this.manuallyEditedTargets).subscribe(
                response => {
                    this.validationResponse = response;
                    if (this.validationResponse.success) {
                        this.SaveTargetStepStatus.emit(this.isSaveAndExit);
                    } else {
                        this.displayMonthlyTargetsErrorMessage = true;
                    }
                    this.getTargetData(this.CalendarYear);
                },
                err => { },
                () => { }
            );
        } else {
            this.displayMonthlyTargetsErrorMessage = true;
            this.getTargetData(this.CalendarYear);
        }
    }

    validatingManuallyEditedTargets(amountType: any[]) {
        for (let i = 0; i < amountType.length; i++) {
            if (amountType[i].value > 100000000) {
                this.validateMonthlyTargetsData = false;
            }
            if (amountType[i].value == null) {
                amountType[i].value = 0;
            }
        }
    }

    closeMonthlyTargetsMessage() {
        this.displayMonthlyTargetsErrorMessage = false;
    }

    // To get the business days to show in Table view
    getBusinessDays() {
        this._targetService.GetBusinessDays().subscribe(
            data => {
                if (data.length > 0) {
                    for (let i in data) {
                        this.businessDays.push({ Year: data[i].year, MonthID: data[i].monthId, Month: data[i].month, NumberofBusinessDays: data[i].businessDays });
                    }
                    this.updateBusinessDays();
                }
            },
            err => { console.log('error while getting Business days'); },
            () => { }

        );
    }

    // Update the business days of each month in grid
    updateBusinessDays() {
        if (this.CalendarYear) {
            let businessDaysofYear: BusinessDays[] = [];
            businessDaysofYear = this.businessDays.filter(x => x.Year == this.CalendarYear);

            for (let i in this.tableData) {
                this.tableData[i].businessDays = businessDaysofYear[i].NumberofBusinessDays;
            }

            let BusinessDaysInYearMap = businessDaysofYear.map(function (item) {
                return item.NumberofBusinessDays;
            });

            // Sum the array's values from left to right
            this.businessDaysInYear = BusinessDaysInYearMap.reduce(function (prev, curr) {
                return prev + curr;
            });
        }
    }

    // Emit target's graph view save when Save&Exit is clicked.
    saveTargetGraphDataFromParent() {
        this.targetsDataFormSubmit.nativeElement.click();
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
    /* Formatting Annual Charges and Gross Collection */
    validNumber(val: any) {
        let isValidChar = false;
        let exp = /^\d+(?:\.[0-9])?$/;
        isValidChar = exp.test(val);
        return isValidChar;
    }

    transformAmountToCurrency(amount: any) {
        return this.currencyPipe.transform(amount, 'USD');
    }

    transformToNumber(amount: any) {
        let number = parseFloat(amount.replace(/[^0-9-.]/g, ''));
        if (!isNaN(number)) {
            return number;
        } else {
            return 0;
        }
    }

    onBlur(element: any) {
        let number = this.transformToNumber(element.target.value);
        if (number > 0) {
            element.target.value = this.transformAmountToCurrency(number);
            this.AnnualCharges = element.target.value;
            number = this.transformToNumber(element.target.value);
            let regex = /^[+]?[0-9]{1,10}(?:\.[0-9]{1,2})?$/;
            if (!regex.test(number.toString())) {
                this.isAnnualChargesValid = false;
            } else {
                this.isAnnualChargesValid = true;
            }
        }
    }

    onkeyup(element: any) {
        let regex = /^[+]?[0-9]{1,10}(?:\.[0-9]{1,2})?$/;
        if (element.target.value != '' && element.target.value.charAt(0)!=='$') {
            if (regex.test(element.target.value)) {
                this.isAnnualChargesValid = true;
            } else {
                this.isAnnualChargesValid = false;
            }
        }
        else {
            this.isAnnualChargesValid = true;
        }
    }

    onfocus(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
        }
    }
    onfocusGC(element: any) {
        if (element.target.value != '') {
            let normal = this.transformToNumber(element.target.value);
            element.target.value = normal;
        }
    }

    onBlurGC(element: any) {
        if (element.target.value != '' && element.target.value.slice(-1) != '%') {
            element.target.value = element.target.value + '%';
            this.GrossCollection = element.target.value;
        }
    }

    onkeyUpGC(element: any) {
        if (element.target.value != '') {
            if (element.target.value.slice(-1)=='%') {
                element.target.value = element.target.value.replace('%', '');
            }
            let regex = /^(?:100(?:\.00?)?|\d?\d(?:\.\d\d?)?)$/;
            if (regex.test(element.target.value)) {
                this.isGrossCollectionValid = true;
            } else {
                this.isGrossCollectionValid = false;
            }
        }
    }

    // Get Edit Actions of Monthly Targets page
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.MONTHLYTARGETS)
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

    /*------ end region Public methods ------*/
}
