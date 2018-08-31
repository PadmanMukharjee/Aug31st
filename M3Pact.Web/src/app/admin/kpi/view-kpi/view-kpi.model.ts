export class KPIEditViewModel {
    public id: number;
    public kpiDescription: string;
    public measure: string;
    public standard: string;
    public isHeatMapItem: ValueText;
    public sendAlert: ValueText;
    public escalateAlert: ValueText;
    public isUniversal: ValueText;
}
export interface ValueText {
    value: string | number | boolean;
    text: string;
}