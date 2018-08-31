import { PayerData } from './payer-data.model';

export class DepositLogClientData {
    public clientCode: string;
    public date: Date;
    public total: number;
    public payers = new Array<PayerData>();
    public isMonthClosed: boolean;
    public savedLastNumberOfBusinessDays: number;
}

export class ExportDepositViewModel {
    public clientCode: string;
    public exportMonths= new Array<Date>();
}

