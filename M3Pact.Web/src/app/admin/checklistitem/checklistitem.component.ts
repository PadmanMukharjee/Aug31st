import { Component, OnInit, ChangeDetectionStrategy, AfterViewInit, ChangeDetectorRef, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, AbstractControl, FormControl } from '@angular/forms';

import { ADMIN_CHECKLIST, ADMIN_KPI, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { CheckListItem } from '../models/checklist-item.model';
import { CheckListService } from '../services/checklist.service';
import { KPIService } from '../kpi/create-kpi/kpi.service';

import { CheckListType } from '../models/checklist-type.model';
import { ValueLabel } from '../models/checklist-item-table.model';
import { KPIViewModel } from '../kpi/create-kpi/kpi.model';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';

import { CheckListItemTable } from '../models/checklist-item-table.model';
import { UserService } from '../../shared/services/user.service';
import { Observable } from 'rxjs/Rx';
import { Router } from '@angular/router';

import * as _ from 'lodash';

@Component({
  selector: 'app-checklistitem',
  templateUrl: './checklistitem.component.html',
  styleUrls: ['./checklistitem.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [KPIService]
})
export class ChecklistitemComponent implements OnInit, OnDestroy {

    /*------ region Public properties ------*/

    public labels = ADMIN_CHECKLIST;
    public kpiLabels = ADMIN_KPI;
    public sharedLabels = ADMIN_SHARED;
    public checkListItem: CheckListItem;
    public checkListItems: CheckListItem[];
    questionForm = this.fb.group({
        questionID: [0,
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
        question: ['',
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
        expectedResponse: [false,
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
        kpi: [false,
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
        universal: [false],
        freeform: [false,
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
        checklistType: [
            '',
            {
                validators: [
                    Validators.required,
                ]
            }
        ],
        kpiDescription: [''],
        sendAlert: [false],
        escalateAlert: [false],
        relationshipManager: [false],
        billingManager: [false],
        after: ['']
    });

    public tableBody: Array<any> = [];
    public isEditPage = false;

    public canEdit = false;
    public editActions: any[] = [];
    public checklistTypeOptions: Array<ValueLabel> = [];
    public afterText: string;
    public isEdit = false;
    public isSaveClicked: boolean;
    public sendToErrorMsg: string;
    public validationConstants = VALIDATION_MESSAGES;
    public ifError: boolean;
    public displayDialog: boolean;
    public dialogueMsg: string;
    public displayLabel: string;
    public ifQuestionExists: any;
    public isKPIDescription: any;
    public yesNoOptions: any;
    public Options: any;
    public saveCall = false;
    public checklistHeatMapItems: Array<string> = [];
    public heatMapMessage: string;
    public displayHeatMapMessage = false;
    public editedChecklistItemCode: string;
    public showDialogMessage = false;
    public filterCheckListItem: any[];
    public paginationCount: any;
    public rowsIn: number = 10;
  
    /*------ end region Public properties ------*/

    @ViewChild('qtn')

    /*------ region Private properties ------*/
    private questionControl;

    /*------ end region Private properties ------*/

    /*------ region constructor ------*/
  constructor(
      private fb: FormBuilder,
      private service: CheckListService,
      private cdref: ChangeDetectorRef,
      private userService: UserService,
      private _checkListService: CheckListService,
      private router: Router,
      private _kpiService: KPIService
  ) {
      this.router.routeReuseStrategy.shouldReuseRoute = function () {
          return false;
      };
  }
    /*------ end region constructor ------*/

    /*-----region lifecycle events -----*/

    ngOnInit() {
        this.checkListItem = new CheckListItem;
        this.checkListItem.kpiDescription = new KPIViewModel();
        this.checkListItem.checkListType = new CheckListType();
        this.yesNoOptions = [
            { label: 'No', value: false },
            { label: 'Yes', value: true }
        ];
        this.Options = [
            { label: 'No', value: false },
            { label: 'Yes', value: true },
            { label: 'N/A', value: 'N/A'}
        ];
        this.service.getCheckListItems().subscribe((res) => {
          if (res && res.length) {
            let body = [];
            this.checkListItems = res;
            res.forEach(e => {
              let g: CheckListItemTable = {
                  id: e.questionID,
                  question: e.question,
                  checklistType: { label: e.checkListType.checkListTypeName, value: e.checkListType.checkListTypeID.toString() },
                  expectedResponse: { text: (e.expectedResponse ? 'Yes' : 'No'), value: e.expectedResponse },
                  KPI: { text: (e.kpi ? 'Yes' : 'No'), value: e.kpi },
                  universal: { text: (e.universal ? 'Yes' : 'No'), value: e.universal },
                  freeform: { text: (e.freeform ? 'Yes' : 'No'), value: e.freeform },
                  kpiDescription: e.kpi ? e.kpiDescription.kpiDescription : 'N/A',
                  sendAlert: e.kpi ? { text: (e.kpiDescription.sendAlert ? 'Yes' : 'No'), value: e.kpiDescription.sendAlert } : { text: 'N/A', value: 'N/A'},
                  escalateAlert: e.kpi ? { text: (e.kpiDescription.escalateAlert ? 'Yes' : 'No'), value: e.kpiDescription.escalateAlert } : { text: 'N/A', value: 'N/A' }
                };

              body.push(g);
            });
              this.tableBody = (_.sortBy(body, 'id')).reverse();

            this.cdref.markForCheck();
          }
            this.filterCheckListItem = this.checkListItems;

            let e = { first: 0, rows: this.rowsIn };
            this.onTablePageChange(e);
        }, (error) => {

            }, () => { });
        this.questionForm.get('kpi').valueChanges.subscribe(e => {
        if (e) {
            this.questionForm.get('kpiDescription').setValidators([Validators.required]);
        } else {
            this.resetProperties(1);
            this.questionForm.get('kpiDescription').clearValidators();
        }
        this.questionForm.get('kpiDescription').updateValueAndValidity();
    });
        this.questionForm.get('sendAlert').valueChanges.subscribe(e => {
            if (!e) {
                this.resetProperties(2);
            }
        });
        this.questionForm.get('escalateAlert').valueChanges.subscribe(e => {
            if (e) {
                this.questionForm.get('after').setValidators([Validators.required]);
            } else {
                this.resetProperties(3);
                this.questionForm.get('after').clearValidators();
            }
            this.questionForm.get('after').updateValueAndValidity();
        });

        this.loadPreLoadControllers();
    }

    ngOnDestroy() {
        this.cdref.detach();
    }

    /*-----end region lifecycle events -----*/


    /*------ region public methods ------*/

    /*
     * Checking white space
     * */
    public noWhitespaceValidator(control: FormControl) {
        let isWhitespace = (control.value || '').trim().length === 0;
        let isValid = !isWhitespace;
        return isValid ? null : { 'whitespace': true };
    }

    resetProperties(resetLevel: number) {
        if (resetLevel == 1) {
            this.questionForm.get('kpiDescription').setValue('');
            this.questionForm.get('universal').setValue(false);
            this.questionForm.get('sendAlert').setValue(false);
        } else if (resetLevel == 2) {
            this.questionForm.get('escalateAlert').setValue(false);
            this.questionForm.get('relationshipManager').setValue(false);
            this.questionForm.get('billingManager').setValue(false);
        } else {
            this.questionForm.get('after').setValue('');
        }
    }

    /**
 * Called when pagechange event occurs
 */
    onTablePageChange(e) {
        let start = this.filterCheckListItem.length > 0 ? e.first + 1 : e.first;
        let end = (e.first + e.rows) > this.filterCheckListItem.length ? this.filterCheckListItem.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.filterCheckListItem.length;
    }

    /**
    * Called when filter event occurs
    */
    onTableFilter(event) {
        this.filterCheckListItem = event.filteredValue;
        let e = { first: 0, rows: this.rowsIn };
        this.onTablePageChange(e);
    }
    onChecklistTypeSelect() {
        if (this.questionForm.get('checklistType').value == 1) {
            this.afterText = 'weeks';
        } else {
            this.afterText = 'months';
        }
    }

    loadPreLoadControllers() {
        const checklistTypes = this._checkListService.getChecklistTypes();

        Observable
            .forkJoin(checklistTypes)
            .subscribe(([checklistType]) => {
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
                this.cdref.markForCheck();
            });
    }

    /*
     * Removing validation of Question text box on focus
     * */
    removeValidationMessage() {
        this.ifQuestionExists = null;
        this.questionForm.get('question').setValidators([this.noWhitespaceValidator]);
        this.questionForm.get('question').updateValueAndValidity();
    }

    /*
     * Removing validation of KPI description text box on focus
     * */
    removeKPIDescriptionValidationMessage() {
        this.isKPIDescription = false;
        this.questionForm.get('kpiDescription').setValidators([this.noWhitespaceValidator]);
        this.questionForm.get('kpiDescription').updateValueAndValidity();
    }

    editItem(item) {
        this.ifQuestionExists = null;
        this.checkListItem = new CheckListItem();
        this.checkListItem.kpiDescription = new KPIViewModel();
        this.checkListItem.checkListType = new CheckListType();
        this.checkListItem = this.checkListItems.find(x => x.questionID == item.id);
        this.editedChecklistItemCode = this.checkListItem.questionCode;
        this.questionForm.get('questionID').setValue(item.id);
        this.questionForm.get('question').setValue(item.question);
        this.questionForm.get('expectedResponse').setValue(item.expectedResponse.value);
        this.questionForm.get('kpi').setValue(item.KPI.value);
        this.questionForm.get('freeform').setValue(item.freeform.value);
        this.questionForm.get('checklistType').setValue(this.checkListItem.checkListType.checkListTypeID);
        if (item.KPI.value) {
            let kpi = this.checkListItem.kpiDescription;
            this.questionForm.get('universal').setValue(item.universal.value);
            this.questionForm.get('kpiDescription').setValue(kpi.kpiDescription);
            this.questionForm.get('sendAlert').setValue(kpi.sendAlert);
            if (kpi.sendAlert) {
                this.questionForm.get('escalateAlert').setValue(kpi.escalateAlert);
                this.questionForm.get('relationshipManager').setValue(kpi.sendToRelationshipManager);
                this.questionForm.get('billingManager').setValue(kpi.sendToBillingManager);
                if (kpi.escalateAlert) {
                    let triggerTime = kpi.escalateTriggerTime.split(',');
                    this.afterText = triggerTime[1];
                    this.questionForm.get('after').setValue(triggerTime[0]);
                }
            }
        }
        if (!item.KPI.value || !item.sendAlert.value || !item.escalateAlert.value) {
            this.onChecklistTypeSelect();
            this.questionForm.get('after').reset();
        }

    this.isEditPage = true;
    this.questionControl.nativeElement.focus();
  }

  /**
   * which acts as a helper method to check the required validation
   * @param  {string} name
   * @returns boolean
   */
  required(name: string): boolean {
    return (
        this.questionForm.get(`${name}`).hasError('required') &&
        this.questionForm.get(`${name}`).touched ||
        this.questionForm.get(`${name}`).hasError('whitespace')
    );
  }

  /**
   * to strictly check the value and parse it to boolean if it is not
   * @param  {any} val
   * @returns boolean
   */
  parseBoolean(val): boolean {
    if (typeof val === 'boolean') {
      return !!val;
    } else if (typeof val === 'string') {
      return (val === 'true');
    }
    return false;
  }

    /*
     * Checking in db wheather kpi description already exits.
     * */
    ifKPIDescriptionExists() {
        this._kpiService.getAllKPIs().subscribe(
            (response: any) => {
                if (response) {
                    this.isKPIDescription = response.find(x => x.kpiDescription.toLocaleLowerCase() == this.questionForm.get('kpiDescription').value.toLocaleLowerCase().trim() && x.measure.measureId != this.questionForm.get('questionID').value);
                    if (this.isKPIDescription === null || this.isKPIDescription === undefined) {
                        if ((this.questionForm.get('universal').dirty || this.questionForm.get('kpi').dirty) && (!this.questionForm.get('universal').value || !this.questionForm.get('kpi').value)) {
                            this.saveChecklistItem();
                        } else {
                            this.save();
                        }
                    } else {
                        this.saveCall = false;
                    }
                } else {
                    this.saveCall = false;
                }
                this.cdref.markForCheck();
            });
    }

  /**
   * to save the question form by a checklist service call and update the table with the saved content
   */
    onChecklistItemSubmit() {
        this.isSaveClicked = true;
        this.saveCall = true;
        this.sendToSelection();
        if (this.checkListItems != null) {
            this.ifQuestionExists = this.checkListItems.find(x => x.checkListType.checkListTypeID == this.questionForm.get('checklistType').value &&
                x.question.toLocaleLowerCase() == this.questionForm.get('question').value.toLocaleLowerCase().trim() && x.questionID != this.questionForm.get('questionID').value);
        }
        if (this.ifQuestionExists === null || this.ifQuestionExists === undefined) {
            if (this.questionForm.get('kpi').value) {
                this.ifKPIDescriptionExists();
            } else if ((this.questionForm.get('universal').dirty || this.questionForm.get('kpi').dirty) && (!this.questionForm.get('universal').value || !this.questionForm.get('kpi').value)) {
                 this.saveChecklistItem();
            } else {
                 this.save();
            }
        } else {
            this.addingValidationToRequiredFields();
            this.saveCall = false;
        }
    }


    save() {
        if (this.questionForm.valid && this.questionForm.dirty && !this.ifError) {
            this.checkListItem = new CheckListItem;
            if (this.questionForm.get('questionID').value == 0) {
                this.checkListItem.questionID = 0;
            } else if (this.questionForm.get('questionID').value > 0) {
                this.checkListItem.questionID = this.questionForm.get('questionID').value;
                this.checkListItem.questionCode = this.editedChecklistItemCode;
            }
            if (!this.settingChecklistItem()) {
                this.service.saveCheckListItem(this.checkListItem).subscribe((response: any) => {
                    if (response!=null) {
                        this.displayDialog = true;
                        this.dialogueMsg = 'Checklist Item ' + (this.isEditPage ? this.sharedLabels.UPDATEMESSAGE : this.sharedLabels.SUCCESSMESSAGE);
                        this.showDialogMessage = true;
                        this.cdref.markForCheck();
                        if (!this.isEditPage) {
                            this.checkListItems.push(response);
                        }
                    } else {
                        this.displayDialog = true;
                        this.dialogueMsg = this.sharedLabels.ERROR_SAVE_MESSAGE;
                        this.showDialogMessage = true;
                        this.cdref.markForCheck();
                    }
                });
            } else {
                this.displayDialog = true;
                this.displayHeatMapMessage = true;
                this.heatMapMessage = this.labels.IS_HEATMAP_ITEM;
                this.cdref.markForCheck();
            }
        } else {
            this.addingValidationToRequiredFields();
        }
        this.saveCall = false;
    }

    /**
     * To get the heat map items - service call.
     */
    getHeatMapItems() {
        return Observable.forkJoin(
            this.service.getChecklistHeatMapItems()
        );
    }

    /**
     * Calling save method after getting heat map items.
     */
    saveChecklistItem() {
        this.getHeatMapItems().subscribe(
            data => {
                this.checklistHeatMapItems = data[0];
                this.filterCheckListItem = [...this.checklistHeatMapItems];
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
                this.save();
            });
    }

    /*
     * Adding validations to all required fields
     * */
    addingValidationToRequiredFields() {
        this.questionForm.get('questionID').markAsTouched();
        this.questionForm.get('kpiDescription').markAsTouched();
        this.questionForm.get('after').markAsTouched();
        this.questionForm.get('checklistType').markAsTouched();
    }

    /**
    * closes the dialog on clicking ok button
    **/
    displayDialogClose() {
        this.displayHeatMapMessage = false;
        this.displayDialog = false;
        this.showDialogMessage = false;
        this.router.navigateByUrl('/administration/checklistitems');
    }


    settingChecklistItem(): boolean {
        this.checkListItem.kpiDescription = new KPIViewModel();
        this.checkListItem.checkListType = new CheckListType();
        this.checkListItem.question = this.questionForm.get('question').value.trim();
        this.checkListItem.expectedResponse = this.questionForm.get('expectedResponse').value;
        this.checkListItem.kpi = this.questionForm.get('kpi').value;
        this.checkListItem.universal = this.questionForm.get('universal').value;
        this.checkListItem.freeform = this.questionForm.get('freeform').value;

        this.checkListItem.kpiDescription.kpiDescription = this.questionForm.get('kpiDescription').value.trim();
        this.checkListItem.kpiDescription.sendAlert = this.questionForm.get('sendAlert').value;
        this.checkListItem.kpiDescription.sendToRelationshipManager = this.questionForm.get('relationshipManager').value;
        this.checkListItem.kpiDescription.sendToBillingManager = this.questionForm.get('billingManager').value;
        this.checkListItem.kpiDescription.escalateAlert = this.questionForm.get('escalateAlert').value;
        if (this.checkListItem.kpiDescription.escalateAlert) {
            this.checkListItem.kpiDescription.escalateTriggerTime = this.questionForm.get('after').value + ',' + this.afterText;
        }
        this.checkListItem.checkListType.checkListTypeID = this.questionForm.get('checklistType').value;
        this.checkListItem.kpiDescription.recordStatus = 'A';
        if (this.checkListItem.universal) {
            this.checkListItem.kpiDescription.isHeatMapItem = true;
            this.checkListItem.kpiDescription.heatMapScore = 20;
        } else if (this.checkListItem.questionID > 0 && (this.questionForm.get('universal').dirty || this.questionForm.get('kpi').dirty) && (!this.questionForm.get('universal').value || !this.questionForm.get('kpi').value)) {
            let isHeatMapItem = this.checklistHeatMapItems.includes(this.checkListItem.questionCode);
            return isHeatMapItem;
        }
        return false;
    }

    /*
     * Adding validation to sendTO
     * */
    sendToSelection() {
        if (this.isSaveClicked && this.questionForm.get('sendAlert').value &&
            !this.questionForm.get('relationshipManager').value &&
            !this.questionForm.get('billingManager').value) {
            this.sendToErrorMsg = this.validationConstants.SENDTO_CHECKBOX_ERROR;
            this.ifError = true;
        } else {
            this.sendToErrorMsg = '';
            this.ifError = false;
        }
    }

  public resetForm() {
    this.initializeQuestionForm();
    this.isEditPage = false;
    this.questionControl.nativeElement.focus();
    this.removeKPIDescriptionValidationMessage();
    this.removeValidationMessage();
  }

    /*------ end region public methods ------*/

    /*------ region private methods ------*/

  /** to reset the question form and intitalize the form with default values
   */
  private initializeQuestionForm() {
      this.questionForm.reset();
      this.questionForm.setValue({
          expectedResponse: false,
          freeform: false,
          kpi: false,
          question: '',
          questionID: 0,
          checklistType: '',
          universal: false,
          kpiDescription: '',
          sendAlert: false,
          relationshipManager: false,
          billingManager: false,
          escalateAlert: false,
          after: ''
      });
  }
    /*------ end region private methods ------*/
}
