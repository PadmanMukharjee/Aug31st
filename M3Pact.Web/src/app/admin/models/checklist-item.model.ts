import { KPIViewModel } from '../kpi/create-kpi/kpi.model';
import { CheckListType } from '../models/checklist-type.model';

export class CheckListItem {
    questionID: number;
    questionCode: string;
    question: string;
    id: string;
    kpi: boolean;
    universal: boolean;
    expectedResponse: boolean;
    freeform: boolean;
    kpiDescription: KPIViewModel;
    checkListType: CheckListType;
}