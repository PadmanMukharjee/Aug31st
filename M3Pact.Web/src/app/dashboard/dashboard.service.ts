import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { Response, Headers, RequestOptions } from '@angular/http';
import { HttpRequest, HttpEventType, HttpClient, HttpHeaders } from '@angular/common/http';
import { Http } from '@angular/http';


@Injectable()
export class DashBoardService {
    constructor(private http: Http, private httpUtility: HttpUtility) {

    }

    fetchPowerBIToken() {
        return this.httpUtility.get('/PowerBI');
    }

    fetchGroups(token) {
        // console.log("fetchGroups" + token)
        /*return this.http.get(`https://conduit.productionready.io/api/profiles/eric`)
        .map((res:Response) => res.json()).subscribe(data => console.log(JSON.stringify(data)));*/
        return this.getApiCall(`https://api.powerbi.com/beta/myorg/groups`, token);
    }

    fetchReports(selectedWorkspace, token) {
        return this.getApiCall(`https://api.powerbi.com/beta/myorg/` + (selectedWorkspace ? `groups/` + selectedWorkspace + `/reports` : `reports`), token);
    }

    fetchDashboards(selectedWorkspace, token) {
        return this.getApiCall(`https://api.powerbi.com/beta/myorg/` + (selectedWorkspace ? `groups/` + selectedWorkspace + `/dashboards` : `dashboards`), token);
    }

    getApiCall(endpoint, token) {
        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        headers.append('Authorization', 'Bearer ' + token);
        const opts = new RequestOptions({ headers: headers });
        return this.http.get(endpoint, opts)
            .map((res: Response) => res.json());
    }
}
