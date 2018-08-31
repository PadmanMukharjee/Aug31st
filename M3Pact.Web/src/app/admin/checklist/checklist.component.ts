import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { DualListComponent } from 'angular-dual-listbox';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/forkJoin';
import { FormBuilder, FormGroup, FormArray, Validators, AbstractControl } from '@angular/forms';


import { ADMIN_CHECKLIST, SHARED } from '../../shared/utilities/resources/labels';
import { CheckListItem } from '../models/checklist-item.model';
import { CheckListService } from '../services/checklist.service';
import { CheckList } from '../models/checklist.model';
import { ValueLabel } from '../models/checklist-item-table.model';
import { SystemsService } from '../../admin/systems/systems.service';
import { SitesService } from '../../admin/sites/sites.service';
import { Message } from 'primeng/components/common/api';

import * as _ from 'lodash';

@Component({
    selector: 'app-checklist',
    templateUrl: './checklist.component.html',
    styleUrls: ['./checklist.component.css'],
    providers: [SystemsService, SitesService],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChecklistComponent implements OnInit, OnDestroy {

    /*------ region Public properties ------*/
    public format: any = DualListComponent.DEFAULT_FORMAT;
    public keepSorted = true;
    public key: string;
    public display: any;
    public filter = true;
    public disabled = false;
    public labels = ADMIN_CHECKLIST;
    public allChecklistItems: CheckListItem[];
    public selectedChecklistItems: CheckListItem[] = [];
    public filteredUniversal: CheckListItem[] = [];
    public isEdit = false;
    public siteOptions: Array<ValueLabel> = [];
    public systemOptions: Array<ValueLabel> = [];
    public checklistTypeOptions: Array<ValueLabel> = [];
    public defaultChecklistType: string;
    public msgs: Message[] = [];
    public msg = '';
    public showErrorMsg = false;
    /*------ end region Public properties ------*/

    /*------ region Private properties ------*/
    private shallowChecklistItems: CheckListItem[] = [];
    private _editModel: CheckList = { checklistId: 0, checklistItems: [], checklistType: 1, name: '', sites: [], systems: [] };
    /*------ end region Private properties ------*/

    checklistForm = this.fb.group({
        checklistId: [0,
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
        name: ['',
            {
                validators: [
                    Validators.required,
                ],
                updateOn: 'blur'
            }
        ],
        systems: [[],
        {
            validators: [
                Validators.required,
            ]
        }],
        sites: [[], {
            validators: [
                Validators.required,
            ]
        }],
        checklistType: [1],
        checklistItems: [
            [],
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
    });

    /*------ region constructor ------*/
    constructor(private _checkListService: CheckListService,
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private cdref: ChangeDetectorRef,
        private _systemService: SystemsService,
        private _sitesService: SitesService
    ) {
        this.allChecklistItems = new Array<CheckListItem>();
    }
    /*------ end region constructor ------*/

    /*-----region lifecycle events -----*/
    ngOnInit() {
        this.defaultChecklistType = this.labels.WEEK;
        this.format = {
            add: 'Add', remove: 'Remove', all: 'All', none: 'None', draggable: true, locale: 'en'
        };

        this.route.data.subscribe((data) => {
            if (data && data.item && data.item['checklists']) {
                const checklists = data.item['checklists'];
                let checklistItems = checklists.checklistItems.map(t => { return { questionCode: t.key }; });
                let model: CheckList = {
                    checklistId: checklists.checklistId,
                    name: checklists.name,
                    sites: checklists.sites.map(t => t.key),
                    systems: checklists.systems.map(t => t.key),
                    checklistType: checklists.checklistType.key,
                    checklistItems: checklistItems.slice(0)
                };
                this.defaultChecklistType = this.getChecklistTypeName(model.checklistType);
                this.selectedChecklistItems = checklistItems;
                this.isEdit = true;
                this._editModel = model;
            } else {
                this.isEdit = false;
            }
        });

        this.loadPrepopulatedControls();
    }

    ngOnDestroy() {
        this.cdref.detach();
    }
    /*-----end region lifecycle events -----*/


    /*------ region public methods ------*/

    /**
     * prepopulates all controls
     */
    loadPrepopulatedControls() {
        const checklistItems = this._checkListService.getCheckListItems(this.defaultChecklistType);
        const systems = this._systemService.getAllSystems();
        const sites = this._sitesService.getSites(true);
        const checklistTypes = this._checkListService.getChecklistTypes();

        Observable
            .forkJoin(checklistItems, systems, sites, checklistTypes)
            .subscribe(([checkitems, system, site, checklistType]) => {
                if (checkitems && checkitems.length) {
                    this.segregateUniversalAndKPIQuestions(checkitems);
                }

                this.siteOptions = [];
                if (site && site.length) {
                    site.forEach(element => {
                        let model: ValueLabel;
                        model = {
                            label: element.siteName,
                            value: element.siteId
                        };
                        this.siteOptions.push(model);
                    });
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }

                if (system && system.length) {
                    system.forEach(element => {
                        let model: ValueLabel;
                        model = {
                            label: element.systemName,
                            value: element.id
                        };
                        this.systemOptions.push(model);
                    });
                }

                this.checklistTypeOptions = [];
                if (checklistType && checklistType.length) {
                    checklistType.forEach(element => {
                        let model: ValueLabel;
                        model = {
                            label: element.value,
                            value: element.key
                        };
                        this.checklistTypeOptions.push(model);
                    });
                }

                if (this.isEdit) {
                    this.selectedChecklistItems =
                        this.selectedChecklistItems.filter(t => t.question && t.question.length);
                    this.shallowChecklistItems = this.selectedChecklistItems.slice(0);
                    let sites = Object.keys(this.siteOptions).map((k) => this.siteOptions[k].value.toString());
                    this._editModel.sites = sites.filter(
                        function (e) {
                            return this.indexOf(e) >= 0;
                        },
                        this._editModel.sites.toString()
                    );
                    let systems = Object.keys(this.systemOptions).map((k) => this.systemOptions[k].value.toString());
                    this._editModel.systems = systems.filter(
                        function (e) {
                            return this.indexOf(e) >= 0;
                        },
                        this._editModel.systems.toString()
                    );
                    this.checklistForm.setValue(this._editModel);
                }
                this.cdref.markForCheck();
            });
    }

    /**
      * which acts as a helper method to check the required validation
      * @param  {string} name
      * @returns boolean
      */
    required(name: string) {
        return (
            this.checklistForm.get(`${name}`).hasError('required') &&
            this.checklistForm.get(`${name}`).touched
        );
    }


    /**
     * to submit the form on error free
     */
    onChecklistSubmit(event) {
        let checklistContainsItems = this.checklistForm.get('checklistItems').value;
        if (checklistContainsItems && checklistContainsItems.length) {
            let finalvalues = _.uniqBy(this.filteredUniversal.concat(this.selectedChecklistItems), function (e: CheckListItem) {
                return e.questionCode;
            });
            this.checklistForm.get('checklistItems').setValue(finalvalues);
        } else {
            this.checklistForm.get('checklistItems').setValue(this.selectedChecklistItems);
        }

        if (this.checklistForm.valid) {
            this._checkListService.saveCheckList(this.checklistForm.value).subscribe(s => {
                if (s != null) {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_SUCCESS, summary: '', detail: SHARED.SUCCESS_MESSAGE });
                    if (!this.isEdit) {
                        this.resetChecklistForm();
                    }
                    this.cdref.markForCheck();
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_SAVE_MESSAGE });
                    this.cdref.markForCheck();
                }
            }
            );
        } else {
            this.checklistForm.get('name').markAsTouched();
            this.checklistForm.get('systems').markAsTouched();
            this.checklistForm.get('sites').markAsTouched();
            this.checklistForm.get('checklistType').markAsTouched();
        }
    }

    /**
     * to reset a checklist form
     */
    public resetChecklistForm() {
        if (this.isEdit) {
            this.checklistForm.setValue(this._editModel);
            this.selectedChecklistItems = this.shallowChecklistItems.slice(0);
        } else {
            this.initializeChecklistForm();
        }
    }

    /**
     * to alert if any client exist for the removed system from a checklist
     * @param  {} event
     */
    systemChange(event) {
        if (this.isEdit) {
            let removedValue = event.itemValue;
            this._checkListService.checkClientOnSystemDelete(event.itemValue, this.checklistForm.get('checklistId').value).subscribe(
                r => {
                    if (r.dependent) {
                        this.showErrorMsg = true;
                        this.msg = this.labels.CLIENT_SYSTEM_ASSOCIATION_ERROR;
                        let control = this.checklistForm.get('systems');
                        let value = [].concat(control.value);
                        value.push(removedValue);
                        control.setValue(value);
                        this.cdref.markForCheck();
                    }
                }
            );
        }
    }

    /**
     * to alert if any client exist for the removed site from a checklist
     * @param  {} event prime ng multi select event
     */
    siteChange(event) {
        if (this.isEdit) {
            let removedValue = event.itemValue;
            this._checkListService.checkClientOnSiteDelete(event.itemValue, this.checklistForm.get('checklistId').value).subscribe(
                r => {
                    if (r.dependent) {
                        this.showErrorMsg = true;
                        this.msg = this.labels.CLIENT_SITE_ASSOCIATION_ERROR;
                        let control = this.checklistForm.get('sites');
                        let value = [].concat(control.value);
                        value.push(removedValue);
                        control.setValue(value);
                        this.cdref.markForCheck();
                    }
                }
            );
        }
    }

    // Triggered on change of Checklist Type
    checklistTypeChange(event) {
        let checklistType = this.getChecklistTypeName(event.value);
        this._checkListService.getCheckListItems(checklistType).subscribe((items) => {
            if (items && items.length) {
                this.segregateUniversalAndKPIQuestions(items);
            }
        });
    }

    // Universal & Non Universal KPIs
    segregateUniversalAndKPIQuestions(checklistItems) {
        if (checklistItems && checklistItems.length) {
            this.allChecklistItems = [];
            this.filteredUniversal = [];
            checklistItems.forEach(t => {
                if (!t.universal) {
                    this.allChecklistItems.push(t);
                    let foundlistitem = this.checkSelectedChecklistItemsHasUniversalKpi(t);
                    if (foundlistitem >= 0) {
                        this.selectedChecklistItems[foundlistitem] = t;
                    }
                } else {
                    this.filteredUniversal.push(t);
                }
            });
            this.key = 'questionCode';
            this.display = this.checklistLabel;
            if (!this.isEdit) {
                this.checklistForm.get('checklistItems').setValue(this.filteredUniversal);
            }
            this.cdref.markForCheck();
        }
    }

    // Get ChecklistType Name from ID
    getChecklistTypeName(checklistTypeID) {
        let checklistType = this.labels.WEEK;
        if (checklistTypeID == this.labels.MONTH_CHECKLIST_ID) {
            checklistType = this.labels.MONTH;
        }
        return checklistType;
    }

    /*------ end region public methods ------*/

    /*------ region private methods ------*/

    private initializeChecklistForm() {
        this.checklistForm.reset();
        let model: CheckList = {
            checklistId: 0,
            name: '',
            sites: [],
            systems: [],
            checklistType: 1,
            checklistItems: []
        };
        this.checklistForm.setValue(model);
        this.checklistForm.get('checklistItems').setValue(this.filteredUniversal);
        this.checklistTypeChange(model.checklistType);
        this.selectedChecklistItems = [];
    }

    /**
    * to get the index of checklist item from the list of selected checklist
    * @param  {CheckListItem} checkItem
    * @returns number
    */
    private checkSelectedChecklistItemsHasUniversalKpi(checkItem: CheckListItem): number {
        let foundIndex = -1;
        this.selectedChecklistItems.some((t, index) => {
            if (t.questionCode == checkItem.questionCode) {
                foundIndex = index;
            }
            return t.questionCode == checkItem.questionCode;
        });
        return foundIndex;
    }

    private checklistLabel(checklistItem: CheckListItem) {
        return checklistItem.question;
    }

    /*------ end region private methods ------*/
}
