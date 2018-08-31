import { HttpUtility } from '../../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { KPIViewModel } from './kpi.model';
import { ClientKPISetupViewModel } from '../../../client/kpi-setup/kpi-setup.model';

@Injectable()
export class KPIService {
    constructor(
        private httpUtility: HttpUtility) {
    }
    /*
     * Getting kpiQuestions based on checkListType
     * */
    public getKPIQuestionsBasedOnSource(checkListTypeCode: string) {
        return this.httpUtility.get('KPI/GetKPIQuestionBasedOnCheckListType?checkListTypeCode=' + checkListTypeCode);
    }

    /*
     * Saving KPI details
     * */
    public saveKPIDetails(KPIDetails: KPIViewModel) {
        return this.httpUtility.post('KPI/SaveKPIDetails', KPIDetails);
    }

    /*
     * Getting checkListTypes
     * */
    public getChecklistTypes() {
        return this.httpUtility.get('KPI/GetCheckListTypes');
    }

    /*
     * Getting Measure based on checkLIstTypes
     * */
    public getMeasureBasedOnClientTypeID(checkListTypeId: number) {
        return this.httpUtility.get('KPI/GetMeasureBasedOnClientTypeID?checkListTypeId=' + checkListTypeId);
    }

    /*
     * Getting All KPIs
     * */
    public getAllKPIs() {
        return this.httpUtility.get('KPI/GetAllKPIs');
    }

    /*
     * Getting All M3Metrics KPIs
     * */
    public getAllM3MetricsKPIs() {
        return this.httpUtility.get('KPI/GetAllM3MetricsKPIs');
    }

    /*
     * Getting all KPIs based on ID
     * */
    public getKPIById(kpiId) {
        return this.httpUtility.get('KPI/GetKPIById?kpiID=' + kpiId);
    }

    /**
     * To get KPIs which can be assigned for the client
     * @param clientCode
     */
    public getKPIQuestionsForClient(clientCode) {
        return this.httpUtility.get('KPI/GetKPIQuestionsForClient?clientCode=' + clientCode);
    }

    /**
     * To save Client KPI
     * @param clientKPI
     */
    public saveClientKPIDetails(clientKPI: ClientKPISetupViewModel) {
        return this.httpUtility.post('KPI/SaveKPIForClient', clientKPI);
    }


    public getAssignedKPIsForClient(clientCode) {
        return this.httpUtility.get('KPI/GetAssignedKPIForClient?clientCode=' + clientCode);
    }

    /**
     * To get the kpi heat map items.
     */
    public getKpiHeatMapItems() {
        return this.httpUtility.get('KPI/GetKpiHeatMapItems');
    }


}
