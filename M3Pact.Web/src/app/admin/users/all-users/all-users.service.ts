import { HttpUtility } from '../../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { AllUsersViewModel } from './models/all-users-viewmodel.model';


@Injectable()
export class AllUsersService {
    constructor(
        private httpUtility: HttpUtility) {
    }

    /**
     * Get All Users
     */
    getAllUsers() {
        return this.httpUtility.get('AllUsers/GetAllUsersDataAndClientsAssigned');
    }

    /**
     * Save Users
     * @param userModel
     */
    saveUsers(userModel: AllUsersViewModel[]) {
          return this.httpUtility.post('AllUsers/SaveUsers', userModel);
    }
}