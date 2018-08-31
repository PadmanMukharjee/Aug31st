import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { BusinessUnitViewModel } from './business-units.model';

@Injectable()
export class BusinessUnitsService {
    constructor(private httpUtility: HttpUtility) {
    }

    /**
     * Get Business Units (returned result is sorted by BusniessUnit Name when called from Client Data page)
     * @param fromClient
     */
    getBusinessUnits(fromClient: boolean = false) {
        return this.httpUtility.get('BusinessUnit/GetBusinessUnits?fromClient=' + fromClient);
    }

    /**
     * Save Business Units
     * @param businessUnits
     */
    saveBusinessUnits(businessUnits: BusinessUnitViewModel[]) {
        return this.httpUtility.post('BusinessUnit/SaveBusinessUnits', businessUnits);
    }

    /**
     * Activate or Inactivate Business Unit
     * @param businessUnit
     */
    activeOrInactiveBusinessUnit(businessUnit: BusinessUnitViewModel) {
        return this.httpUtility.post('BusinessUnit/ActiveOrInactiveBusinessUnit', businessUnit);
    }

    /**
     * Get clients associated with Business Unit
     * @param businessUnitId
     */
    getClientsAssociatedWithBusinessUnit(businessUnitId: number) {
        return this.httpUtility.get('BusinessUnit/GetClientsAssociatedWithBusinessUnit?businessUnitId=' + businessUnitId);
    }
}
