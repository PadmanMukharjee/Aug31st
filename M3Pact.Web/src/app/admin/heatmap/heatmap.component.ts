import { Component, OnInit } from '@angular/core';
import { DualListComponent } from 'angular-dual-listbox';
import { Message } from 'primeng/components/common/api';
import { HeatmapService } from './heatmap.service';
import { Observable } from 'rxjs/Observable';
import { HeatMapItemModel } from './HeatMapItemModel';
import { HEATMAP_SETUP } from '../../shared/utilities/resources/labels';

@Component({
    selector: 'app-heatmap',
    templateUrl: './heatmap.component.html',
    styleUrls: ['./heatmap.component.css']
})
export class HeatmapComponent implements OnInit {

    /*------ region Public properties ------*/

    universalChecklistItems: HeatMapItemModel[];
    universalM3Metrics: any[];
    heatMapItemsFromChecklist: any[];
    heatMapItemsFromMetrics: any[];
    pageTitle: string;
    format: any = DualListComponent.DEFAULT_FORMAT;
    keepSorted = true;
    public msgs: Message[] = [];
    key: string;
    display: string;
    labels = HEATMAP_SETUP;
    displayDialog = false;
    dialogHeader: string;
    dialogueMsg: string;
    updatedHeatMapChecklist: HeatMapItemModel[];
    updatedHeatMapMetric: HeatMapItemModel[];

    /*------ end region Public properties ------*/


    /*------ region constructor ------*/

    constructor(private _heatMapService: HeatmapService) {
    }
    /*------ end region constructor ------*/

    /*-----region lifecycle events -----*/
    ngOnInit() {
        this.universalChecklistItems = new Array<HeatMapItemModel>();
        this.universalM3Metrics = new Array<HeatMapItemModel>();
        this.heatMapItemsFromChecklist = new Array<HeatMapItemModel>();
        this.heatMapItemsFromMetrics = new Array<HeatMapItemModel>();
        this.pageTitle = this.labels.PAGE_TITLE;
        this.format = {
            add: 'Add', remove: 'Remove', all: 'All', none: 'None', draggable: true, locale: 'en'
        };
        const kpiItems = this._heatMapService.getKpisforHeatMap();
        const heatMapItems = this._heatMapService.getHeatMapItems();
        this.key = 'KpiId';
        this.display = 'KpiDescription';
        Observable.forkJoin(kpiItems, heatMapItems)
            .subscribe(
            ([kpiItems, heatMapItems]) => {
                if (kpiItems && kpiItems.length > 0) {
                    for (let i in kpiItems) {
                        if (kpiItems[i].checklistType == this.labels.WEEK || kpiItems[i].checklistType == this.labels.MONTH) {
                            let universalChecklist = new HeatMapItemModel();
                            universalChecklist.ChecklistType = kpiItems[i].checklistType;
                            universalChecklist.KpiDescription = kpiItems[i].kpiDescription;
                            universalChecklist.KpiId = kpiItems[i].kpiId;
                            this.universalChecklistItems.push(universalChecklist);
                        } else {
                            let m3Metric = new HeatMapItemModel();
                            m3Metric.ChecklistType = kpiItems[i].checklistType;
                            m3Metric.KpiDescription = kpiItems[i].kpiDescription;
                            m3Metric.KpiId = kpiItems[i].kpiId;
                            this.universalM3Metrics.push(m3Metric);
                        }
                    }
                }
                if (heatMapItems && heatMapItems.length > 0) {
                    for (let i in heatMapItems) {
                        if (heatMapItems[i].checklistType == this.labels.WEEK || heatMapItems[i].checklistType == this.labels.MONTH) {
                            let universalChecklist = new HeatMapItemModel();
                            universalChecklist.ChecklistType = heatMapItems[i].checklistType;
                            universalChecklist.KpiDescription = heatMapItems[i].kpiDescription;
                            universalChecklist.KpiId = heatMapItems[i].kpiId;
                            this.heatMapItemsFromChecklist.push(universalChecklist);
                        } else {
                            let m3Metric = new HeatMapItemModel();
                            m3Metric.ChecklistType = heatMapItems[i].checklistType;
                            m3Metric.KpiDescription = heatMapItems[i].kpiDescription;
                            m3Metric.KpiId = heatMapItems[i].kpiId;
                            this.heatMapItemsFromMetrics.push(m3Metric);
                        }
                    }
                    this.updatedHeatMapChecklist = [ ...this.heatMapItemsFromChecklist];
                    this.updatedHeatMapMetric = [...this.heatMapItemsFromMetrics];
                }
            });

    }
    /*-----end region lifecycle events -----*/


    /*------ region public methods ------*/

    /**
     * To save HeatMap Items
     */
    saveHeatMapItems() {
        if (this.heatMapItemsFromChecklist.length == this.labels.HEATMAP_LENGTH && this.heatMapItemsFromMetrics.length == this.labels.HEATMAP_LENGTH) {
            let CombinedHeatMap: HeatMapItemModel[] = this.heatMapItemsFromChecklist.concat(this.heatMapItemsFromMetrics);
            this._heatMapService.saveHeatMapItems(CombinedHeatMap).subscribe(
                (data) => {
                    if (data == true) {
                        this.displayDialog = true;
                        this.dialogHeader = this.labels.SEVERITY.SUCCESS;
                        this.dialogueMsg = this.labels.MESSAGES.SAVED;
                    } else {
                        this.displayDialog = true;
                        this.dialogHeader = this.labels.SEVERITY.ERROR;
                        this.dialogueMsg = this.labels.MESSAGES.ERROR;
                    }
                }
            );

        } else {
            this.displayDialog = true;
            this.dialogHeader = this.labels.SEVERITY.ERROR;
            this.dialogueMsg = this.labels.MESSAGES.FIVE_ITEMS;
        }
    }

    /**
    * closes the dialog on clicking ok button
    **/
    displayDialogClose() {
        this.displayDialog = false;
    }

    /**
     * When Risk factors Heat map items changes this method will be called
     * @param e
     */
    onDestinationChange(e) {
        if (e.length > 5) {
            this.heatMapItemsFromChecklist = [...this.updatedHeatMapChecklist];
            this.displayDialog = true;
            this.dialogHeader = this.labels.SEVERITY.ERROR;
            this.dialogueMsg = this.labels.MESSAGES.FIVE_ITEMS;
        } else {
            this.updatedHeatMapChecklist = [...this.heatMapItemsFromChecklist];
        }
    }

    /**
     * When metrics Heat map items changes this method will be called
     * @param e
     */
    onMetricDestinationChange(e) {
        if (e.length > 5) {
            this.heatMapItemsFromMetrics = [...this.updatedHeatMapMetric];
            this.displayDialog = true;
            this.dialogHeader = this.labels.SEVERITY.ERROR;
            this.dialogueMsg = this.labels.MESSAGES.FIVE_ITEMS;
        } else {
            this.updatedHeatMapMetric = [...this.heatMapItemsFromMetrics];
        }

    }
    /*------ end region public methods ------*/

}
