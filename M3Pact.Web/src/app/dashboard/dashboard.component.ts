import { DashBoardService } from './dashboard.service';
import { Component, ViewEncapsulation } from '@angular/core';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';

import { Router } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { workspace } from './models/workspace.model';

@Component({
  selector: 'app-dashboard',
  encapsulation: ViewEncapsulation.Emulated,
    styleUrls: ['./dashboard.scss', './dashboard.scss'],
    templateUrl: './dashboard.html'
})
export class DashboardComponent {

    /*------ region public properties ------*/

    public dashBoards: Array<any>;
    public isDisabled: boolean;
    public testText: string;
    token: string;
    src: SafeResourceUrl = this.sanitizer.bypassSecurityTrustResourceUrl('');
    reportsrc: SafeResourceUrl = this.sanitizer.bypassSecurityTrustResourceUrl('');
    groups: any;
    workspaces = null;
    public workspaceData: any;
    reports = null;
    dashboards = null;
    workspaceSelected: string;
    reportData: any;
    dashboardData: any;
    public workspaceOptions: workspace[];
    public selectedWorkspace: workspace;
    public dashboardOptions: any[];
    public selectedDashboard: any;

    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(
        public sanitizer: DomSanitizer,
        private _globalEventsManager: GlobalEventsManager,
        private dashboardService: DashBoardService,
        private router: Router) {
        this.workspaceOptions = new Array<workspace>();
        this.selectedWorkspace = new workspace();
        this._globalEventsManager.setClientDropdown(false);
        _globalEventsManager.getPowerBIToken.subscribe(token => {
            if (token != null && token != undefined && token != '') {
                this.token = token;
                this.initialDataLoad();
            } else {
                this.fetchTokenAndDashboards();
            }
        });
    }

    /*------ end region constructor ------*/

    /*------ region Public methods ------*/

    public showLoader() {
        this._globalEventsManager.toggleLoader(true);
    }

    fetchTokenAndDashboards() {
        this.dashboardService.fetchPowerBIToken().subscribe(
            data => {
                this.token = data.access_token;
                this._globalEventsManager.setPowerBIToken(this.token);
                this.initialDataLoad();
            }, (err) => {
            });
    }

    initialDataLoad() {
        const groupsdata = this.dashboardService.fetchGroups(this.token);
        groupsdata.subscribe(data => {
            this.workspaces = [{ id: null, isReadOnly: true, isOnDedicatedCapacity: false, name: 'My Workspace' }].concat(data.value);
            this.workspaceOptions = this.workspaces;
            this.workspaceData = this.workspaces[0];
            this.selectedWorkspace = this.workspaceData;
            this.getDashboards(null);
        },
            err => {
                this.fetchTokenAndDashboards();
            });
    }

    onLoad(iframe) {
        if (this.token) {
            iframe.contentWindow.postMessage(JSON.stringify({ action: 'loadDashboard', accessToken: this.token }), '*');
        }
    }


    getDashboards(selectedworkspace) {
        this.reports = null;
        this.workspaceSelected = selectedworkspace ? selectedworkspace.id : null;
        const dashboardData = this.dashboardService.fetchDashboards(this.workspaceSelected, this.token);
        dashboardData.subscribe(dashboards => {
            this.dashboards = dashboards.value;
            this.dashboardOptions = this.dashboards;
            if (this.dashboards.length) {
                this.dashboardData = dashboards.value[0];
                this.selectedDashboard = this.dashboardData;
                this.getSelectedDashboard(this.selectedDashboard);
            } else {
                // this.message = 'No report exists in this Workspace!';
                this.src = this.sanitizer.bypassSecurityTrustResourceUrl('');
            }
        }, (err) => {
        });
    }

    getSelectedDashboard(dashboard) {
        this.src = this.sanitizer.bypassSecurityTrustResourceUrl(dashboard.embedUrl);
        // this.src = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl+'&filter=ASM_AREA eq 'NSW - SOUTH'')
        // this.src = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl+'&filterPaneEnabled=false')
    }

    onChangeWorkspace() {
        this.getDashboards(this.selectedWorkspace);
        // this.getReports(this.selectedWorkspace);
    }

    onChangeDashboard() {
        this.getSelectedDashboard(this.selectedDashboard);
    }

    /*------ end region Public methods ------*/
}



