// Angular imports
import { Component, ChangeDetectorRef, OnInit, OnDestroy } from '@angular/core';
import { DatePipe } from '@angular/common';
import { NgForm } from '@angular/forms';

// File Imports
import { BusinessDaysService } from './business-days.service';
import { UserService } from '../../shared/services/user.service';
import { HolidayModel } from './holiday.model';
import { KeyValuePair } from '../../shared/models/DepositLog/KeyValuepair';
import { PieChartView } from './pie-chart.model';
import { BusinessDaysConstants } from '../../app.constants';
import { BUSINESS_DAYS, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';

// Primeng Imports
import { SelectItem } from 'primeng/components/common/selectitem';
import { Paginator, DataTable } from 'primeng/primeng';
import { Message } from 'primeng/components/common/message';

@Component({
    selector: 'app-businessdays',
    templateUrl: './business-days.component.html',
    styleUrls: ['../../../assets/build/css/switch.css', './business-days.component.css'],
    providers: [BusinessDaysService]
})
export class BusinessDaysComponent implements OnInit, OnDestroy {

    /*-------- region public properties ---------*/

    public holidays: HolidayModel[];
    public labels = BUSINESS_DAYS;
    public sharedLabels = ADMIN_SHARED;
    public displayDialog: boolean;
    public clientTargetDialog: boolean;
    public holidayDialogLabel: string;
    public totalBusinessDays: number;
    public selectedHoliday: HolidayModel;
    public isDeleteAction: boolean;
    public minDateValue;
    public showErrorMsg: boolean;
    public errorMsg: string;
    public clientTargetMsg: string;
    public series: PieChartView[];
    public pieChartView: PieChartView;
    public years: SelectItem[];
    public curentYear: number;
    public selectedYear: string;
    public canEdit = false;
    public editActions: any[] = [];
    public showErrorDiv: boolean;
    public msgs: Message[] = [];
    public validationConstants = VALIDATION_MESSAGES;
    public filteredBusinessDays: any[];
    public paginationCount: any;
    public rowsIn: number = 10;
    public selectedColumns: any[];
    public tableColumns: any[];
    public pieChartProperties = {
        view: [500, 500],
        showLegend: false,
        colorScheme: {
            domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA', '#0F4BA5']
        },
        showLabels: true,
        explodeSlices: false,
        doughnut: false,
        data: [],
        gradient: '',
        trimLabels: false,
    };

    /*--------end region public properties ---------*/

    /*-------- region Constructor ---------*/

    constructor(private businessDaysService: BusinessDaysService, private userService: UserService,
        private ref: ChangeDetectorRef) {
    }

    /*-------- end region constructor ---------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        let today = new Date();
        this.getScreenActions();
        this.holidays = [];
        this.years = [];
        this.curentYear = today.getFullYear();
        this.fillYearDropDown();
        this.selectedYear = this.curentYear.toString();
        this.showErrorDiv = false;
        this.displayDialog = false;
        this.isDeleteAction = false;
        this.showErrorMsg = false;
        this.clientTargetDialog = false;
        this.minDateValue = new Date(today.getFullYear(), today.getMonth(), today.getDate());
        this.selectedHoliday = new HolidayModel();
        this.msgs = [];
        this.tableColumns = [];
        this.selectedColumns = this.tableColumns;
        this.getHolidaysOfYear();
  }
 
    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------ end region life cycle hooks ------*/

    /*-------- region public Methods ---------*/

    /**
     * Selects holiday row for Editing or Deleting
     * @param holiday
     * @param isDelete
     */
    public modifySelectedHoliday(holiday: HolidayModel, isDelete: boolean) {
        this.displayDialog = true;
        this.isDeleteAction = isDelete;
        this.holidayDialogLabel = isDelete ? this.labels.DELETE_BUSINESS_HOLIDAY : this.labels.EDIT_BUSINESS_HOLIDAY;
        this.selectedHoliday.holidayName = holiday.holidayName;
        this.selectedHoliday.dateKey = holiday.dateKey;
        this.selectedHoliday.holidayDate = this.convertHolidayDateToString(holiday.holidayDate);
        this.focusOnHoliday();
    }

    /**
   * called when table's page changes
 */
    onTablePageChange(e) {
        let start = this.filteredBusinessDays.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.filteredBusinessDays.length ? this.filteredBusinessDays.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.filteredBusinessDays.length;
    }

    /**
     * Adding a Holiday
     */
    public addHoliday() {
        this.displayDialog = true;
        this.holidayDialogLabel = this.labels.ADD_BUSINESS_HOLIDAY;
        this.isDeleteAction = false;
        this.selectedHoliday = new HolidayModel();
        this.focusOnHoliday();
    }

    /**
     * Get Business Days Chart
     */
    getPieChart() {
        this.businessDaysService.getHolidaysByMonth(this.selectedYear).subscribe(
            data => {
                if (data != null) {
                    let tempData: any[];
                    let total = 0;
                    tempData = new Array();
                    for (var value in data) {
                        let keyValue = new KeyValuePair();
                        keyValue.name = value;
                        keyValue.value = data[value];
                        tempData.push(keyValue);
                        total = total + data[value];
                    }
                    this.totalBusinessDays = total;
                    this.pieChartProperties.data = tempData;
                } else {
                    this.msgs.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                    this.showErrorDiv = true;
                }
            }
        );
    }

    /**
     * Get Screen Actions for roles
     */
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.BUSINESSDAYS)
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
    * Fetches Holidays for a given year
    */
    public getHolidaysOfYear() {
        this.businessDaysService.getHolidaysOfYear(this.selectedYear).subscribe(
            data => {
                if (data != null && data.length > 0) {
                    this.holidays = data;
                    this.filteredBusinessDays = [...this.holidays];
           } else {
                    this.holidays = [];
                    this.filteredBusinessDays = [];
                }
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
                this.getPieChart();
            }
        );
    }

    /**
     * called when filter event occurs for table
     */
    onTableFilter(event) {
        this.filteredBusinessDays = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };
        this.onTablePageChange(e);
    }

    /**
   * Format the text for each month in pie chart
   * @param monthName
   */
    public monthFormatting(monthName): string {
        if (this.series) {
            this.pieChartView = this.series.find(p => p.name == monthName);
            if (this.pieChartView) {
                return `${this.pieChartView.name.substring(0, 3)}: ${this.pieChartView.value + ' Days'}`;
            }
        }
    }

    /**
   * RESET DATATABLE
   * @param holidayTable
   */
    public getHolidaysOfYearReset(holidayTable: DataTable) {
        holidayTable.reset();
        this.getHolidaysOfYear();
    }
    /*--------end region public Methods ---------*/


    /*-------- region private Methods ---------*/

    /**
     * To fil dropdown with curren and future year
     */
    private fillYearDropDown() {
        this.years.push({
            label: this.curentYear.toString(),
            value: this.curentYear.toString()
        });
        this.years.push({
            label: (this.curentYear + 1).toString(),
            value: (this.curentYear + 1).toString()
        });
    }

  /**
     * Update Client Targets
     */
    private updateClientTargets() {
        this.businessDaysService.updateClientTargets(this.selectedYear).subscribe(
            success => {
                if (success) {
                    this.clientTargetMsg = this.labels.CLIENT_TARGET_SUCSESS;
                } else {
                    this.clientTargetMsg = this.labels.CLIENT_TARGET_FAILED;
                }
                this.clientTargetDialog = true;
            }
        );
    }

    /**
     * Deletes Given Holiday
     * @param holiday
     */
    private deleteHoliday(holiday: HolidayModel) {
        holiday.holidayDate = this.convertHolidayDateToString(holiday.holidayDate);
        this.businessDaysService.deleteHolidays(this.selectedHoliday).subscribe(
            (success) => {
                if (success) {
                    this.getHolidaysOfYear();
                } else {
                    this.msgs.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_SAVE_MESSAGE });
                    this.showErrorDiv = true;
                }
            }
        );
        this.displayDialog = false;
    }

    /**
     * Validate Holiday Date
     */
    private isHolidayValid() {
        let isValid = true;
        let holiday = this.selectedHoliday;
        let holidayInDateFormat = new Date(holiday.holidayDate);
        if (holidayInDateFormat < this.minDateValue) {
            this.errorMsg = this.labels.VALIDATION_MSG;
            this.showErrorMsg = true;
            isValid = false;
        }
        //else if (holidayInDateFormat.getDay() == BusinessDaysConstants.Saturday || holidayInDateFormat.getDay() == BusinessDaysConstants.Sunday) {
        //    this.errorMsg = this.labels.VALIDATION_MSG_DAY;
        //    this.showErrorMsg = true;
        //    isValid = false;
        //}
        if (isValid) {
            this.showErrorMsg = false;
        }
        return isValid;
    }

    /**
     * Check whether holiday already exists or not.
     */
    private isExistingHoliday() {
        this.msgs = [];
        this.showErrorDiv = false;
        let holiday = this.selectedHoliday;
        let holidayInDateFormat = new Date(holiday.holidayDate);
        this.holidays.forEach(hl => {
            if (hl.dateKey != holiday.dateKey) {
                let currentdate = new Date(hl.holidayDate);
                if (currentdate.getDate() == holidayInDateFormat.getDate() &&
                    currentdate.getMonth() == holidayInDateFormat.getMonth() &&
                    currentdate.getFullYear() == holidayInDateFormat.getFullYear()) {
                    this.msgs = [];
                    this.msgs.push({ severity: this.validationConstants.WARN, summary: '', detail: this.labels.HOLIDAY_DATE_EXISTS_ERROR_MESSAGE });
                    this.showErrorDiv = true;
                }
                if (hl.holidayName == holiday.holidayName) {
                    this.msgs.push({ severity: this.validationConstants.WARN, summary: '', detail: this.labels.HOLIDAY_NAME_EXISTS_ERROR_MESSAGE });
                    this.showErrorDiv = true;
                }
            }
        });
        if (this.showErrorDiv) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * Add or Edit a holiday
     * @param form
     */
    private addOrEditHoliday(form: NgForm) {
        if (form.valid) {
            this.selectedHoliday.holidayName = this.selectedHoliday.holidayName.trim();
            this.selectedHoliday.holidayDate = this.convertHolidayDateToString(this.selectedHoliday.holidayDate);
            if (this.isHolidayValid() && !(this.isExistingHoliday())) {
                this.businessDaysService.addOrEditHoliday(this.selectedHoliday).subscribe(
                    (success) => {
                        if (success) {
                            this.getHolidaysOfYear();
                        } else {
                            this.msgs = [];
                            this.msgs.push({ severity: ADMIN_SHARED.ERROR, summary: '', detail: ADMIN_SHARED.ERROR_SAVE_MESSAGE });
                            this.showErrorDiv = true;
                        }
                    }
                );
                this.displayDialog = false;
            }
        }
    }

    /**
     * Gets Locale Date String for any given date
     * @param holidayDate
     */
    private convertHolidayDateToString(holidayDate: string): string {
        return new Date(holidayDate).toLocaleDateString();
    }

    /**
    * cancel popup
    */
    private clearDialog() {
        this.displayDialog = false;
        this.showErrorMsg = false;
        this.showErrorDiv = false;
    }

   /**
     * Hide Refresh Client targets popup
     */
    private hideClientPopUp() {
        this.clientTargetDialog = false;
    }

    /**
     * setting delete or edit buttons
     * @param holidayDate
     */
    private setEditOrDeleteBtn(holidayDate: string): boolean {
        if (new Date(holidayDate) >= this.minDateValue) {
            return true;
        }
        return false;
    }

    /**
     * focus on input field after popup is shown
     */
    private focusOnHoliday() {
        this.ref.detectChanges();
        jQuery('#txtholidayname').focus();
    }

   /*--------end region private Methods ---------*/

}
