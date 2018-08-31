import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../models/index';
import { UserService } from './user.service';
import { Observable } from 'rxjs/Observable';
import { map, take } from 'rxjs/operators';
import { GlobalEventsManager } from '../utilities/global-events-manager';

@Injectable()
export class AuthGuardService implements CanActivate {

    /*------ region constructor ------*/

    constructor(private router: Router, private _userService: UserService, private _globalEventsManager: GlobalEventsManager) {
    }

    /*------ end region constructor ------*/

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        // check guards for stopping tester or project admin from
        // accessing user crud and user role crud pages
        let currentUser: User = this._userService.getCurrentUser();
        if (currentUser != null) {
            // logged in so return true
            return true;
        }
        // not logged in so redirect to login page with the return url
        this._globalEventsManager.isUserLoggedIn(false);
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
