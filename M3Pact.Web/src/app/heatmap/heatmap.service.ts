import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';

@Injectable()
export class ClientsHeatMapService {

    constructor(private httpUtility: HttpUtility) {
    }

    // Get Clients HeatMap
    getClientsHeatMap(heatMapRequest) {
        return this.httpUtility.post('HeatMap/GetHeatMapForClients', heatMapRequest);
    }

    // Gets HeatMap filters data
    getHeatMapFilterData() {
        return this.httpUtility.get('HeatMap/GetHeatMapFilterData');
    }

}
