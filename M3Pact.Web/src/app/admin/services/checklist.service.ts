import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { CheckListItem } from '../models/checklist-item.model';
import { CheckList } from '../models/checklist.model';

@Injectable()
export class CheckListService {
    constructor(private httpUtility: HttpUtility) {
    }

    /**
     * makes a http service call to save checklistitem resource in both edit and create case
     * @param  {CheckListItem} item
     * @returns Observable
     */
    saveCheckListItem(item: CheckListItem): Observable<CheckListItem> {
        if (item.questionID > 0) {
            return this.httpUtility.post('checklistitems/save', item);
        } else if (item.questionID == 0) {
            return this.httpUtility.post('checklistitems', item);
        }
    }

    /**
     * makes a http service call to get all questions resources
     * @returns Observable
     */
    getCheckListItems(checklistType: string = null): Observable<Array<CheckListItem>> {
        return this.httpUtility.get('checklistitems/' + checklistType);
    }


    /**
     * makes a http service call to get checklist by checklist resource id
     * @param  {number} id
     * @returns Observable
     */
    getCheckList(id: number): Observable<Array<CheckList>> {
        return this.httpUtility.get('checklists/' + id);
    }

    /**
     * makes a http service call to save checklist resource in both edit and create case
     * @param  {CheckList} list
     * @returns Observable
     */
    saveCheckList(list: CheckList): Observable<CheckList> {
        let model = JSON.parse(JSON.stringify(list));
        model.checklistType = { 'key': list.checklistType, value: '' };
        model.sites = list.sites.map(r => { return { 'key': r, 'value': '' }; });
        model.systems = list.systems.map(r => { return { 'key': r, 'value': '' }; });
        model.checklistItems = list.checklistItems.map(r => { return { 'key': r.questionCode, 'value': '' }; });
        if (list.checklistId > 0) {
            return this.httpUtility.post('checklists/save', model);
        } else if (list.checklistId == 0) {
            return this.httpUtility.post('checklists', model);
        }
    }

    /**
     * To get all the checklits to view
     */
    getViewAllChecklists() {
        return this.httpUtility.get('checklists/GetAllChecklists');
    }

    /**
     * To get checklistTypes
     */
    getChecklistTypes() {
        return this.httpUtility.get('checklists/checklisttypes');
    }


    /**
     *  makes a http service call to check if any client exists for a checklist with systemId
     * @param  {number} systemId
     * @param  {number} checklistId
     */
    checkClientOnSystemDelete(systemId: number, checklistId: number) {
        return this.httpUtility.get(`checklists/checkclientdependencyonsystem?checklistId=${checklistId}&systemId=${systemId}`);
    }


    /**
     * makes a http service call to check if any client exists for a checklist with siteId
     * @param  {number} siteId
     * @param  {number} checklistId
     */
    checkClientOnSiteDelete(siteId: number, checklistId: number) {
        return this.httpUtility.get(`checklists/checkclientdependencyonsite?checklistId=${checklistId}&siteId=${siteId}`);
    }

    /**
     * makes a http service call to get monthly checklist
     * @param  {number} systemId
     * @param  {number} siteId
     */
    getMonthlyCheckList(systemId: number, siteId: number) {
        return this.httpUtility.get(`checklists/monthlychecklists?systemId=${systemId}&siteId=${siteId}`);
    }

    /**
     * makes a http service call to get weekly checklist
     * @param  {number} systemId
     * @param  {number} siteId
     */
    getWeeklyCheckList(systemId: number, siteId: number) {
        return this.httpUtility.get(`checklists/weeklychecklists?systemId=${systemId}&siteId=${siteId}`);
    }

    /**
     * To get the checklist heat map items.
     */
    getChecklistHeatMapItems() {
        return this.httpUtility.get('checklistitems/GetChecklistHeatMapQuestions');
    }
}
