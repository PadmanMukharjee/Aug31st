import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { SystemsModel } from './systems.model';

@Injectable()
export class SystemsService {

    constructor(private httpUtility: HttpUtility) {
    }

    /**
     * Gets Systems
     */
    getAllSystems(fromClient: boolean = false) {
        return this.httpUtility.get('System/GetAllSystems?fromClient=' + fromClient);
    }

    /**
     * Saves Systems
     * @param systems
     */
    saveSystems(systems: SystemsModel[]) {
        return this.httpUtility.post('System/SaveSystems', systems);
    }

    /**
     * To get clients associated with system
     * @param systemId
     */
    getClientsAssociatedWithSystem(systemId: number) {
        return this.httpUtility.get('System/getClientsAssociatedWithSystem?systemId=' + systemId);
    }

    /**
     * active or deactive system
     * @param system
     */
    activateOrInactivateSystems(system: SystemsModel) {
        return this.httpUtility.post('System/ActivateOrInactivateSystems', system);
    }

}
