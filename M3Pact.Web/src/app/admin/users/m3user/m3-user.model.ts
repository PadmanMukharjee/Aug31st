import { SelectedClients } from '../all-users/models/selected-clients.model';
import { UserLogin } from '../../../shared/models/user-login';
import { ValidationResponseViewModel } from '../../../common/models/validation.model';


export class M3UserModel extends ValidationResponseViewModel {
    public user: UserLogin;
    public isUserExist: boolean;
    public selectedRole: string;
    public businessUnit: string;
    public title: string;
    public reportsTo: string;
    public Site: string;
    public clients: SelectedClients[];
    public isActiveEmployee: boolean;
    constructor() {
        super();
        this.user = new UserLogin();
        this.clients = new Array<SelectedClients>();
    }
}
