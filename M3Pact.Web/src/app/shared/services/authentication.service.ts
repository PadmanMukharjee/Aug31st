import { HttpUtility } from '../utilities/http/http-utility.service';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { User, ResponseModel } from '../models';
import 'rxjs/add/operator/map';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Config } from '../utilities/config';
import { CookieService } from 'ngx-cookie-service';
import { AppConstants } from '../../app.constants';

@Injectable()
export class AuthenticationService {

    /*------ region constructor ------*/
    constructor(private httpClient: HttpClient,
        private cookieService: CookieService, private config: Config) { }
    /*------ end region constructor ------*/

    /*------ region public methods  ------*/

    public getAuthenticationToken(user: User) {        
        let params = {
            userName: user.email,
            password: user.password,
            grant_type: 'password'
        };
        let formData = new FormData();
        for (let k in params) {
            formData.append(k, params[k]);
        }        
        const headers = new HttpHeaders().append('Authorization', 'Basic TTNwYWN0OnNlY3JldA==');
        let authUrl = this.config.getByKey('authUrl');
        return this.httpClient.post(authUrl + 'connect/token', formData, { headers });
    }


    logout() {
        // remove user from local storage to log user out
        this.cookieService.delete(AppConstants.user.currentUser);
        this.cookieService.delete(AppConstants.user.role);
    }

    getTokenFromStorage() {
        let user = this.cookieService.get(AppConstants.user.currentUser);
        if (user === '') {
            return '';
        }

        return JSON.parse(this.cookieService.get(AppConstants.user.currentUser)).accessToken;
    }

    getTestAccounts() {
        let testAccountString = this.config.getByKey('testAccounts');
        let testAccounts = [];
        if (testAccountString) {
            testAccounts = testAccountString.split(',');
        }
        return testAccounts;
    }

    /*------end region public methods------*/
}
