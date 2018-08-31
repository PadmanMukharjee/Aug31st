import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { HeatMapItemModel } from './HeatMapItemModel';

@Injectable()
export class HeatmapService {

    constructor(private httpUtility: HttpUtility) { }

    /**
     * Get All Universal KPIs
     */
    getKpisforHeatMap() {
        return this.httpUtility.get('HeatMapSetup/GetKpisforHeatMap');
    }

    /**
     * Get HeatMap Items
     */
    getHeatMapItems() {
        return this.httpUtility.get('HeatMapSetup/GetHeatMapItems');
    }

    /**
     * Save HeatMap Items
     * @param heatMaps
     */
    saveHeatMapItems(heatMaps: HeatMapItemModel[]) {
        return this.httpUtility.post('HeatMapSetup/SaveHeatMapItems', heatMaps);
    }

}
