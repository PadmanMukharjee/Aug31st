import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { UserLogin } from '../shared/models/user-login';
import { LOGINPAGEURLS } from '../shared/utilities/resources/url';


@Injectable()
export class LoginService {

    public labels;

    constructor(
        private httpUtility: HttpUtility) {
        this.labels = LOGINPAGEURLS;
    }

    /**
     *sends user details for to server for validating
     * @param user
     */
    checkUser(user: UserLogin) {
        return this.httpUtility.post('AllUsers/CheckUser', user);
    }

}
