import { Injectable } from '@angular/core';
import { HttpUtility } from '../../shared/utilities/http/http-utility.service';

@Injectable()
export class ViewAllClientsService {
    constructor(private httpUtility: HttpUtility) { }



    /**
     * Service method to get all clients data
     */
    GetAllClients() {
        return this.httpUtility.get('ClientData/GetAllClientsData');
    }

    /**
     * To get all the sites.
     */
    GetAllSites() {
        return this.httpUtility.get('ClientData/GetAllSites');
    }

}
