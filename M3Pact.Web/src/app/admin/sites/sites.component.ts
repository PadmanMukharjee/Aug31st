// Third party imports
import { Message } from 'primeng/components/common/api';
import * as _ from 'lodash';

// Angular imports
import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

// Common File Imports
import { SitesService } from './sites.service';
import { SiteViewModel } from './site.model';
import { ADMIN_SITE, ADMIN_SHARED, ADMIN_BUSINESS_UNIT, SHARED } from '../../shared/utilities/resources/labels';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';

@Component({
    selector: 'app-sites',
    templateUrl: './sites.component.html',
    providers: [SitesService],
    styleUrls: ['../../../assets/build/css/switch.css'],
})
export class SitesComponent implements OnInit, OnDestroy {

    /*------ region public properties ------*/

    public labels = ADMIN_SITE;
    public sharedLabels = ADMIN_SHARED;
    public BusinessLables = ADMIN_BUSINESS_UNIT;
    public validationConstants = VALIDATION_MESSAGES;
    public messageList: Message[] = [];
    public selectedSite: SiteViewModel;
    public businessUnitsofSite: any[] = [];
    public displaybusinessUnits: boolean;
    public dialogHeader: string;
    public valid = true;
    public siteNameValid = true;
    public siteAcronymLengthValid = true;
    public requiredMessage: string;
    public businessSites: SiteViewModel[];
    public businessSite: SiteViewModel;
    public selectedBusinessSite: any;
    public displayDialog: boolean;
    public isAddBusinessSite: boolean;
    public canEdit = false;
    public editActions: any[] = [];
    public displayingClients: boolean;
    public displayingBusinessUnits: boolean;
    public growlMessage: Message[] = [];
    public isSiteExist: boolean;
    public isSiteCodeExist: boolean;
    public paginationCount: any;
    public rowsIn: number = 10;
    public businessSitesFiltered: any;

    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(private businessSitesService: SitesService, private router: Router,
        private userService: UserService, private ref: ChangeDetectorRef) {
    }

    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.businessSites = [];
        this.getScreenActions();
        this.getSites();
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }

    /*------ end region life cycle hooks ------*/


    /*------ region service calls------*/

    /**
     * Get Edit Actions of Sites page
     */
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.SITES)
            .subscribe((actions) => {
                if (actions) {
                    if (actions.length === 0) {
                        this.canEdit = false;
                    } else {
                        this.canEdit = true;
                        this.editActions = actions;
                    }
                }
            });
    }

    /**
     * gets the sites data from db
    **/
    getSites() {
        this.businessSitesService.getSites(false).subscribe(
            (response: any) => {
                if (response != null) {
                    this.businessSites = response;
                    this.businessSites.forEach(x => {
                        x.isActive = (x.recordStatus === this.sharedLabels.ACTIVE_CHAR);
                        x.clientsCount = x.clients.length;
                    });
                    this.businessSites = this.businessSites.reverse();
                    this.businessSitesFiltered = this.businessSites;
                } else {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            }
        );
    }

    /**
     * saves BusinessSites Changes to db
    **/
    saveBusinessSitesChanges(businessSites: SiteViewModel[]) {
        this.businessSitesService.saveSites(businessSites).subscribe(
            (success) => {
                if (success) {
                    this.getSites();
                } else {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }

    /*------ end region service calls------*/


    /*------ region public methods ------*/

     /**
     * Called when table's page change event occurs
     * @param event
     */
    onTablePageChange(e) {
        let start = this.businessSitesFiltered.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.businessSitesFiltered.length ? this.businessSitesFiltered.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.businessSitesFiltered.length;
    }

    /**
     * Called when filter event occurs
     * @param event
     */
    onTableFilter(event) {
        this.businessSitesFiltered = event.filteredValue;
    }

    /**
     * displays a dialog on selecting a row
    **/
    onRowSelect(site: SiteViewModel) {
        this.businessSite = new SiteViewModel();
        this.businessSite.siteName = site.siteName;
        this.businessSite.siteCode = site.siteCode;
        this.businessSite.siteId = site.siteId;
        this.businessSite.isActive = site.isActive;
        this.dialogHeader = this.sharedLabels.EDIT + ' ' + this.labels.BUSINESS_SITE;
        this.displayDialog = true;
        this.focusSite();
    }

    /**
     * Triggers on change of Site
     * @param site
     */
    changeStateOfSite(site: SiteViewModel) {
        this.selectedSite = site;
        if (!site.isActive) {
            this.businessSitesService.getClientsAssociatedWithSite(site).subscribe(
                data => {
                    if (data != null && data.length > 0) {
                        this.businessUnitsofSite = data;
                        this.displaybusinessUnits = true;
                    } else {
                        this.activeOrInactiveBusinessSite();
                    }
                }
            );
        } else {
            this.activeOrInactiveBusinessSite();
        }
    }

    /**
     * Activate or Inactivate Site
     */
    activeOrInactiveBusinessSite() {
        this.businessSitesService.activateOrInactivateSites(this.selectedSite).subscribe(
            data => {
                if (!data) {
                    this.growlMessage = [];
                    this.growlMessage.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
                this.displaybusinessUnits = false;
                this.businessUnitsofSite = [];
                this.router.navigateByUrl(this.router.url);
            }
        );
    }

    /**
     * keep Active Business Site
     */
    keepActiveBusinessSite() {
        this.displaybusinessUnits = false;
        this.selectedSite.isActive = true;
        this.businessUnitsofSite = [];
    }

    /**
     * displays a dialog on clicking add button
    **/
    showDialogToAdd() {
        this.isAddBusinessSite = true;
        this.businessSite = new SiteViewModel();
        this.businessSite.isActive = true;
        this.dialogHeader = this.sharedLabels.ADD + ' ' + this.labels.BUSINESS_SITE;
        this.displayDialog = true;
        this.focusSite();
    }

    /**
     * added/edited changes to sites will be saved
    **/
    saveBusniessSite() {
        this.validateSite();
        if (!this.valid || !this.siteNameValid || !this.siteAcronymLengthValid) {
            return;
        }

        let businessSitesObject: SiteViewModel[];
        businessSitesObject = [];
        let businessSiteObject: SiteViewModel;

        if (this.isAddBusinessSite) {
            businessSiteObject = new SiteViewModel();
            this.isAddBusinessSite = false;
        } else {
            businessSiteObject = this.businessSites.find(x => x.siteId == this.businessSite.siteId);
        }

        this.checkForSiteExist();

        if (this.isSiteExist || this.isSiteCodeExist) {
            return;
        }

        businessSiteObject.siteCode = this.businessSite.siteCode.trim();
        businessSiteObject.siteName = this.businessSite.siteName.trim();
        businessSiteObject.siteDescription = this.businessSite.siteName.trim();
        businessSiteObject.isActive = this.businessSite.isActive;
        if (businessSiteObject.isActive) {
            businessSiteObject.recordStatus = ADMIN_SHARED.ACTIVE_CHAR;
        } else {
            businessSiteObject.recordStatus = ADMIN_SHARED.INACTIVE_CHAR;
        }
        businessSitesObject.push(businessSiteObject);
        this.businessSites = [];
        this.saveBusinessSitesChanges(businessSitesObject);
        this.businessSite = null;
        this.displayDialog = false;
    }

    /**
     * clear messages
     */
    clearMessages() {
        this.messageList = [];
        this.valid = true;
        this.siteNameValid = true;
        this.siteAcronymLengthValid = true;
    }

    /**
     * Focus on input field after the popup is shown
     */
    focusSite() {
        this.ref.detectChanges();
        jQuery('.site').focus();
    }

    /**
     * On click of client count
     * @param businessSite
     */
    onClientsColumnSelect(businessSite) {
        this.displayingClients = true;
        this.businessSite = businessSite;
    }

    /**
     * Close popup
     */
    closeDialog() {
        this.displayingBusinessUnits = false;
        this.displayingClients = false;
    }

    /**
     * Validations when new site added
     */
    validateSite() {
        if (this.businessSite.siteCode == undefined || this.businessSite.siteCode.trim().length == 0) {
            this.valid = false;
        } else {
            if (this.businessSite.siteCode.length > 3) {
                this.siteAcronymLengthValid = false;
            } else {
                this.siteAcronymLengthValid = true;
            }
            this.valid = true;
        }
        if (this.businessSite.siteName == undefined || this.businessSite.siteName.trim().length == 0) {
            this.siteNameValid = false;
        } else {
            this.siteNameValid = true;
        }
    }

    /**
     * check whether a site already exist or not
     */
    checkForSiteExist() {
        if (!this.isAddBusinessSite) {
            if (this.businessSites.some(val => _.toLower(val.siteName) == _.toLower(this.businessSite.siteName) && val.siteId != this.businessSite.siteId)) {
                this.messageList = [];
                this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SITE_EXISTS });
                this.isSiteExist = true;
                return;
            }
            if (this.businessSites.some(val => _.toLower(val.siteCode) == _.toLower(this.businessSite.siteCode) && val.siteId != this.businessSite.siteId)) {
                this.messageList = [];
                this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SITE_CODE_EXISTS });
                this.isSiteCodeExist = true;
                return;
            }
        } else if (this.isAddBusinessSite) {
            if (this.businessSites.some(val => _.toLower(val.siteName) == _.toLower(this.businessSite.siteName))) {
                this.messageList = [];
                this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SITE_EXISTS });
                this.isSiteExist = true;
                return;
            }
            if (this.businessSites.some(val => _.toLower(val.siteCode) == _.toLower(this.businessSite.siteCode))) {
                this.messageList = [];
                this.messageList.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.SITE_CODE_EXISTS });
                this.isSiteCodeExist = true;
                return;
            }
        }
    }
 /*------ end region public methods ------*/
}

