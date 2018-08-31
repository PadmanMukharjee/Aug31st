import { ClientViewModel } from './DepositLog/client.model';
import { SelectedClients } from '../../admin/users/all-users/models/selected-clients.model';
import { ValidationResponseViewModel } from '../../common/models/validation.model';

export class UserLogin extends ValidationResponseViewModel {
    public userId: any;
    public firstName: string;
    public lastName: string;
    public middleName: string;
    public userName: string;
    public mobileNumber: any;
    public email: string;
    public password: string;
    public recordStatus: string;
    public isActive: boolean;
    public isMeridianUser: boolean;
    public roleCode: string;
    public roleName: string;
    public incorrectLoginAttemptsCount: number;
    public lastSuccessfulLogin: Date;
    public lockoutEndDateUtc: Date;
    public lastPasswordChanged: Date;
    public lockoutEnabled: boolean;
    public clients: ClientViewModel[];
    public selectedClientData: SelectedClients[];
    public forgotPasswordToken: string;
    public loggedInUserID: string;
    constructor() {
        super();
        this.clients = [];
        this.selectedClientData = new Array<SelectedClients>();
    }
}