// Angular imports
import { AfterViewInit, Component, OnDestroy, ViewEncapsulation, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

// Common File Imports
import { GlobalEventsManager } from '../../../shared/utilities/global-events-manager';
import { UserService } from '../../../shared/services';
import { User } from '../../../shared/models/user';
import { PAGE_TOP } from '../../../shared/utilities/resources/labels';
import { TodoListService } from '../../../todo-list/todo-list.service';
import { SCREEN_CODE } from '../../../shared/utilities/resources/screencode';

@Component({
    selector: 'app-page-top',
    templateUrl: './page-top.html',
    styleUrls: ['./page-top.scss'],
    encapsulation: ViewEncapsulation.None,
    providers: [TodoListService]
})
export class PageTopComponent implements AfterViewInit, OnDestroy {

    /*------ region public properties ------*/
    public currentUser: User;
    public name: string;
    public lastName: string;
    public activeClients: any[];
    public selectedClientCode: any;
    public showClientDropdown = true;
    public labels = PAGE_TOP;
    public showTodoIcon: boolean;
    public showIcon: boolean;
    /*------ end region public properties ------*/

    /*------ region constructor ------*/
    constructor(private router: Router, private zone: NgZone, private location: Location,
        private _globalEventsManager: GlobalEventsManager, private _userService: UserService,
        private _todoService: TodoListService
    ) {
    }
    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngOnInit(): void {
        this.currentUser = this._userService.getCurrentUser() || null;
        if (this.currentUser) {
            this.getActiveClients();           
            if (this.currentUser.email) {
                this.currentUser.fullName = this.currentUser.email.substring(0, this.currentUser.email.lastIndexOf('@'));
                if (this.currentUser.email.indexOf('@') < 0) {
                    this.currentUser.fullName = this.currentUser.email;
                }
            }
        }

        this._globalEventsManager.getGlobalClientCode.subscribe(
            (clientCode) => this.zone.run(() => {
                if (this.activeClients) {
                    this.selectedClientCode = clientCode.value;
                    let isClientPresent = this.activeClients.findIndex(cc => cc.value === clientCode.value && cc.clientName === clientCode.clientName);
                    if (isClientPresent === -1) {
                        this.getActiveClients();
                    }
                }
            })
        );

        this._globalEventsManager.getClientCode.subscribe(
            (clientCode) => this.zone.run(() => {
                if (this.activeClients) {
                    let isClientPresent = this.activeClients.findIndex(cc => cc.value === clientCode.value && cc.clientName === clientCode.clientName);
                    if (isClientPresent === 1) {
                        this.getActiveClients();
                    }
                }
            })
        );

        this._globalEventsManager.showClientDropdown.subscribe(
            (show) => this.zone.run(() => {
                this.showClientDropdown = show;
            })
        );

        this.showOrHideTodoIcon();
    }

    ngAfterViewInit(): void {
    }

    ngOnDestroy(): void {
    }
    /*------ end region life cycle hooks ------*/

    /*------ region Public methods ------*/
    public signOut() {
        localStorage.clear();
        sessionStorage.clear();
        this._userService.removeCurrentUser();
        // Load layout after logged in
        this._globalEventsManager.isUserLoggedIn(false);
        this.router.navigate(['/login']);
    }

    getActiveClients() {
        this._userService.getActiveClients().subscribe((clients) => {
            if (clients) {
                this.activeClients = [];
                for (let client of clients) {
                    this.activeClients.push({ label: client.clientName, value: client.clientCode, clientName: client.clientName });
                }
            }
            this.setClientCodeGlobally();
        });
    }

    setClientCodeGlobally() {
        let selectedClient = this.activeClients.find(cc => cc.value === this.selectedClientCode);
        if (selectedClient == undefined) {
            let firstClient = this.activeClients[0];
            this._globalEventsManager.setGlobalClientCode(firstClient);
        } else {
            this._globalEventsManager.setGlobalClientCode(selectedClient);
        }
        this.router.navigateByUrl(this.location.path(false));
    }
    /**
     * Redirect to Todo Page
     */
    redirectToDo() {
        this.router.navigateByUrl('/todo');
    }
    /**
     * To highlight todo icon with an exclamation 
     */
    highlightTodoIcon() {        
        this._todoService.GetClientsForAllToDoTasks().subscribe(
            (resp) => {
                if (resp != null && resp.length > 0) {
                    this.showIcon = true;
                   // jQuery('#todo').addClass('alert1');
                } else if (resp != null && resp.length == 0) {
                    this.showIcon = false;
                    //jQuery('#todo').removeClass('alert1');
                    jQuery('#todo').off('click');
                    jQuery('#todo').css({ 'pointer-events': "none", 'cursor': "not-allowed" });
                }
            },
            (err) => { }
        )
    }

     /**
     * To show or hide todo icon 
     */
    showOrHideTodoIcon() {
        this._userService.getUserScreenActions(SCREEN_CODE.TODO)
            .subscribe((actions) => {
                if (actions) {
                    if (actions.length === 0) {
                        this.showTodoIcon = false;
                    } else {
                        this.showTodoIcon = true;
                        this.highlightTodoIcon();
                    }
                }
            });
    }
    /*------ end region Public methods ------*/
}
