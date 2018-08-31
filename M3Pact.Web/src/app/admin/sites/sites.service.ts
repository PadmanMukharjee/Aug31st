import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { SiteViewModel } from './site.model';

@Injectable()
export class SitesService {
    constructor(
        private httpUtility: HttpUtility) {
    }

    /**
     * Get All Business Sites
     * @param isActiveSites
     */
    getSites(isActiveSites: boolean) {
        return this.httpUtility.get('Site/GetSites?isActiveSites=' + isActiveSites);
    }

    /**
     * Save Sites
     * @param siteModel
     */
    saveSites(siteModel: SiteViewModel[]) {
        return this.httpUtility.post('Site/SaveSites', siteModel);
    }

    /**
     * Get Clients Associated With Site
     * @param site
     */
    getClientsAssociatedWithSite(site: SiteViewModel) {
        return this.httpUtility.post('Site/GetClientsAssociatedWithSite', site);
    }

    /**
     * Activate Or Inactivate Sites
     * @param site
     */
    activateOrInactivateSites(site: SiteViewModel) {
        return this.httpUtility.post('Site/ActivateOrInactivateSites', site);
    }

    /**
     * Get Sites Dropdown
     * @param isActiveSites
     */
    getSitesDropdown(isActiveSites: boolean) {
        return this.httpUtility.get('Site/GetSitesDropdown?isActiveSites=' + isActiveSites);
    }
}