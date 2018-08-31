import { Component, NgZone, OnInit, ViewEncapsulation } from '@angular/core';
import { GlobalEventsManager } from './shared/utilities/global-events-manager';
import { User } from './shared/models';
import { UserService } from './shared/services/user.service';

/*
 * App Component
 * Top Level Component
 */
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss', "../assets/build/css/custom.scss"],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {

    /*-----region Public properties -----*/
    public currentUser: User;
    /*-----region Public properties -----*/

    /*-----region Constructor -----*/
    constructor(private _userService: UserService, private _globalEventsManager: GlobalEventsManager,
        private zone: NgZone) {
    }
    /*-----endregion Constructor -----*/

    /*-----region lifecycle event -----*/
    public ngOnInit(): void {
        this.currentUser = this._userService.getCurrentUser() || null;
        // After Loggin Success fire this event
        this._globalEventsManager.userLoggedInEmitter.subscribe(
            (isLoggedIn) => this.zone.run(() => {
                // mode will be null the first time it is created,
                if (isLoggedIn !== null) {
                    if (isLoggedIn) {
                        this.currentUser = this._userService.getCurrentUser();
                    } else {
                        this.currentUser = null;
                    }
                }
            }));
    }
    /*-----endregion lifecycle event -----*/


}
