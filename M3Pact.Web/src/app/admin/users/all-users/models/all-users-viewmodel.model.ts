import { ClientViewModel } from '../../../../shared/models/DepositLog/client.model';

export class AllUsersViewModel {
    public id: number;
    public userId: any;
    public firstName: string;
    public lastName: string;
    public mobileNumber: any;
    public email: string;
    public recordStatus: string;
    public isActive: boolean;
    public businessUnit: string;
    public reportsTo: string;
    public site: string;
    public isMeridianUser: boolean;
    public role: string;
    public title: string;
    public selectedClientsCount: any;
    public selectedClients: Array<ClientViewModel>;
    public Status: string;
    public fullName: string;
}