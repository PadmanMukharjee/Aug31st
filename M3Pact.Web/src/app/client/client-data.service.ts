import { Headers, RequestOptions } from '@angular/http';
import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { ClientViewModel } from './../shared/models/DepositLog/client.model';
import { HttpRequest, HttpEventType, HttpClient, HttpHeaders } from '@angular/common/http';
import { Http } from '@angular/http';
import { AuthenticationService } from '../shared/services';


@Injectable()
export class ClientDataService {

    constructor(
        private httpUtility: HttpUtility, private http: Http, private _authService: AuthenticationService
    ) {
    }
    public progress: number;
    public message: string;

    /**
     * Used to get the client data depending upon clientCode
     * @param clientCode
     */
    getClientData(clientCode) {
        return this.httpUtility.get('ClientData/GetClientData?ClientCode=' + clientCode);
    }

    /**
     * Used to save the client details
     * @param clientData
     */
    saveClientData(clientData: FormData) {
        let token = this._authService.getTokenFromStorage();
        let opts = new RequestOptions();
        let headers = new Headers();
        headers.append('Authorization', `Bearer  ${token}`);
        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        opts.headers = headers;
        return this.http.post(this.httpUtility.getApiUrl('ClientData/SaveClientData'), clientData);
    }

    /**
     * To get the document depending on path
     * @param filename
     */
    getDocument(filename: string) {
        return this.httpUtility.get('ClientData/GetDocument?path=' + filename);
    }

    /**
     *  Used to get the client step status details
     * @param clientCode
     */
    getClientStepStatusDetails(clientCode) {
        return this.httpUtility.get('ClientData/GetClientStepStatusDetails?clientCode=' + clientCode);
    }

    /**
     *  Used to save the client step status details
     * @param stepStatusDetail
     */
    saveClientStepStatus(stepStatusDetail) {
        return this.httpUtility.post('ClientData/SaveClientStepStatusDetail', stepStatusDetail);
    }

    activateClient(clientCode: string) {
        return this.httpUtility.post('ClientData/ActivateClient?clientCode=' + clientCode, null);
    }

    /**
     * check for client code already exists or not
     * @param clientCode
     */
    checkForExistingClientCode(clientCode: string) {
        return this.httpUtility.get('ClientData/CheckForExistingClientCode?clientCode=' + clientCode);
    }
}
