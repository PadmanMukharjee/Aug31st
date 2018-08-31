// Angular imports
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MomentModule } from 'angular2-moment';
import * as moment from 'moment';
import { DaterangepickerConfig, DaterangePickerComponent } from 'ng2-daterangepicker';
import { IMonth } from 'ng2-date-picker';
import { DatePickerDirective } from 'ng2-date-picker';
import { DpDatePickerModule } from 'ng2-date-picker';
import { DatePickerComponent } from 'ng2-date-picker';
import { Subscription } from 'rxjs';

// Common File Imports
import { COMPLETED_CHECKLIST, SHARED } from '../../shared/utilities/resources/labels';
import { ChecklistService } from '../checklist.service';
import { ChecklistDataRequestViewModel } from '../models/checklist-datarequest.model';
import { GlobalEventsManager } from '../../shared/utilities/global-events-manager';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { ValidationResponseViewModel } from '../../common/models/validation.model';

// Primeng Imports
import { Message } from 'primeng/components/common/api';

@Component({
  selector: 'app-completed-checklist',
  templateUrl: './completed-checklist.component.html',
  styleUrls: ['./completed-checklist.component.css']
})
export class CompletedChecklistComponent implements OnInit, OnDestroy {
    @ViewChild('dateDirectivePicker') datePickerDirective: DatePickerDirective;

    /*------ region public properties ------*/
    public labels = COMPLETED_CHECKLIST;
    public sharedLabels = SHARED;
    public daterange: any = {};
    public isWeekly = true;
    public checklistTitle: string = this.labels.WEEKLY_CHECKLISTS;
    public columns: any[];
    public rowGroupMetadata: any;
    public checklists: any[];
    public showCompletedChecklists = false;
    public pageTitle: string;
    public clientCode: string;
    public datePickerConfig = {
        min: '',
        max: '',
        enableMonthSelector: true,
        isMonthDisabledCallback: false,
    };
    public theme = 'dp-material';
    public fromMonth: string;
    public toMonth: string;
    public errorMessage: string;
    public showErrorMessage = false;
    public fromMonthDate: any;
    public toMonthDate: any;
    public fromWeekDate: any;
    public toWeekDate: any;
    public initMonthPicker = false;
    public fromMonthChanged = false;
    public toMonthChanged = false;
    public fromMonthLoadCount = 0;
    public toMonthLoadCount = 0;
    public canEdit = false;
    public editActions: any[] = [];
    public validationResponse: ValidationResponseViewModel;
    public msgs: Message[] = [];
    public displayDialog = false;
    public dialogMessage: string;
    public checklistRequest: ChecklistDataRequestViewModel;
    public checklistFirstEffectiveDate: any;
    public defaultToDate: any;
    public defaultFromDate: any;
    public noChecklistMessage: string;
    public weeklyEffectiveDate: any;
    public monthlyEffectiveDate: any;
    /*------ end region public properties ------*/

    /*------ region private properties ------*/
    private clientCodeSubscriber: Subscription;
    @ViewChild(DaterangePickerComponent)
    private picker: DaterangePickerComponent;
    /*------ end region private properties ------*/

    /*------ region constructor ------*/
    constructor(private _globalEvents: GlobalEventsManager, private _userService: UserService,
                private daterangepickerOptions: DaterangepickerConfig, private _checklistService: ChecklistService) {
        this._globalEvents.setClientDropdown(true);
    }
    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit() {
        this.clientCodeSubscriber = this._globalEvents.getGlobalClientCode.subscribe((globalClientCode) => {
            if (globalClientCode) {
                this.clientCode = globalClientCode.value;
                this.pageTitle = this.labels.COMPLETED_CHECKLISTS + ': ' + globalClientCode.value + ' - ' + globalClientCode.label;
                this.showCompletedChecklists = false;
                this._checklistService.getClientChecklistTypeData(this.clientCode).subscribe((data) => {
                    if (data != null) {
                        let weekDate = data.filter(x => x.checklistType == this.labels.WEEK)[0].fromDate;
                        let monthDate = data.filter(x => x.checklistType == this.labels.MONTH)[0].fromDate;
                        this.weeklyEffectiveDate = moment(weekDate);
                        this.monthlyEffectiveDate = moment(monthDate);
                        if (this.isWeekly) {
                            this.getWeeklyChecklistsByDefault();
                            this.datePickerSettings();
                        } else {
                            this.getMonthlyChecklistsByDefault();
                        }
                        this.monthPickerSettings();
                    } else {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: this.sharedLabels.ERROR_GET_DETAILS });
                    }
                });
            }
        });
        this.getScreenActions();
    }

    ngOnDestroy() {
        if (this.clientCodeSubscriber) {
            this.clientCodeSubscriber.unsubscribe();
        }
    }
    /*------ end region life cycle hooks ------*/

    /*------ region Public methods ------*/

    // Settings of Month picker in Monthly mode
    monthPickerSettings() {
        let today = new Date();
        let todayMonth = today.toLocaleString('en-us', { month: 'short' });
        let todayYear = today.getFullYear();
        this.datePickerConfig.max = this.fromMonth = this.toMonth = todayMonth + ', ' + todayYear;
        let minDate = new Date(this.monthlyEffectiveDate);
        let minDateMonth = minDate.toLocaleString('en-us', { month: 'short' });
        this.datePickerConfig.min = minDateMonth + ', ' + minDate.getFullYear();
    }

    // Settings of Date picker in Weekly mode
    datePickerSettings() {
        this.daterangepickerOptions.settings = {
            ranges: {
                'This week': [moment().day(1), moment().day(7)],
                'Last week': [moment().weekday(-6), moment().weekday(-0)],
                'Last 4 weeks': [moment().subtract(4, 'weeks').startOf('isoWeek'), moment().weekday(-0)],
                'Last 8 weeks': [moment().subtract(8, 'weeks').startOf('isoWeek'), moment().weekday(-0)]
            },
            locale: {
                firstDay: 1 // Monday
            },
            maxDate: moment().day(7),
            startDate: this.defaultFromDate,
            endDate: this.defaultToDate,
            minDate: this.checklistFirstEffectiveDate
        };
    }

    // Triggers after range selection
    public selectedDate(value: any, datepicker?: any) {
        // any object can be passed to the selected event and it will be passed back here
        datepicker.start = value.start;
        datepicker.end = value.end;
        this.fromWeekDate = value.start;
        this.toWeekDate = value.end;
        this.constructChecklists(value.start, value.end);
    }

    // Construct Monthly/Weekly Checklists
    constructChecklists(fromDate, toDate) {
        this.checklistTitle = this.isWeekly ? this.labels.WEEKLY_CHECKLISTS : this.labels.MONTHLY_CHECKLISTS;
        this.getChecklistData(fromDate, toDate, false);
    }

    // Initialize Weekly Checklists mode
    initWeeklyChecklist() {
        if (this.showCompletedChecklists) {
            this.showCompletedChecklists = false;
        }
        this.showErrorMessage = false;
        this.checklistTitle = this.labels.WEEKLY_CHECKLISTS;
        this.isWeekly = true;
        this.toggleChecklistMode(this.isWeekly);
        this.getWeeklyChecklistsByDefault();
    }

    // Initialize Monthly Checklists mode
    initMonthlyChecklist() {
        this.showErrorMessage = false;
        this.initMonthPicker = true;
        this.toMonthChanged = this.fromMonthChanged = false;
        this.fromMonthLoadCount = this.toMonthLoadCount = 0;
        if (this.showCompletedChecklists) {
            this.showCompletedChecklists = false;
        }
        this.checklistTitle = this.labels.MONTHLY_CHECKLISTS;
        this.isWeekly = false;
        this.toggleChecklistMode(this.isWeekly);
        this.getMonthlyChecklistsByDefault();
    }

    // Weekly Checklists by default
    getWeeklyChecklistsByDefault() {
        this.checklistFirstEffectiveDate = this.weeklyEffectiveDate;
        this.defaultToDate = moment().day(7); // This week's coming Sunday
        this.defaultFromDate = moment().subtract(7, 'weeks').startOf('isoWeek'); // Last 8th week monday date
        this.applyStartEndDatesAndGetChecklists();
    }

    // Monthly Checklists by default
    getMonthlyChecklistsByDefault() {
        this.checklistFirstEffectiveDate = this.monthlyEffectiveDate;
        this.defaultToDate = moment().startOf('month');  // Current Month date
        this.defaultFromDate = moment().subtract(7, 'months').startOf('month'); // Last 8th Month Date
        this.applyStartEndDatesAndGetChecklists();
    }

    // Get Checklists with default date ranges
    applyStartEndDatesAndGetChecklists() {
        let checklistType = this.isWeekly ? this.labels.WEEKLY : this.labels.MONTHLY;
        if (this.defaultFromDate < this.checklistFirstEffectiveDate) {
            this.defaultFromDate = this.checklistFirstEffectiveDate;
            if (this.defaultFromDate > this.defaultToDate) {
                let date = this.checklistFirstEffectiveDate.clone();
                date = this.isWeekly ? date.format(this.labels.DATE_FORMAT) : date.format(this.labels.MONTH_FORMAT);
                this.noChecklistMessage = this.labels.NO_CHECKLISTS_TO_DISPLAY.replace('CCCC', checklistType).replace('DDDD', date);
                this.defaultFromDate = this.defaultToDate;
                this.showCompletedChecklists = false;
                return;
            }
        }
        if (!this.isWeekly) {
            this.fromMonth = this.defaultFromDate.clone().format(this.labels.MONTH_FORMAT);
            this.toMonth = this.defaultToDate.clone().format(this.labels.MONTH_FORMAT);
        } else {
            this.fromWeekDate = this.defaultFromDate.clone().format(this.labels.DATE_FORMAT);
            this.toWeekDate = this.defaultToDate.clone().format(this.labels.DATE_FORMAT);
        }
        this.constructChecklists(this.defaultFromDate, this.defaultToDate);
    }

    // Toggle the weekly, monthly buttons
    toggleChecklistMode(isWeekly) {
        let activeId = isWeekly ? '#weekly' : '#monthly';
        let defaultId = !isWeekly ? '#weekly' : '#monthly';
        this.addRemoveClassesOfChecklistTypeButtons(activeId, defaultId);
    }

    // Add/Remove active css class
    addRemoveClassesOfChecklistTypeButtons(activeId, defaultId) {
        let activeClass = 'btn-primary active checkListBtn';
        let defaultClass = 'btn-default';
        jQuery(defaultId).removeClass(activeClass).addClass(defaultClass);
        jQuery(activeId).removeClass(defaultClass).addClass(activeClass);
    }

    // Get Checklist data
    getChecklistData(fromDate, toDate, isReopen) {
        let request = new ChecklistDataRequestViewModel();
        request.ClientCode = this.clientCode;
        request.FromDate = fromDate.format(this.labels.DATE_FORMAT);
        request.ToDate = toDate.format(this.labels.DATE_FORMAT);
        if (this.isWeekly) {
            request.ChecklistType = this.labels.WEEK;
            let noOfWeeks = toDate.diff(fromDate, 'weeks') + 1;
            if (noOfWeeks > this.labels.MAX_DATE_RANGE) {
                this.showErrorMessage = true;
                this.errorMessage = this.labels.MESSAGE.WEEK_RANGE_EXCEED;
                return;
            }
            this.getMondaysAndFrameAsColumns(fromDate, toDate);
        } else {
            request.ChecklistType = this.labels.MONTH;
            let noOfMonths = toDate.diff(fromDate, 'months') + 1;
            if (noOfMonths > this.labels.MAX_DATE_RANGE) {
                this.showErrorMessage = true;
                this.errorMessage = this.labels.MESSAGE.MONTH_RANGE_EXCEED;
                return;
            }
            this.getMonthStartDatesAndFrameAsColumns(fromDate, toDate);
        }
        this.getCompletedChecklists(request, isReopen);

    }

    // Get Mondays between the dates selected in Weekly checklist mode and make as column headers
    getMondaysAndFrameAsColumns(fromDate, toDate) {
        this.columns = [];
        let sunday = 0;
        let day = 1; // Monday
        let current = fromDate.clone();
        if (current.day() == day) {
            this.columns.push({ header: fromDate.clone().format(this.labels.DATE_FORMAT), field: fromDate.clone().format(this.labels.DATE_FORMAT) });
        }
        if (current.day() == sunday) {
            current = fromDate.clone().subtract(1, 'weeks');
        }
        while (current.day(7 + day).isBefore(toDate)) {
            this.columns.push({ header: current.clone().format(this.labels.DATE_FORMAT), field: current.clone().format(this.labels.DATE_FORMAT) });
        }
        this.columns = this.columns.reverse();
    }

    // Get Month start date between the dates selected in Monthly checklist mode and make as column headers
    getMonthStartDatesAndFrameAsColumns(fromDate, toDate) {
        this.columns = [];
        let dateStart = fromDate.clone();
        let dateEnd = toDate.clone();
        while (dateEnd > dateStart || dateStart.format('M') === dateEnd.format('M')) {
            this.columns.push({ header: dateStart.format(this.labels.MONTH_FORMAT), field: dateStart.format(this.labels.DATE_FORMAT) });
            dateStart.add(1, 'month');
        }
        this.columns = this.columns.reverse();
    }

    // Get completed checklists of selected date range
    getCompletedChecklists(request, isReopen) {
        this.showErrorMessage = false;
        this._checklistService.getCompletedChecklistsData(request).subscribe((data) => {
            if (data) {
                if (data != null) {
                    this.checklists = data;
                    this.assignChecklistStatusToColumns(this.checklists[0]);
                    this.updateRowGroupMetaData();
                    if (this.isWeekly) {
                        this.picker.datePicker.setStartDate(this.fromWeekDate);
                        this.picker.datePicker.setEndDate(this.toWeekDate);
                    }
                    this.showCompletedChecklists = true;
                    if (isReopen) {
                        this.displayDialog = false;
                        this.msgs = [];
                        this.msgs.push({ severity: 'success', summary: 'Checklist dated ' + this.checklistRequest.FromDate, detail: ' re-opened successfully' });
                    }
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: 'error', summary: 'Error Occurred' });
                }
            }
        });
    }

    // Assign Pending/Completed status to checklist dates
    assignChecklistStatusToColumns(checklist) {
        if (!checklist) {
            checklist = {};
        }
        let _self = this;
        this.columns.forEach(function (element) {
            element.checklistStatus = checklist.hasOwnProperty(element.field) ? _self.labels.CHECKLIST_STATUS.COMPLETED : _self.labels.CHECKLIST_STATUS.PENDING;
        });
    }

    // Implements row grouping by ChecklistName
    updateRowGroupMetaData() {
        // rowGroupMetadata object is created to represent how many rows a checklist should span along with the rowIndex of the group
        this.rowGroupMetadata = {};
        if (this.checklists) {
            for (let i = 0; i < this.checklists.length; i++) {
                let rowData = this.checklists[i];
                let checklistName = rowData.ChecklistName;
                if (i == 0) {
                    this.rowGroupMetadata[checklistName] = { index: 0, size: 1 };
                } else {
                    let previousRowData = this.checklists[i - 1];
                    let previousRowGroup = previousRowData.ChecklistName;
                    if (checklistName === previousRowGroup) {
                        this.rowGroupMetadata[checklistName].size++;
                    } else {
                        this.rowGroupMetadata[checklistName] = { index: i, size: 1 };
                    }
                }
            }
        }
    }

    // On click of open button in checklist date column
    openChecklist(checklistDate) {
        this.checklistRequest = new ChecklistDataRequestViewModel();
        this.checklistRequest.ChecklistType = this.isWeekly ? this.labels.WEEK : this.labels.MONTH;
        this.checklistRequest.ClientCode = this.clientCode;
        this.checklistRequest.FromDate = checklistDate;
        let checklistTypeName = this.isWeekly ? this.labels.WEEKLY : this.labels.MONTHLY;
        this.dialogMessage = this.labels.OPEN_CHECKLIST_CONFIRM.replace('XXXX', checklistTypeName).replace('DDDD', checklistDate);
        this.displayDialog = true;
    }

    // Open completed checklist
    openCompletedChecklist() {
        this._checklistService.openCompletedChecklist(this.checklistRequest).subscribe((resp) => {
            if (resp) {
                this.validationResponse = resp;
                if (this.validationResponse && this.validationResponse.success) {
                    let startDate = moment();
                    let endDate = moment();
                    if (this.isWeekly) {
                        startDate = moment(this.fromWeekDate);
                        endDate = moment(this.toWeekDate);
                    } else {
                        startDate = moment(this.fromMonth);
                        endDate = moment(this.toMonth);
                    }
                    this.getChecklistData(startDate, endDate, true);
                }
            }
        });
    }

    // Date change event of FromDate in Monthly checklist mode
    onFromDateChange() {
        this.fromMonthLoadCount += 1;
        if (this.fromMonth && !this.initMonthPicker) {
            this.initMonthPicker = false;
            this.fromMonthDate = moment(this.fromMonth);
            if (this.toMonth) {
                this.toMonthDate = moment(this.toMonth);
                if (this.fromMonthDate > this.toMonthDate) {
                    this.showErrorMessage = true;
                    this.errorMessage = this.labels.MESSAGE.START_DATE_GREATER_THAN_END_DATE;
                } else if (this.fromMonthDate > moment()) {
                    this.showErrorMessage = false;
                    this.errorMessage = this.labels.MESSAGE.FUTURE_DATE_NOT_VALID;
                } else {
                    this.getChecklistData(this.fromMonthDate, this.toMonthDate, false);
                }
            }
        } else {
            if (this.fromMonthLoadCount == 2) {
                this.fromMonthChanged = true;
                if (this.fromMonthChanged && this.toMonthChanged) {
                    this.initMonthPicker = false;
                }
            }
        }

    }

    // Date change event of ToDate in Monthly checklist mode
    onEndDateChange() {
        this.toMonthLoadCount += 1;
        if (this.toMonth && !this.initMonthPicker) {
            this.initMonthPicker = false;
            this.toMonthDate = moment(this.toMonth);
            if (this.fromMonth) {
                this.fromMonthDate = moment(this.fromMonth);
                if (this.toMonthDate < this.fromMonthDate) {
                    this.showErrorMessage = true;
                    this.errorMessage = this.labels.MESSAGE.END_DATE_LESS_THAN_START_DATE;
                } else if (this.toMonthDate > moment()) {
                    this.showErrorMessage = true;
                    this.errorMessage = this.labels.MESSAGE.FUTURE_DATE_NOT_VALID;
                } else {
                    this.showErrorMessage = false;
                    this.getChecklistData(this.fromMonthDate, this.toMonthDate, false);
                }
            }
        } else {
            if (this.toMonthLoadCount == 2) {
                this.toMonthChanged = true;
                if (this.fromMonthChanged && this.toMonthChanged) {
                    this.initMonthPicker = false;
                }
            }
        }
    }

    // Get Edit Actions of Compledted Checklists page page
    getScreenActions() {
        this._userService.getUserScreenActions(SCREEN_CODE.COMPLETEDCHECKLISTS)
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

    closeDialog() {
        this.checklistRequest = new ChecklistDataRequestViewModel();
        this.displayDialog = false;
    }
    /*------ end region Public methods ------*/
}
