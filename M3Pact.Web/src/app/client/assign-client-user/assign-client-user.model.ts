import { AllUsersViewModel } from '../../admin/users/all-users/models/all-users-viewmodel.model';

export class ClientUsersModel {
    constructor() {
        this.clientUsers = new Array<AllUsersViewModel>();
    }
    public clientCode: string;
    public clientUsers: AllUsersViewModel[];

}
