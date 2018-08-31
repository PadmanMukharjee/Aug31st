// Angular imports
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import * as _ from 'lodash';
import { CurrencyPipe } from '@angular/common';

// Common File Imports
import { ADMIN_KPI, SHARED, } from '../../../shared/utilities/resources/labels';
import { SCREEN_CODE } from '../../../shared/utilities/resources/screencode';
import { Router } from '@angular/router';
import { KPIEditViewModel } from './view-kpi.model';
import { KPIService } from '../create-kpi/kpi.service';
import { UserService } from '../../../shared/services/user.service';
import { Message } from 'primeng/components/common/api';

@Component({
    selector: 'app-kpi',
    templateUrl: './view-kpi.component.html',
    providers: [KPIService, CurrencyPipe],

})

export class ViewKPIComponent implements OnInit {

    /*------ region Public properties ------*/

    public kpis: KPIEditViewModel[];
    public kpi: KPIEditViewModel;
    public labels = ADMIN_KPI;
    public cols: any[];
    public yesText: string;
    public noText: string;
    public standard: string;
    public canEdit = false;
    public yesNoOptions: any;
    public messages: Message[] = [];
    public paginationCount: any;
    public rowsIn: number = 10;
    public sharedLabels = SHARED;
    public kpifiltered: any;
    /*------ end region Public properties ------*/


    /*------ region constructor ------*/
    constructor(private kpiService: KPIService, private router: Router, private _userService: UserService, private currencyPipe: CurrencyPipe) {
    }
    /*------ end region constructor ------*/

    /*-----region lifecycle events -----*/

    ngOnInit() {
        this.getScreenActions();
        this.getKpi();
        this.yesText = 'Yes';
        this.noText = 'No';
        this.yesNoOptions = [
            { label: 'No', value: false },
            { label: 'Yes', value: true }
        ];
    }
    /*-----end region lifecycle events -----*/


    /*------ region public methods ------*/

    /**
     * Called when pagechange event occurs
     */
    onTablePageChange(e) {
        let start = this.kpifiltered.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.kpifiltered.length ? this.kpifiltered.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.kpifiltered.length;
    }
    /*
     * Getting action based on role.
     * */
    getScreenActions() {
        this._userService.getUserScreenActions(SCREEN_CODE.VIEWEDITKPI)
            .subscribe((actions) => {
                if (actions) {
                    if (actions.length === 0) {
                        this.canEdit = false;
                    } else {
                        this.canEdit = true;
                    }
                }
            });
    }

    /**
     * Getting all KPI's
     */
    getKpi() {
        this.kpiService.getAllM3MetricsKPIs().subscribe(
            (response: any) => {
                if (response != null) {
                    let body = [];
                    response.forEach(e => {
                        let g: KPIEditViewModel = {
                            id: e.kpiid,
                            kpiDescription: e.kpiDescription,
                            measure: e.measure.measureText,
                            standard: this.formStandard(e.measure.measureUnit, e.alertLevel),
                            isHeatMapItem: { text: (e.isHeatMapItem ? 'Yes' : 'No'), value: e.isHeatMapItem },
                            sendAlert: { text: (e.sendAlert ? 'Yes' : 'No'), value: e.sendAlert },
                            escalateAlert: { text: (e.escalateAlert ? 'Yes' : 'No'), value: e.escalateAlert },
                            isUniversal: { text: (e.isUniversal ? 'Yes' : 'No'), value: e.isUniversal }
                        };
                        body.push(g);
                    });
                    this.kpis = (_.sortBy(body, 'id')).reverse();
                    this.kpifiltered = [... this.kpis];
                } else {
                    this.messages = [];
                    this.messages.push({ severity: this.sharedLabels.SEVERITY_ERROR, summary: '', detail: this.sharedLabels.ERROR_GET_DETAILS });
                }
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            }
        );
    }

    formStandard(measureUnit: string, alertLevel: string) {
        let measureStandard;
        if (measureUnit === this.labels.KPI_UNIT_PERCENTAGE) {
            let standard = alertLevel.split(',');
            measureStandard = standard[0] + ' ' + ((parseFloat(standard[1]) * 100).toFixed(3).slice(0, -1)).toString().replace(',', ' ');
            measureStandard += '%';
        } else if (measureUnit === this.labels.KPI_UNIT_AMOUNT) {
            let standard = alertLevel.split(',');
            measureStandard = standard[0] + ' ' + this.transformAmountToCurrency(parseFloat(standard[1])).toString().replace(',', ' ');
        } else {
            measureStandard = alertLevel.replace(',', ' ');
        }
        return measureStandard;
    }
    /**
     * Redirecting to create kpi page.
    **/
    onRowSelect(kpi: KPIEditViewModel) {
        this.kpi = new KPIEditViewModel();
        this.router.navigateByUrl('/administration/kpi/createKpi?kpiID=' + kpi.id);
    }

    transformAmountToCurrency(amount: any) {
        return this.currencyPipe.transform(amount, 'USD');
    }

    /**
    * Called when filter event occurs
    */
    onTableFilter(event) {
        this.kpifiltered = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };
        this.onTablePageChange(e);
    }

    /*------ end region public methods ------*/
}
