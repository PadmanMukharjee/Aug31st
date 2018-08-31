// Angular imports
import { Component, OnInit, ViewEncapsulation, ViewChild, ChangeDetectorRef, Inject, OnDestroy, ElementRef } from '@angular/core';
import { Router } from '@angular/router';

// Common imports
import { GlobalEventsManager } from '../../shared/utilities/global-events-manager';
import { CLIENT_HISTORY, SHARED } from '../../shared/utilities/resources/labels';
import { ClientHistoryService } from './client-history.service';
import { ClientHistoryViewModel } from './client-history.model';

// Third party Imports
import { Message } from 'primeng/components/common/api';
import { Table } from 'primeng/components/table/table';
import { Subscription } from 'rxjs';
import { DaterangepickerConfig, DaterangePickerComponent } from 'ng2-daterangepicker';
import * as moment from 'moment';
import { DatePickerDirective } from 'ng2-date-picker';
import { DpDatePickerModule } from 'ng2-date-picker';
import { DatePickerComponent } from 'ng2-date-picker';
import * as _ from 'lodash';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
    selector: 'client-history',
    templateUrl: './client-history.component.html',
    styleUrls: ['./client-history.component.css']
})
export class ClientHistoryComponent implements OnInit {

    /*----- region public properties -------*/

    public daterange: any = {};
    public client: any;
    public clientCode: string;
    public clientName: string;
    public labels = CLIENT_HISTORY;
    public clientHistoryData: ClientHistoryViewModel[];
    public startDate: any;
    public endDate: any;
    public defaultFromDate: any;
    public defaultToDate: any;
    public msgs: Message[] = [];
    public clientCreatedDate: any = '';
    public clientCreatedBy: string = '';
    public columnPropertyOptions: any;
    public data: any = [];
    public minDate: any;
    public modifiedOrAddedBy: string;
    public action: string;
    public oldValue: string;
    public newValue: string;
    public selectedMSelectValue: any;
    public filteredClientHistoryData: any[];
    public rowsIn: number = 15;
    public paginationCount: any;

    /*------------end region public properties --------------*/


    /*----- region private properties -------*/

    private clientCodeSubscriber: Subscription;
    @ViewChild(DaterangePickerComponent)
    private picker: DaterangePickerComponent;
    @ViewChild('dt') dataTable: Table;

    /*----- end region private properties -------*/

    /*------ region constructor ------*/
    constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService , private _globalEventsManager: GlobalEventsManager, private daterangepickerOptions: DaterangepickerConfig,
        private _clientHistoryService: ClientHistoryService) {
        this._globalEventsManager.setClientDropdown(false);
    }
    /*------ end region constructor ------*/

    /*------ region life cycle events -------------*/

    ngOnInit() {
        this.columnPropertyOptions = [];
        this.defaultFromDate = moment().subtract(2, 'months').format(this.labels.DATE_FORMAT);
        this.defaultToDate = moment().format(this.labels.DATE_FORMAT);
        this.getClientCodeAndNameFromLocalStorage();
    }

    ngOnDestroy() {
        localStorage.removeItem(this.labels.CLIENT_CODE);
        localStorage.removeItem(this.labels.CLIENT_NAME);
        localStorage.clear();
    }
    
    /*------end region life cycle events -------------*/

    /*------------ region public methods ----------*/

    /**
 * Called when pagechange event occurs
 */
    onTablePageChange(e) {
        let start = this.filteredClientHistoryData.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.filteredClientHistoryData.length ? this.filteredClientHistoryData.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.filteredClientHistoryData.length;
    }

    /**
   * get client code and client name from local storage.
   */
    getClientCodeAndNameFromLocalStorage() {
        this.data[this.labels.CLIENT_CODE] = this.storage.get(this.labels.CLIENT_CODE);
        this.clientCode = this.data[this.labels.CLIENT_CODE];
        this.data[this.labels.CLIENT_NAME] = this.storage.get(this.labels.CLIENT_NAME);
        this.clientName = this.data[this.labels.CLIENT_NAME];
        this.getClientCreationDetails(this.clientCode);
        this.getClientHistory(this.clientCode, this.defaultFromDate, this.defaultToDate);
    }

    /**
     * Date picker settings
     */
    datePickerSettings() {
        this.daterangepickerOptions.settings = {
            ranges: {
                'This week': [moment().day(1), moment().day(7)],
                'Last week': [moment().weekday(-6), moment().weekday(-0)],
                'Last 4 weeks': [moment().subtract(4, 'weeks').startOf('isoWeek'), moment().weekday(-0)],
                'Last 8 weeks': [moment().subtract(8, 'weeks').startOf('isoWeek'), moment().weekday(-0)]
            },
            maxDate: moment(new Date()),
            startDate: moment().subtract(2, 'months').format(this.labels.DATE_FORMAT),
            endDate: this.defaultToDate,
            minDate: moment(this.minDate).format(this.labels.DATE_FORMAT)
          };
    }

    /**
   * Triggers after range selection
   * @param value
   * @param datepicker
   */
    public selectedDate(value: any, datepicker?: any) {
        // any object can be passed to the selected event and it will be passed back here
        this.selectedMSelectValue = [];
        this.startDate = value.start;
        this.endDate = value.end;
        this.getClientHistory(this.clientCode, this.startDate.format(this.labels.DATE_FORMAT), this.endDate.format(this.labels.DATE_FORMAT));
    }
      
    /**
     * To get the cient history.
     * @param clientCode
     * @param startDate
     * @param endDate
     */
    getClientHistory(clientCode, startDate, endDate) {
        this._clientHistoryService.getClientHistory(clientCode, startDate, endDate).subscribe(
            data => {
                this.clientHistoryData = new Array<ClientHistoryViewModel>();
                if (data != null && data.length > 0) {
                    let timeFormat = this.labels.DATE_TIME_FORMAT;
                    this.dataTable.reset();
                    this.modifiedOrAddedBy = '';
                    this.action = '';
                    this.oldValue = '';
                    this.newValue = '';
                    this.clientHistoryData = data;
                    this.constructColumnPropertyFilterValues(data);
                    this.clientHistoryData.forEach(function (history) {
                        let utcDate = moment.utc(history.modifiedOrAddedDate).toDate();
                        history.modifiedOrAddedDate = moment(utcDate).local().format(timeFormat);
                       })
                 
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_SUCCESS, summary: '', detail: SHARED.NO_DATA });
                }
                this.filteredClientHistoryData = [... this.clientHistoryData];
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            },
            err => {
                this.msgs = [];
                this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
            }
        )
    }

    /**
     * Frame filter drop down values for Created/Updated Property Column
     * @param historyData
     */
    constructColumnPropertyFilterValues(historyData) {
        this.columnPropertyOptions = [];
        let columnPropertyValues = _.uniqBy(historyData.map(a => a.updatedOrAddedProperty), function (columnProperty: string) {
            return columnProperty;
        }).sort();
        for (let i = 0; i < columnPropertyValues.length; i++) {
            this.columnPropertyOptions.push({ label: columnPropertyValues[i], value: columnPropertyValues[i] });
        }
    }

    /**
     * To get the client creation details.
     * @param clientCode
     */
    getClientCreationDetails(clientCode) {
        this._clientHistoryService.getClientCreationDetails(clientCode).subscribe(
            data => {
                if (data != null) {
                    if (data.CreatedDate != null) {
                        this.clientCreatedDate = moment(data.CreatedDate).format(this.labels.DATE_FORMAT);
                    } else {
                        this.clientCreatedDate = '';
                    }
                    if (data.CreatedBy != null) {
                        this.clientCreatedBy = data.CreatedBy;
                    } else {
                        this.clientCreatedBy = '';
                    }
                    this.minDate = data.ClientFirstStepCreatedDate;
                    this.datePickerSettings();
                }
            });
    }

    /**
  * Called when filter event occurs
  */
    onTableFilter(event) {
        this.filteredClientHistoryData = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };
        this.onTablePageChange(e);
    }

    /*------------end region public methods ----------*/
}
