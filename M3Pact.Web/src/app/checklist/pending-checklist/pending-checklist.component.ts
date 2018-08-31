import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { ClientChecklistResponse } from '../models/client-checklistresponse.model';
import { PENDING_CHECKLIST } from '../../shared/utilities/resources/labels';
import { GlobalEventsManager } from '../../shared/utilities/global-events-manager';
import { ChecklistService } from '../checklist.service';
import { DatePipe } from '@angular/common';
import { getLocaleMonthNames } from '@angular/common/src/i18n/locale_data_api';
import { monthsShort } from 'moment';
import { Element } from '@angular/compiler';
import { Router } from '@angular/router';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { Message } from 'primeng/components/common/api';
import { Subscription } from 'rxjs';
import * as moment from 'moment';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'app-pending-checklist',
    templateUrl: './pending-checklist.component.html',
    styleUrls: ['./pending-checklist.component.css'],
    providers: [ChecklistService, DatePipe]
})
export class PendingChecklistComponent implements OnInit, OnDestroy {

    public clientChecklistResponse: ClientChecklistResponse;
    public monthChecklistResponse: ClientChecklistResponse;
    public clientChecklistResponses = new Array<ClientChecklistResponse>();
    public monthlyChecklistResponses = new Array<ClientChecklistResponse>();
    public pendingWeeklyChecklists: any[] = [];
    public pendingMonthlyChecklists: any[] = [];
    public selectedDate: string;
    public selectedMonth: string;
    public selectedMonthLabel: string;
    public checklistResponseNotSelected: boolean[] = [];
    public checklistFreeFormNotFilled: boolean[] = [];
    public monthlyChecklistResponseNotSelected: boolean[] = [];
    public monthlyChecklistFreeFormNotFilled: boolean[] = [];
    public labels = PENDING_CHECKLIST;
    public checklistType: string;
    public clientCode: any;
    public weelkyChecklistName: string;
    public monthlyChecklistName: string;
    public showWeekly = true;
    public showMonthly = false;
    public noWeeklyPendingChecklist: boolean;
    public noMonthlyPendingChecklist: boolean;
    public validationConstants = VALIDATION_MESSAGES;
    public msgs: Message[] = [];
    public pageTitle: string;
    public today: string;
    public weeklyChecklistEffective: string;
    public monthlyChecklistEffective: string;
    /*------ region private properties ------*/
    private clientCodeSubscriber: Subscription;
    /*------ end region private properties ------*/

    /*------ region constructor ------*/
    constructor(private _globalEvents: GlobalEventsManager, private _checklistService: ChecklistService, private datePipe: DatePipe,
        private ref: ChangeDetectorRef, private router: Router
    ) {
        this._globalEvents.setClientDropdown(true);
        this.today = moment().format('MM/DD/YYYY');
    }
    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit() {
        this.clientCodeSubscriber = this._globalEvents.getGlobalClientCode.subscribe(
            (globalClientCode) => {
                this.clientCode = globalClientCode;
                this.pageTitle = this.labels.TITLE + ': ' + globalClientCode.value + ' - ' + globalClientCode.label;
                this.pendingMonthlyChecklists = [];
                this.pendingWeeklyChecklists = [];
                this.clientChecklistResponses = new Array<ClientChecklistResponse>();
                this.monthlyChecklistResponses = new Array<ClientChecklistResponse>();
                if (this.clientCode != null) {
                    this.getWeeklyPendingChecklist(this.clientCode.value);
                    this.getMonthlyPendingChecklist(this.clientCode.value);
                }
            },
            (err) => { }
        );
        this._globalEvents.showMonthlyChecklist.subscribe(
            (resp) => {
                this.showMonthly = resp;
                this.showWeekly = !resp;
            }
        )
    }

    ngOnDestroy() {
        if (this.clientCodeSubscriber) {
            this.clientCodeSubscriber.unsubscribe();
        }       
    }
    /*------ end region life cycle hooks ------*/


    /*------ region Public methods ------*/

    canDeactivate(): Observable<boolean> | boolean {
        this._globalEvents.showPendingChecklistMonthly(false);
        return true;
    }

    /*To get the Pending Weekly Checklists  */
    getWeeklyPendingChecklist(clientCode) {
        this.pendingWeeklyChecklists = [];
        this.selectedDate = '';
        this.clientChecklistResponses = new Array<ClientChecklistResponse>();
        this._checklistService.getWeeklyPendingChecklist(clientCode).subscribe(
            (pendingChecklistDates) => {
                if (pendingChecklistDates && pendingChecklistDates.length > 0) {
                    for (let i in pendingChecklistDates) {
                        this.pendingWeeklyChecklists.push({ label: this.datePipe.transform(pendingChecklistDates[i], 'MM/dd/y'), value: this.datePipe.transform(pendingChecklistDates[i], 'MM/dd/y') });
                    }
                    // this.pendingWeeklyChecklists = this.pendingWeeklyChecklists.slice(0).reverse();
                    this.selectedDate = moment(this.pendingWeeklyChecklists[0].label).format('MM/DD/YYYY');

                    this.noWeeklyPendingChecklist = false;
                    if (this.selectedDate && this.selectedDate <= this.today) {
                        this.getPendingChecklistQuestions(clientCode, this.selectedDate, 'WEEK');
                    } else {
                        this.weeklyChecklistEffective = this.labels.CHECKLIST_EFFECTIVE.replace('DDDD', this.selectedDate);
                    }
                }
            },
            (err) => { }
        );
    }

    /* To get the Pending Monthly Checklists  */
    getMonthlyPendingChecklist(clientCode) {
        this.pendingMonthlyChecklists = [];
        this.selectedMonth = '';
        this.monthlyChecklistResponses = new Array<ClientChecklistResponse>();
        this._checklistService.getMonthlyPendingChecklist(clientCode).subscribe(
            (pendingChecklistDates) => {
                if (pendingChecklistDates && pendingChecklistDates.length > 0) {
                    let months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
                    for (let i in pendingChecklistDates) {
                        let month = months[new Date(pendingChecklistDates[i]).getMonth()];
                        let year = new Date(pendingChecklistDates[i]).getFullYear().toString();
                        this.pendingMonthlyChecklists.push({ label: month + ', ' + year, value: month + ', ' + year });
                    }
                    // this.pendingMonthlyChecklists = this.pendingMonthlyChecklists.slice(0).reverse();
                    this.selectedMonthLabel = this.pendingMonthlyChecklists[0].label;
                    this.selectedMonth = moment(this.pendingMonthlyChecklists[0].label).format('MM/DD/YYYY');
                    if (this.selectedMonth && this.selectedMonth <= this.today) {
                        this.getPendingChecklistQuestions(clientCode, this.selectedMonth, 'MONTH');
                    } else {
                        this.monthlyChecklistEffective = this.labels.MONTHLY_CHECKLIST_EFFECTIVE.replace('DDDD', this.selectedMonth);
                    }
                }
            },
            (err) => { }
        );
    }

    /**
     * To get the pending checklist questions
     * @param clientcode
     * @param pendingDate
     * @param checklistType
     */
    getPendingChecklistQuestions(clientcode: string, pendingDate: any, checklistType: string) {
        if (checklistType === 'WEEK') {
            this.clientChecklistResponses = new Array<ClientChecklistResponse>();
        } else {
            this.monthlyChecklistResponses = new Array<ClientChecklistResponse>();
        }
        this._checklistService.getPendingChecklistQuestions(clientcode, pendingDate, checklistType).subscribe(
            (questions) => {
                if (questions != null && questions.length > 0) {
                    for (let i in questions) {
                        this.clientChecklistResponse = new ClientChecklistResponse();
                        this.clientChecklistResponse.ActualFreeForm = questions[i].actualFreeForm;
                        this.clientChecklistResponse.ActualResponse = questions[i].actualResponse;
                        this.clientChecklistResponse.QuestionText = questions[i].questionText;
                        this.clientChecklistResponse.RequireFreeform = questions[i].requireFreeform;
                        this.clientChecklistResponse.CheckListAttributeMapID = questions[i].checkListAttributeMapID;
                        this.clientChecklistResponse.ClientCheckListMapID = questions[i].clientCheckListMapID;
                        this.clientChecklistResponse.ChecklistName = questions[i].checklistName;
                        this.clientChecklistResponse.ExpectedRespone = questions[i].expectedRespone;
                        this.clientChecklistResponse.IsKPI = questions[i].isKPI;
                        this.clientChecklistResponse.Questionid = questions[i].questionid;
                        this.clientChecklistResponse.QuestionCode = questions[i].questionCode;
                        if (checklistType === 'WEEK') {
                            this.clientChecklistResponses.push(this.clientChecklistResponse);
                            this.weelkyChecklistName = this.clientChecklistResponses[0].ChecklistName;
                        } else {
                            this.monthlyChecklistResponses.push(this.clientChecklistResponse);
                            this.monthlyChecklistName = this.monthlyChecklistResponses[0].ChecklistName;
                        }
                    }
                } 
           },
            (err) => { }
        );
    }

    /* method to submit the checklist responses */
    submitChecklistResponse(checklistType: string) {
        this.checklistType = checklistType;
        if (this.validateChecklistResponses()) {
            if (checklistType === 'weekly') {
                this.saveOrSubmitWeeklyChecklistResponse(true);
            } else {
                this.saveOrSubmitMonthlyChecklistResponse(true);
            }
        }
    }

    /* To validate the checklist responses */
    validateChecklistResponses() {
        let checklistResponses = [];
        if (this.checklistType === 'weekly') {
            this.checklistFreeFormNotFilled.length = this.clientChecklistResponses.length;
            this.checklistResponseNotSelected.length = this.clientChecklistResponses.length;
            checklistResponses = this.clientChecklistResponses;
        }
        if (this.checklistType === 'monthly') {
            this.checklistFreeFormNotFilled.length = this.monthlyChecklistResponses.length;
            this.checklistResponseNotSelected.length = this.monthlyChecklistResponses.length;
            checklistResponses = this.monthlyChecklistResponses;
        }

        this.checklistFreeFormNotFilled.fill(false);
        this.checklistResponseNotSelected.fill(false);
        let canSubmitResponse = true;
        for (let i in checklistResponses) {
            if (checklistResponses[i].ActualFreeForm.trim() === '' && checklistResponses[i].RequireFreeform == true) {
                this.checklistFreeFormNotFilled[i] = true;
                canSubmitResponse = false;
            }
            if (checklistResponses[i].ActualResponse === null) {
                this.checklistResponseNotSelected[i] = true;
                canSubmitResponse = false;
            }
        }
        return canSubmitResponse;
    }


    /**
    * Method used to convert month to month first date
    * @param isSubmit
    */
    convertMonthtoDate() {
        let commaIndex = this.selectedMonth.indexOf(',');
        let monthName = this.selectedMonth.substring(0, commaIndex);
        let year = + this.selectedMonth.substring(commaIndex + 2);
        let months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        let month = months.findIndex(function (element) {
            return element == monthName;
        });
        let date = commaIndex != -1 ? new Date(year, month) : this.selectedMonth;
        return this.datePipe.transform(date, 'MM/dd/y');
    }

    /**
     * To Save or Submit the Cheklist responses weekly
     * @param isSubmit
     */
    saveOrSubmitWeeklyChecklistResponse(isSubmit: boolean) {
        this._checklistService.saveOrSubmitChecklistResponse(this.clientCode.value, this.selectedDate, isSubmit, this.clientChecklistResponses).subscribe(
            (data) => {
                if (data === true) {
                    if (isSubmit) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'success', summary: '', detail: 'Submitted Checklist Successfully' });
                        this.getWeeklyPendingChecklist(this.clientCode.value);
                    } else {
                        this.msgs = [];
                        this.msgs.push({ severity: 'success', summary: '', detail: 'Saved Checklist Successfully' });
                    }
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: 'error', summary: '', detail: 'Error occured' });
                }
            },
            (err) => { }
        );
    }

    /**
 * To Save or Submit the Cheklist responses monthly
 * @param isSubmit
 */
    saveOrSubmitMonthlyChecklistResponse(isSubmit: boolean) {
        this._checklistService.saveOrSubmitChecklistResponse(this.clientCode.value, this.convertMonthtoDate(), isSubmit, this.monthlyChecklistResponses).subscribe(
            (data) => {
                if (data === true) {
                    if (isSubmit) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'success', summary: '', detail: 'Submitted Checklist Successfully' });
                        this.getMonthlyPendingChecklist(this.clientCode.value);
                    } else {
                        this.msgs = [];
                        this.msgs.push({ severity: 'success', summary: '', detail: 'Saved Checklist Successfully' });
                    }
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: 'error', summary: '', detail: 'Error occured' });
                }
            },
            (err) => { }
        );
    }

    switchToWeekly(element: any) {
        if (!this.showWeekly) {
            this.showWeekly = true;
            this.showMonthly = false;

        }
    }

    switchToMonthly(element: any) {
        if (!this.showMonthly) {
            this.showWeekly = false;
            this.showMonthly = true;
        }
    }

    onRadioButtonSelect(index: number, type: string) {
        if (type === 'weekly') {
            jQuery('#WeeklyFreeForm' + index).focus();
        } else {
            jQuery('#MonthlyFreeForm' + index).focus();
        }
        this.checklistResponseNotSelected[index] = false;
    }

    onKeyUp(index: number) {
        this.checklistFreeFormNotFilled[index] = false;
    }

    /*------ end region Public methods ------*/
}
