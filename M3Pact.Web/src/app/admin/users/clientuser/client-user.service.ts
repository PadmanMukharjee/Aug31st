import { HttpUtility } from '../../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { UserLogin } from '../../../shared/models/user-login';

@Injectable()
export class ClientUsersService {

    constructor(
        private httpUtility: HttpUtility) {
    }

    /**
     * Get clients for a user
     */
    public getClients() {
        return this.httpUtility.get('ClientData/GetClientsForAUser');
    }

    /**
     * Get User by UserID
     * @param userId
     */
    public getUserById(userId) {
        return this.httpUtility.get('AllUsers/GetUserDetails?userID=' + userId);
    }

    /**
     * Save Client User
     * @param clientUser
     */
    public saveUsers(clientUser) {
        return this.httpUtility.post('AllUsers/SaveClientUser', clientUser);
    }

}