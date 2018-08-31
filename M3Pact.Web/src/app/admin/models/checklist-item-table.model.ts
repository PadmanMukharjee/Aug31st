import { KPIViewModel } from '../kpi/create-kpi/kpi.model';

export interface CheckListItemTable {
    id: number;
    question: string;
    checklistType: ValueLabel;
    expectedResponse: ValueText;
    KPI: ValueText;
    universal: ValueText;
    freeform: ValueText;
    kpiDescription: string;
    sendAlert: ValueText;
    escalateAlert: ValueText;
}

export interface ValueText {
    value: string | number | boolean;
    text: string;
}

export interface ValueLabel {
    value: string | number | boolean;
    label: string;
}
