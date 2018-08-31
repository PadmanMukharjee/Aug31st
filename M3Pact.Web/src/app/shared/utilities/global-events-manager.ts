import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class GlobalEventsManager {


    protected _isUserLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
    protected _isCurrentUserChanged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
    protected _isLoaderShown: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public userLoggedInEmitter: Observable<boolean> = this._isUserLoggedIn.asObservable();
    public currentUserChangedEmitter: Observable<boolean> =
    this._isCurrentUserChanged.asObservable();
    public loaderEmitter: Observable<boolean> = this._isLoaderShown.asObservable();

    public _showCloseMonth: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
    public showCloseMonth: Observable<boolean> = this._showCloseMonth.asObservable();

    public _showMonthlyChecklist: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
    public showMonthlyChecklist: Observable<boolean> = this._showMonthlyChecklist.asObservable();

    protected _errorType: BehaviorSubject<string> = new BehaviorSubject<string>(null);
    public errorStateHandler: Observable<string> = this._errorType.asObservable();

    protected _globalClientCode: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    public getGlobalClientCode: Observable<any> = this._globalClientCode.asObservable();

    protected _clientCode: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    public getClientCode: Observable<any> = this._clientCode.asObservable();

    protected _clientMode: BehaviorSubject<string> = new BehaviorSubject<string>(null);
    public getClientMode: Observable<string> = this._clientMode.asObservable();


    protected _showClientDropdown: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
    public showClientDropdown: Observable<boolean> = this._showClientDropdown.asObservable();

    protected _powerBILoginToken: BehaviorSubject<string> = new BehaviorSubject<string>(null); // PowerBI token for dashboard and reports page
    public getPowerBIToken: Observable<string> = this._powerBILoginToken.asObservable();

    constructor() { }

    public isUserLoggedIn(ifLoggedIn: boolean) {
        this._isUserLoggedIn.next(ifLoggedIn);
    }
    public updateCurrentUser(ifChanged: boolean) {
        this._isCurrentUserChanged.next(ifChanged);
    }

    public toggleLoader(show: boolean) {
        this._isLoaderShown.next(show);
    }
    public error(statusCode: string) {
        this._errorType.next(statusCode);
    }

    /**
     * Set from View/Edit Client page or Global selected client dropdown
     * @param clientCode
     */
    public setGlobalClientCode(clientCode: any) {
        this._globalClientCode.next(clientCode);
    }

    /**
     * Set from Client Create page or View All Clients page
     * @param clientCode
     */
    public setClientCode(clientCode: any) {
        this._clientCode.next(clientCode);
    }

    public setClientMode(clientMode: string) {
        this._clientMode.next(clientMode);
    }

    /**
     * To set wether to show client dropdown
     * @param show
     */
    public setClientDropdown(show: boolean) {
        this._showClientDropdown.next(show);
    }

    public setPowerBIToken(token: string) {
        this._powerBILoginToken.next(token);
    }

    public showCloseMonthPage(show: boolean) {
        this._showCloseMonth.next(show);
    }

    public showPendingChecklistMonthly(show: boolean) {
        this._showMonthlyChecklist.next(show);
    }
}
