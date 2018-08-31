import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { UserLogin } from '../../shared/models/user-login';


@Injectable()
export class ResetPasswordService {
    constructor(
        private httpUtility: HttpUtility) {
    }
    /**
     * send user data to savenewpassword method
     * @param user
     */
    public updatePassword(user: UserLogin) {
        return this.httpUtility.post('AllUsers/SaveNewPasswordData', user);
    }
}
