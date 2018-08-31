import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { SpecialitiesModel } from './specialities.model';

@Injectable()
export class SpecialitiesService {

    constructor(private httpUtility: HttpUtility) {
    }

    /**
     * Gets Specialities (returned result is sorted by Specialty Name when called from Client Data page).
     * @param fromClient
     */
    getSpecialities(fromClient: boolean = false) {
        return this.httpUtility.get('Speciality/GetSpecialities?fromClient=' + fromClient);
    }

    /**
     * Saves specialities
     * @param specialities
     */
    saveSpecialities(specialities: SpecialitiesModel[]) {
        return this.httpUtility.post('Speciality/SaveSpecialities', specialities);
    }

    /**
     * Activate Or Inactivate Specialties
     * @param speciality
     */
    activeOrInactiveSpecialities(speciality: SpecialitiesModel) {
        return this.httpUtility.post('Speciality/ActiveOrInactiveSpecialities', speciality);
    }

    /**
     * Get Clients Associated With Specialty
     * @param specialtyId
     */
    getClientsAssociatedWithSpecialty(specialtyId: number) {
        return this.httpUtility.get('Speciality/GetClientsAssociatedWithSpecialty?specialtyId=' + specialtyId);
    }
}
