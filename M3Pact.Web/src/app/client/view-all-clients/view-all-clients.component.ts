// Angular imports
import { Component, OnInit, ViewEncapsulation, ViewChild, ChangeDetectorRef, Inject, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

// Common imports

import { ViewAllClientsService } from './view-all-clients.service';
import { GlobalEventsManager } from '../../shared/utilities/global-events-manager';
import { VIEW_ALL_CLIENTS, SHARED } from '../../shared/utilities/resources/labels';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';
import { ValueLabel } from '../../admin/models/checklist-item-table.model';


// Third party Imports
import { Message } from 'primeng/components/common/api';
import { Table } from 'primeng/components/table/table';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';
import * as XLSX from 'xlsx';
import * as moment from 'moment';

@Component({
    selector: 'view-all-clients',
    templateUrl: './view-all-clients.component.html',
    styleUrls: ['./view-all-clients.component.css'],
})
export class ViewAllClientsComponent implements OnInit, OnDestroy {

    /*------ region public properties ------*/
    public paginationCount: any;
    public canEdit = false;
    public editActions: any[] = [];
    public statusOptions: any;
    public siteOptions: any;
    public client: any[];
    public labels = VIEW_ALL_CLIENTS;
    public msgs: Message[] = [];
    public dataLoading: boolean;
    public filteredClientData: any[];
    public rowsIn: number = 20;
    public localStorageData: any = [];

    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService, private _viewAllClientsService: ViewAllClientsService,
                private _globalEventsManager: GlobalEventsManager, private _userService: UserService, private router: Router, private cdref: ChangeDetectorRef) {
        this.getScreenActions();
        this.getAllSites();
        this.getClientsData();
    }

    /*------ end region constructor ------*/

    ngOnInit() {
        this.statusOptions = [
            { label: 'Active', value: 'Active' },
            { label: 'Inactive', value: 'Inactive' },
            { label: 'Partially Completed', value: 'Partially Completed' }
        ];
    }

    ngOnDestroy() {
        if (jQuery('.ui-table-scrollable-body')) {
            jQuery('.ui-table-scrollable-body').off('click');
        }
    }

    /*------ region Public methods ------*/
    
 /**
 * Called when pagechange event occurs
 */
    onTablePageChange(e) {
        let start = this.filteredClientData.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.filteredClientData.length ? this.filteredClientData.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.filteredClientData.length;
    }
        
    // Get Edit Actions of ViewAll Clients page
    getScreenActions() {
        this._userService.getUserScreenActions(SCREEN_CODE.CLIENTVIEWALL)
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
     * To get all the sites.
     */
    getAllSites() {
        this._viewAllClientsService.GetAllSites().subscribe(
            data => {
                if (data !== null) {
                    this.siteOptions = [];
                    data.forEach(element => {
                        let model: ValueLabel;
                        model = {
                            label: element.value,
                            value: element.key
                        };
                        this.siteOptions.push(model);
                    });
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }
            }
        );
    }


    /**
     * Gets all clientdata
     */
    getClientsData() {
        this.dataLoading = true;
        this._viewAllClientsService.GetAllClients().subscribe(
            data => {
                if (data != null) {
                    this.client = data;
                    this.filteredClientData = [...this.client];
                    this.dataLoading = false;
                    let e = { first: 0, rows: this.rowsIn };
                    this.onTablePageChange(e);
                    jQuery(".ui-table-scrollable-body").scroll(function () {
                        jQuery(".ui-multiselect-panel.ui-widget.ui-widget-content.ui-corner-all.ui-shadow").css('display', 'none');
                        jQuery(".ui-table-scrollable-body").trigger("click");
                    });
                 } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_SUCCESS, summary: '', detail: SHARED.NO_DATA });
                }
            },
            err => {
                this.msgs = [];
                this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
            },
        );
     }
        
    // Gets selected row in the grid
    onRowSelect(rowData) {
        if (rowData.status === this.labels.ACTIVE) {
            this._globalEventsManager.setClientDropdown(true);
            this._globalEventsManager.setGlobalClientCode({ label: rowData.clientName, value: rowData.clientCode });
            this._globalEventsManager.setClientMode('edit');
        } else if (rowData.status === this.labels.INACTIVE) {
            this._globalEventsManager.setClientCode({ label: rowData.clientName, value: rowData.clientCode });
            this._globalEventsManager.setClientMode('editInactive');
        } else {
            this._globalEventsManager.setClientCode({ label: rowData.clientName, value: rowData.clientCode });
            this._globalEventsManager.setClientMode('editPartial');
        }
        this.router.navigateByUrl('/client');
    }

    /**
     * Redirect to client history.
     * @param client
     */
    redirectToClientHistory(client) {
        this.saveInLocal(this.labels.CLIENT_CODE_PROPERTY, client.clientCode);
        this.saveInLocal(this.labels.CLIENT_NAME_PROPERTY, client.clientName);
        this.router.navigateByUrl('/client/client-history');
    }

    /**
     * save the data in local storage.
     * @param key
     * @param val
     */
    saveInLocal(key, val): void {
        this.storage.set(key, val);
        this.localStorageData[key] = this.storage.get(key);
    }

    exportToExcel() {
        this.exportAsExcelFile();        
    }

    public exportAsExcelFile(): void {
        this.filteredClientData.forEach(function (v) { delete v.clientId });
        if (!this.editActions.includes('ViewActualM3Revenue')) {
            this.filteredClientData.forEach(function (v) { delete v.actualM3Revenue });
        }
        if (!this.editActions.includes('ViewForecastedM3Revenue')) {
            this.filteredClientData.forEach(function (v) { delete v.forecastedM3Revenue });
        }
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.filteredClientData);
        ws.A1.v = this.labels.CLIENT_CODE;
        ws.B1.v = this.labels.CLIENT_NAME;
        ws.C1.v = this.labels.SITE;
        ws.D1.v = this.labels.BILLING_MANAGER;
        ws.E1.v = this.labels.RELATIONSHIP_MANAGER;
        ws.F1.v = this.labels.MTD_DEPOSIT;
        ws.G1.v = this.labels.MTD_TARGET;
        ws.H1.v = this.labels.PROJECTED_CASH;
        ws.I1.v = this.labels.MONTHLY_TARGET;
        if (this.editActions.includes('ViewActualM3Revenue')) {
            ws.J1.v = this.labels.ACTUAL_M3_REVENUE;
            if (this.editActions.includes('ViewForecastedM3Revenue')) {
                ws.K1.v = this.labels.FORECASTED_M3_REVENUE;
                ws.L1.v = this.labels.STATUS;
            } else {
                ws.K1.v = this.labels.STATUS;
            }
        } else if (this.editActions.includes('ViewForecastedM3Revenue')) {
            ws.J1.v = this.labels.FORECASTED_M3_REVENUE;
            ws.K1.v = this.labels.STATUS;
        }
        else {
            ws.J1.v = this.labels.STATUS;
        }        

        /* generate workbook and add the worksheet */
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Clients');

        /* save to file */
        XLSX.writeFile(wb, this.labels.EXCEL_FILE_NAME + '_' + moment(new Date()).format('MMDDYYYYhhmmss')+'.xlsx');       
    }

     /**
     * Called when filter event occurs
     */
  onTableFilter(event) {
        this.filteredClientData = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };
        this.onTablePageChange(e);
    }

    /*------ end region Public methods ------*/

}
    
