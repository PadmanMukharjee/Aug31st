import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { ClientUsersModel } from './assign-client-user.model';

@Injectable()
export class AssignClientUserService {
    constructor(private httpUtility: HttpUtility) {
    }

    /**
     * To Get all the Users
     */
    getAllUsers() {
        return this.httpUtility.get('ClientUser/GetAllUsersData');
    }

    /**
     * To get All M3Users
     */
    getAllM3Users() {
        return this.httpUtility.get('ClientUser/GetAllM3Users');
    }

    /**
     * To Save Users associated with Client
     * @param clientUsers
     */
    saveClientUsers(clientUsers: ClientUsersModel[]) {
        return this.httpUtility.post('ClientUser/SaveClientUserMap', clientUsers);
    }

    /**
     * Get Users associated with client
     * @param clientCode
     */
    getClientUsers(clientCode: string) {
        return this.httpUtility.get(`ClientUser/GetUsersForClient?clientCode=${clientCode}`);
    }
}