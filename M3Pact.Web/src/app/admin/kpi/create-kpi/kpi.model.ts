import { CheckListType } from '../../models/checklist-type.model';
import { MeasureViewModel } from './measure.model';

export class KPIViewModel {
    public kpiid: number;
    public kpiDescription: string;
    public source: CheckListType;
    public measure: MeasureViewModel;
    public standard: string;
    public kpiMeasure: any;
    public isHeatMapItem: boolean;
    public kpiMeasureUnit: string;
    public heatMapScore: number;
    public sendAlert: boolean;
    public sendToList: any;
    public sendToRelationshipManager: boolean;
    public sendToBillingManager: boolean;
    public escalateAlert: boolean;
    public escalateTriggerTime: string;
    public alertLevel: string;
    public isUniversal: boolean;
    public recordStatus: string;
}
