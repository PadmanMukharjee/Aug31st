import { Inject, Injectable } from '@angular/core';
import { User } from '../models/user';
import { CookieService } from 'ngx-cookie-service';
import {AppConstants} from '../../app.constants';
import { HttpUtility } from '../utilities/http/http-utility.service';

@Injectable()
export class UserService {

    constructor(
        private httpUtility: HttpUtility,
        private cookieService: CookieService
    ) { }

    getCurrentUser(): User {
        let user = this.cookieService.get(AppConstants.user.currentUser);
        if (user === '') {
            return null;
        }

        return JSON.parse(this.cookieService.get(AppConstants.user.currentUser));
    }

    setCurrentUser(user: User) {
        this.cookieService.set(AppConstants.user.currentUser, JSON.stringify(user));
    }

    removeCurrentUser() {
        this.cookieService.delete(AppConstants.user.currentUser, '/');
        if (this.cookieService.check(AppConstants.user.currentUser)) {
            this.cookieService.delete(AppConstants.user.currentUser);
        }
    }

    setRole(role) {
        this.cookieService.set(AppConstants.user.role, JSON.stringify(role));
    }

    getRoleCode() {
        let role = this.cookieService.get(AppConstants.user.role);
        if (role === '') {
            return null;
        }
        return JSON.parse(this.cookieService.get(AppConstants.user.role));
    }

    getActiveClients() {
        return this.httpUtility.get('ClientData/GetActiveClientsForAUser');
    }

    getUserScreenActions(screenCode: string) {
        return this.httpUtility.get('User/GetUserScreenActions?screenCode=' + screenCode);
    }
}
