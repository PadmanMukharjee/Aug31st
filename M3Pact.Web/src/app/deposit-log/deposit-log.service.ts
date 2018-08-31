import { Headers, RequestOptions } from '@angular/http';
import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { DepositLogClientData } from './models/deposit-log-client-data.model';
import { DepositLogAttributeViewModel } from './models/deposit-log-attribute.model';
import { AuthenticationService } from '../shared/services';


@Injectable()
export class DepositLogService {
    constructor(
        private httpUtility: HttpUtility, private _authService: AuthenticationService) {
    }

    /**
     * To get the deposit log data.
     * @param clientCode
     * @param month
     * @param year
     */
    getDepositLog(clientCode, month, year) {
        return this.httpUtility.get('DepositLog/GetClientDepositLogs?clientCode=' + clientCode + '&month=' + month + '&year=' + year);
    }

    /**
     * To get the payers of a client.
     * @param clientCode
     */
    getPayersForClient(clientCode) {
        return this.httpUtility.get('DepositLog/GetPayersForClient?clientCode=' + clientCode);
    }

    /**
     * To get the deposit log amount for a specific date.
     * @param clientCode
     * @param date
     */
    getDepositLogForClientDate(clientCode, date) {
        return this.httpUtility.get('DepositLog/GetPayersForClientDate?clientCode=' + clientCode + '&depositDate=' + date);
    }

    /**
     * To save the deposit log amount for a client.
     * @param depositeLogData
     */
    saveDepositLogData(depositeLogData: DepositLogClientData) {
        //  let body = JSON.stringify(depositeLogData);
        return this.httpUtility.post('DepositLog/SaveClientDepositLogs', depositeLogData);
    }

    /**
     * To get the deposit log monthly targets.
     * @param clientCode
     * @param month
     * @param year
     */
    getDepositLogMTDData(clientCode, month, year) {
        return this.httpUtility.get('DepositLog/GetDepositLogMTD?clientCode=' + clientCode + '&month=' + month + '&year=' + year);
    }

    /**
     * To get the top 10 payers of the client.
     * @param clientCode
     * @param month
     * @param year
     */
    getPayersChartData(clientCode, month, year) {
        return this.httpUtility.get('Payer/GetTopPayers?clientCode=' + clientCode + '&month=' + month + '&year=' + year);
    }


    /**
     * Gets Business Days of given month and year.
     * @param year
     * @param month
     */
    getTotalBusinessDaysofAMonthInYear(year, month) {
        return this.httpUtility.get('DepositLog/GetTotalBusinessDaysInaMonthOfAnYear?year=' + year + '&month=' + month);
    }

    /**
     * To get the number of deposit days of a client for a given month.
     * @param clientCode
     * @param year
     * @param month
     */
    getNumberOfDepositDaysOfClientForGivenMonth(clientCode , year , month) {
        return this.httpUtility.get('DepositLog/GetNumberOfDepositDaysOfClientForGivenMonth?clientCode=' + clientCode + '&year=' + year + '&month=' + month);
    }

    /**
     * To get the projected cash of simple business days.
     * @param clientCode
     * @param numberOfLastWorkingDays
     * @param month
     * @param year
     * @param savedLastNumberOfBusienssDays
     */
    getProjectedCashFromSimpleBusinessDays(clientCode, numberOfLastWorkingDays, month, year, savedLastNumberOfBusienssDays) {
        return this.httpUtility.get('DepositLog/GetProjectedCashFromSimpleBusinessDays?clientCode=' + clientCode + '&numberOfLastWorkingDays=' + numberOfLastWorkingDays + '&month=' + month + '&year=' + year + '&savedLastNumberOfBusinessDays=' + savedLastNumberOfBusienssDays);
    }

    /**
     * To get the projected cash of previous weeks.
     * @param clientCode
     * @param month
     * @param year
     * @param numberOfWeeks
     */
    GetProjectedCashOfPreviousWeeks(clientCode, month, year, numberOfWeeks) {
        return this.httpUtility.get('DepositLog/GetProjectedCashOfPreviousWeeks?clientCode=' + clientCode + '&month=' + month + '&year=' + year + '&numberOfWeeks=' + numberOfWeeks);
    }

    /**
     * To get the projected cash of last working days.
     * @param clientCode
     * @param month
     * @param year
     * @param numberOfLastWorkingDays
     */
    getProjectedCashOfLastWorkingDays(clientCode, month, year, numberOfLastWorkingDays) {
        return this.httpUtility.get('DepositLog/GetProjectedCashOfLastWorkingDays?clientCode=' + clientCode + '&month=' + month + '&year=' + year + '&numberOfLastWorkingDays=' + numberOfLastWorkingDays);

    }

    /**
     * To get the number of deposit days of client.
     * @param clientCode
     */
    getNumberOfDepositDaysOfClient(clientCode , month , year) {
        return this.httpUtility.get('DepositLog/GetNumberOfDepositDaysOfClient?clientCode=' + clientCode + '&month=' + month + '&year=' + year);
    }

    /**
     * To get the meridian holidays.
     */
    getHolidays() {
        return this.httpUtility.get('DepositLog/GetHolidayDates');
    }

    /**
     * Get All Entered Deposit Dates For Client
     * @param clientCode
     */
    getEnteredDepositDatesForClient(clientCode) {
        return this.httpUtility.get('DepositLog/GetEnteredDepositDatesForClient?clientCode=' + clientCode);
    }

    /**
     * To save the last entered business days or weeks.
     * @param depositLogAttribute
     */
    saveLastEnteredBusinessDaysOrWeeks(depositLogAttribute: DepositLogAttributeViewModel) {
        return this.httpUtility.post('DepositLog/SaveLastEnteredBusinessDaysOrWeeks', depositLogAttribute);
    }

    /**
     * To get the saved last number of business days or weeks.
     * @param clientCode
     */
    getSavedLastNumberOfBusinessDaysOrWeeks(clientCode: string) {
        return this.httpUtility.get('DepositLog/GetSavedLastNumberOfBusinessDaysOrWeeks?clientCode=' + clientCode);
    }

    /**
     * Get Deposit logs of a year for a client
     * @param clientCode
     * @param year
     */
    getClientDepositLogForAYear(clientCode, year) {
        return this.httpUtility.get('DepositLog/GetClientDepositLogForAYear?=' + clientCode + '&year=' + year);
    }

    /**
     * Open or Close month of a year for a client
     * @param clientCode
     * @param monthID
     * @param year
     * @param isCloseMonth
     */
    openOrCloseMonthOfAYear(clientCode, monthID, year, isCloseMonth) {
        return this.httpUtility.post('DepositLog/CloseOrReopenAMonthOfAYearForClient?=' + clientCode + '&year=' + year + '&monthID=' + monthID + '&isCloseMonth=' + isCloseMonth, null);
    }
    
    getClientMinimumDepositYear(clientCode) {
        return this.httpUtility.get('DepositLog/GetMinimumDepositYear?=' + clientCode);
    }

    /**
     * To export deposit Data
     * @param exportDepositViewModel
     */
    exportDepositData(exportDepositViewModel): Observable<Object[]> {
        return Observable.create(observer => {
            let json = JSON.stringify(exportDepositViewModel);
            let xhr = new XMLHttpRequest();
            let token = this._authService.getTokenFromStorage();

            xhr.open('POST', `${this.httpUtility.getApiUrl('DepositLog/OnDepositDataExport')}`,true);
            xhr.setRequestHeader('Content-type', 'application/json');
            xhr.setRequestHeader('Authorization', `Bearer  ${token}`);
            xhr.responseType = 'blob';
            

            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        var contentType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
                        var blob = new Blob([xhr.response], { type: contentType });
                        observer.next(blob);
                        observer.complete();
                    } else {
                        observer.error(xhr.response);
                    }
                }
            }
            xhr.send(json);

        });
    }
}
