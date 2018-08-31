import { HttpUtility } from '../../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { Http } from '@angular/http';

@Injectable()
export class MthreeUsersService {
    constructor(
        private httpUtility: HttpUtility, private http: Http) {
    }

    /*------------------- to get the roles--------------------*/
    getEmployeeDetails(empId) {
        return this.httpUtility.get('AllUsers/GetEmployeeDetails?employeeID=' + empId);
    }

    /*------------------- to get clients--------------------*/
    public getClients() {
        return this.httpUtility.get('ClientData/GetClientsForAUser');
    }

    /*------------------- to get user name for autofill--------------------*/
    public getUserNamesForAutoFill(key) {
        return this.httpUtility.get('AllUsers/GetEmpNamesForAutoFill?key=' + key);
    }

    /*------------------- to get the roles--------------------*/
    public getRoles() {
        return this.httpUtility.get('AllUsers/GetRolesForLoggedInUser');
    }

    saveUsers(m3User) {
        return this.httpUtility.post('AllUsers/SaveM3User', m3User);
    }

}
