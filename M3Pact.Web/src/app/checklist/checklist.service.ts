import { Injectable } from '@angular/core';
import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { ClientChecklistResponse } from './models/client-checklistresponse.model';
import { SubmitChecklistResponse } from './models/submit-checklistresponse.model';
import { List } from 'lodash';

@Injectable()
export class ChecklistService {
    submitResponse: SubmitChecklistResponse;
    constructor(private httpUtility: HttpUtility) { }
    /**
     * Making API call to get the weekly pending checklist dates
     * @param clientCode
     */
    getWeeklyPendingChecklist(clientCode: string) {
      return  this.httpUtility.get('Checklist/GetWeeklyPendingChecklist?clientCode=' + clientCode);
    }

     /**
     * Making API call to get the monthly pending checklist dates
     * @param clientCode
     */
    getMonthlyPendingChecklist(clientCode: string) {
        return this.httpUtility.get('Checklist/GetMonthlyPendingChecklist?clientCode=' + clientCode);
    }

     /**
      * Making API call to get the pending checklist questions
      * @param clientCode
      * @param pendingDate
      * @param checklistType
      */
    getPendingChecklistQuestions(clientCode: string, pendingDate: Date, checklistType: string) {
        return this.httpUtility.get('Checklist/GetPendingChecklistQuestions?clientCode=' + clientCode + '&pendingChecklistDate=' + pendingDate + '&checklistType=' + checklistType);
    }

    /**
     * Making API call to save or submit the response
     * @param clientCode
     * @param pendingDate
     * @param isSubmit
     * @param clientChecklistResponse
     */
    saveOrSubmitChecklistResponse(clientCode: string, pendingDate: any, isSubmit: boolean, clientChecklistResponse: Array<ClientChecklistResponse>) {
        this.submitResponse = new SubmitChecklistResponse();
        this.submitResponse.clientCode = clientCode;
        this.submitResponse.isSubmit = isSubmit;
        this.submitResponse.pendingDate = pendingDate;
        this.submitResponse.clientChecklistResponse = clientChecklistResponse;

        return this.httpUtility.post('Checklist/SaveOrSubmitChecklistResponse', this.submitResponse);
    }

    // Get Completed Checklists for a given date range
    getCompletedChecklistsData(checklistDataRequest) {
        return this.httpUtility.post('Checklist/GetCompletedChecklistsForADateRange', checklistDataRequest);
    }

    // Open the completed checklist i.e., Make checklist as pending
    openCompletedChecklist(checklistDataRequest) {
        return this.httpUtility.post('Checklist/OpenSelectedChecklist', checklistDataRequest);
    }

    // Get Client checklist type data i.e., Effective weekly and monthly dates
    getClientChecklistTypeData(clientCode: string) {
        return this.httpUtility.get('Checklist/GetClientChecklistTypeData?clientCode=' + clientCode);
    }
}
