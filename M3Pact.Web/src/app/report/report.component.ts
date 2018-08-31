import { DashBoardService } from '../dashboard/dashboard.service';
import { Component, ViewEncapsulation } from '@angular/core';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { workspace } from '../dashboard/models/workspace.model';

import { Router } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';


@Component({
    selector: 'app-report',
    encapsulation: ViewEncapsulation.Emulated,
    templateUrl: './report.component.html',
    styleUrls: ['./report.component.css']
})
export class ReportComponent {

    /*------  region public properties ------*/

    public isDisabled: boolean;
    public testText: string;
    token: string;
    src: SafeResourceUrl = this.sanitizer.bypassSecurityTrustResourceUrl('');
    groups: any;
    workspaces = null;
    public workspaceData: any;
    reports = null;
    workspaceSelected: string;
    reportData: any;
    public workspaceOptions: workspace[];
    public selectedWorkspace: workspace;
    public reportOptions: any[];
    public selectedReport: any;

    /*------ end region public properties ------*/


    /*---------- region constructor ----------*/

    constructor(
        public sanitizer: DomSanitizer,
        private _globalEventsManager: GlobalEventsManager,
        private dashboardService: DashBoardService,
        private router: Router, ) {
        this.workspaceOptions = new Array<workspace>();
        this.selectedWorkspace = new workspace();
        this._globalEventsManager.setClientDropdown(false);
        _globalEventsManager.getPowerBIToken.subscribe(token => {
            if (token != null && token != undefined && token != '') {
                this.token = token;
                this.initialDataLoad();
            } else {
                this.fetchTokenAndReports();
            }
        });
    }

    /*-------- end region constructor --------*/

    /*-------- region public methods --------*/

    public showLoader() {
        this._globalEventsManager.toggleLoader(true);

    }

    fetchTokenAndReports() {
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
            this.getReports(null);
        },
            err => {
                this.fetchTokenAndReports();
            });
    }

    onLoad(iframe) {
        if (this.token) {
            iframe.contentWindow.postMessage(JSON.stringify({ action: 'loadReport', accessToken: this.token }), '*');
        }
    }

    getReports(selectedworkspace) {
        this.reports = null;
        this.workspaceSelected = selectedworkspace ? selectedworkspace.id : null;
        const reportsdata = this.dashboardService.fetchReports(this.workspaceSelected, this.token);
        reportsdata.subscribe(reports => {
            this.reports = reports.value;
            this.reportOptions = this.reports;
            if (this.reports.length) {
                this.selectedReport = reports.value[0];
                this.getSelectedReport(reports.value[0]);
            } else {
                // this.message = 'No report exists in this Workspace!';
                this.src = this.sanitizer.bypassSecurityTrustResourceUrl('');
            }
        }, (err) => {
        });
    }

    getSelectedReport(report) {
        this.src = this.sanitizer.bypassSecurityTrustResourceUrl(report.embedUrl);
        // this.src = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl+'&filter=ASM_AREA eq 'NSW - SOUTH'')
        // this.src = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl+'&filterPaneEnabled=false')
    }

    onChangeWorkspace() {
        this.getReports(this.selectedWorkspace);
    }

    onChangeReport() {
        this.getSelectedReport(this.selectedReport);
    }

    /*------ end region public methods------*/
}
