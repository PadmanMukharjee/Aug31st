import { Config } from '../config';
import { GlobalEventsManager } from '../global-events-manager';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/concatAll';
import 'rxjs/add/operator/shareReplay';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class HttpUtility {

    private baseApiUrl: string;
    private authUrl: string;
    constructor(
        private config: Config,
        private http: HttpClient,
        private router: Router,
        private _globalEventsManager: GlobalEventsManager,
        private cookieService: CookieService ) {
        this.baseApiUrl = this.config.getByKey('baseApiUrl') + 'api/';
        this.authUrl = this.config.getByKey('authUrl');
    }

    /**
     * Post
     * @param url
     * @param body
     * @param options
     */
    public post(url: string, body: any, options?: HttpParams) {
        return this.http.post(this.getApiUrl(url), body)
            .catch((e) => this.handleError(e));
    }

    /**
     * Put
     * @param url
     * @param body
     * @param options
     */
    public put(url: string, body: any, options?: HttpParams) {
        return this.http.put(this.getApiUrl(url), body)
            .catch((e) => this.handleError(e));
    }

    /**
     * delete
     * @param url
     * @param options
     */
    public delete(url: string, options?: HttpParams) {
        return this.http.delete(this.getApiUrl(url))
            .catch((e) => this.handleError(e));
    }
    /**
     * get
     * @param url
     * @param options
     */
    public get(url: string, options?: HttpParams) {
        return this.http.get(this.getApiUrl(url))
            .shareReplay()
            .catch((e) => this.handleError(e));

    }

    /**
     * getFile
     * @param url
     * @param options
     */
    public getFile(url: string, options?: HttpParams) {
        window.open(this.getApiUrl(url));
    }

    /**
     * handleError
     * @param response
     */
    private handleError(errorResponse: HttpErrorResponse) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        let errorStatus = '404';

        if (errorResponse instanceof HttpErrorResponse) {
            errorStatus = errorResponse.status > 0 ? errorResponse.status.toLocaleString() : '404';
        }
        if (errorStatus !== '401' && errorStatus !== '400') {
            this._globalEventsManager.error(errorStatus);

            // Redirect the user after login
            return this.router.navigate(['/error']);
        }
        if (errorStatus === '401') {
            this.cookieService.delete('currentUser');
            this._globalEventsManager.isUserLoggedIn(false);
            return this.router.navigate(['/login']);
        }
        return Observable.throw(errorResponse);

    }


    /**
     * getApiUrl
     * @param url
     */
    public getApiUrl(url) {
        if (url.includes('Account/Login')) {
            return this.authUrl + url;
        } else {
            return this.baseApiUrl + url;
        }
    }

}
