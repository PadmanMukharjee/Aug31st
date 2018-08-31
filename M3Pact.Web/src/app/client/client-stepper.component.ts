// Angular imports
import { Component, OnInit, ViewEncapsulation, ViewChild, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

// Common File Imports
import { ClientDataComponent } from './client-data/client-data.component';
import { KPISetupComponent } from './kpi-setup/kpi-setup.component';
import { LogSetupComponent } from './log-setup/log-setup.component';
import { AssignClientUsersComponent } from './assign-client-user/assign-client-user.component';
import { TargetComponent } from './targets/targets.component';
import { ClientDataService } from './client-data.service';
import { KPIService } from '../admin/kpi/create-kpi/kpi.service';
import { PayerService } from '../admin/payers/payers.service';
import { AssignClientUserService } from './assign-client-user/assign-client-user.service';
import { ClientStepDetailViewModel } from './client-stepper.model';
import { ValidationResponseViewModel } from '../common/models/validation.model';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { CLIENT, CLIENT_STEP_CONSTANTS, ADMIN_SHARED, SHARED } from '../shared/utilities/resources/labels';

// Primeng Imports
import { Message } from 'primeng/components/common/api';

// Other Imports
import { Subscription } from 'rxjs';

@Component({
    selector: 'client-stepper',
    templateUrl: './client-stepper.component.html',
    styleUrls: ['./client-stepper.component.css']
})
export class ClientStepperComponent implements OnInit, OnDestroy {

    /*-----region Input/Output bindings -----*/
    @ViewChild('clientDataStep') ClientDataStep: ClientDataComponent;
    @ViewChild('logSetupStep') LogSetupStep: LogSetupComponent;
    @ViewChild('assignUserStep') AssignUserStep: AssignClientUsersComponent;
    @ViewChild('monthlyTargetsStep') MonthlyTargetsStep: TargetComponent;
    @ViewChild('kpiSetupStep') KpiSetupStep: KPISetupComponent;
    /*-----endregion Input/Output bindings -----*/

    /*------ region public properties ------*/
    public labels = CLIENT;
    public constants = CLIENT_STEP_CONSTANTS;
    public activeStep = this.constants.STEP.CLIENT_DATA;
    public currentStepDetail: ClientStepDetailViewModel;
    public stepDetails: ClientStepDetailViewModel[];
    public client: any;
    public clientCode: string;
    public clientName: string;
    public isFirstStep = true;
    public isLastStep = false;
    public msgs: Message[] = [];
    public showCompletedMsg = false;
    public isAllStepsCompleted = false;
    public pageLabel: string;
    public isSaveAndExit: boolean;
    public clientCreationSuccessMsg: string;
    public isNewClient = false;
    public validationResponse: ValidationResponseViewModel;
    public displayWarning = false;
    public warningMsg: string;
    public clickedStepElement: any;
    public editedStepID: number;
    public isSingleStepAccessOnly = false;
    public isSaveExitVisible = false;
    public isClientAlreadyActivated = false;
    /*------ end region public properties ------*/

    /*------ region private properties ------*/
    private clientCodeSubscriber: Subscription;
    private clientModeSubscriber: Subscription;
    /*------ end region private properties ------*/

    /*------ region constructor ------*/
    constructor(private _clientDataService: ClientDataService, private router: Router, private _globalEventsManager: GlobalEventsManager,
        private _payerService: PayerService, private _assignClientUserService: AssignClientUserService,
        private _KPIService: KPIService, private ref: ChangeDetectorRef) {
        this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
        };
        this.stepDetails = [];
        this.setStepColorsInCreateMode();
        this.getClientCodeAndStepDetails();
    }
    /*------ end region constructor ------*/

    /*-----region lifecycle events -----*/
    public ngOnInit(): void {
        this.initStepper();
        // this.getClientCodeAndStepDetails();
    }

    public ngOnDestroy() {
        if (this.clientModeSubscriber) {
            this.clientModeSubscriber.unsubscribe();
        }
        if (this.clientCodeSubscriber) {
            this.clientCodeSubscriber.unsubscribe();
        }
        this.ref.detach();
    }
    /*-----end region lifecycle events -----*/


    /*------ region Public methods ------*/

    // Initialize stepper
    initStepper() {
        let _self = this;
        jQuery(document).ready(function () {
            _self.initStepperClicks();
        });
    }

    // Initialize stepper clicks
    initStepperClicks() {
        let _self = this;
        jQuery('a[data-toggle="tab"]').click(function (e) {
            let firstStep = jQuery('.progress-indicator li').first();
            if (_self.clientCode === '') {
                jQuery(firstStep).nextAll().addClass('disabled');
                return false;
            } else {
                _self.clickedStepElement = e.currentTarget.parentElement;
                jQuery(firstStep).nextAll().removeClass('disabled');
                let currentStepID = _self.getActiveStepID();
                if (_self.checkForEditsInClientStep(currentStepID)) {
                    _self.editedStepID = currentStepID;
                    return false;
                }
                _self.activeStep = Number(jQuery(_self.clickedStepElement).attr('id'));
            }
            _self.isFirstStep = jQuery(this).parent().is(':first-child') ? true : false;
            _self.isLastStep = jQuery(this).parent().is(':last-child') ? true : false;
        });
    }

    // Get client code and step details
    getClientCodeAndStepDetails() {
        this.clientModeSubscriber = this._globalEventsManager.getClientMode.subscribe((mode) => {
            if (mode == undefined) {
                this.router.navigate(['/depositlog']);
            }
            this.fetchClientCode(mode);
            this.getStepStatusDetails(this.clientCode);
        });
    }

    // Save Client data (StepID-1) to db
    saveClientData(form) {
        form.append('isNewClient', this.isNewClient);
        this._clientDataService.saveClientData(form).subscribe((resp) => {
            if (resp) {
                this.validationResponse = JSON.parse(resp.text());
                if (this.validationResponse.success) {
                    this.enableClientStepsAfterClientDetails();
                    this.ClientDataStep.clientData.clientExist = true;
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_SUCCESS, summary: 'Client Details', detail: SHARED.SUCCESS_MESSAGE });
                    this.clientCode = this.ClientDataStep.clientData.clientCode;
                    this.clientName = this.ClientDataStep.clientData.name;

                    if (!this.isAllStepsCompleted) {
                        this.pageLabel = this.labels.CREATE_CLIENT + ': ' + this.clientName;
                    } else {
                        if (this.ClientDataStep.clientData.isActive == 'A') {
                            this._globalEventsManager.setClientDropdown(true);
                            this._globalEventsManager.setGlobalClientCode({ label: this.clientName, value: this.clientCode });
                            this._globalEventsManager.setClientCode({ label: this.clientName, value: this.clientCode });
                        } else {
                            this._globalEventsManager.setGlobalClientCode({});
                            this._globalEventsManager.setClientCode({ label: this.clientName, value: this.clientCode });
                            this._globalEventsManager.setClientDropdown(false);
                        }
                    }
                    this.saveCurrentStepStatusDetail(this.isSaveAndExit);

                    if (this.isNewClient) {
                        this.isNewClient = false;
                        this._globalEventsManager.setClientCode({ label: this.clientName, value: this.clientCode });
                        this._globalEventsManager.setClientMode('editPartial');
                    }
                } else {
                    this.ClientDataStep.errorMsg = this.validationResponse.errorMessages[0];
                    this.ClientDataStep.showErrorMsg = true;
                }
            }
        },
            err => {
                console.log(err);
            });
    }

    // Enable the client steps after StepID-1 is completed successfully.
    enableClientStepsAfterClientDetails() {
        if (this.isNewClient) {
            let firstStep = jQuery('.progress-indicator li').first();
            jQuery(firstStep).nextAll().removeClass('disabled');
        }
    }

    // Get client code from client mode (from create/edit)
    fetchClientCode(clientmode) {
        if (clientmode === 'edit') {
            this.clientCodeSubscriber = this._globalEventsManager.getGlobalClientCode.subscribe((globalClientCode) => {
                this.setSettingsForEditOfActivateInActivateAClient(globalClientCode);
            });
        } else if (clientmode === 'editInactive') {
            this.clientCodeSubscriber = this._globalEventsManager.getClientCode.subscribe((clientCode) => {
                this.setSettingsForEditOfActivateInActivateAClient(clientCode);
            });
        } else if (clientmode === 'create') {
            this.isNewClient = true;
            this.clientCode = '';
            this.pageLabel = this.labels.CREATE_CLIENT;
            this.setStepColorsInCreateMode();
            this.isClientAlreadyActivated = false;
            this._globalEventsManager.setClientCode(this.clientCode);
        } else if (clientmode === 'editPartial') {
            this.clientCodeSubscriber = this._globalEventsManager.getClientCode.subscribe((globalClientCode) => {
                this.isNewClient = false;
                this.client = globalClientCode;
                this.clientCode = this.client.value;
                this.clientName = this.client.label;
                this.isClientAlreadyActivated = false;
                this.pageLabel = this.labels.CREATE_CLIENT + ': ' + this.client.value + ' - ' + this.client.label;
            });
        }
    }

    //Set Settings For Editing Active/InActive Client
    setSettingsForEditOfActivateInActivateAClient(clientCode) {
        this.isNewClient = false;
        this.client = clientCode;
        this.clientCode = this.client.value;
        this.clientName = this.client.label;
        this.isClientAlreadyActivated = true;
        this.pageLabel = this.labels.VIEW_OR_EDIT_CLIENT + ': ' + this.client.value + ' - ' + this.client.label;
    }

    // Set focus on first step in create mode
    setStepColorsInCreateMode() {
        this.stepDetails.forEach(x => {
            x.stepStatusClass = x.stepID === this.constants.STEP.CLIENT_DATA ? 'active' : '';
        });
    }

    // Get stepper icon details from db
    getStepStatusDetails(clientCode) {
        this._clientDataService.getClientStepStatusDetails(clientCode).subscribe(
            (resp) => {
                if (resp.length > 0) {
                    this.stepDetails = resp;
                    this.prepareClientStepperStructure(this.stepDetails);
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: 'Client Step Status', detail: SHARED.ERROR_GET_DETAILS });
                }
            }
        );
    }

    // Show Save&Exit button when all the steps are editable
    saveAndExitButtonVisibility(details) {
        let canEditSteps = details.filter(x => x.canEdit == true).length;
        if (canEditSteps === this.constants.NO_OF_STEPS) {
            this.isSaveExitVisible = true;
        }
    }

    // Show single step page instead of step bubbles
    showSigleStepPage(details) {
        let canViewStepDetail = details.filter(x => x.canView == true);
        if (canViewStepDetail && canViewStepDetail[0].stepID === this.constants.STEP.KPI_SETUP) {
            this.pageLabel = this.labels.VIEW_KPI + ': ' + this.client.label;
            this.activeStep = canViewStepDetail[0].stepID;
        }
        this.isSingleStepAccessOnly = true;
    }

    // Assign classes (colors) to each step
    constructClassesAndFlagsForStepper(details) {       
        if (details != null) {
            let noOfSteps = details.length;
            this.saveAndExitButtonVisibility(details);
            let canViewSteps = details.filter(x => x.canView == true).length;
            if (canViewSteps === 1) {
                this.showSigleStepPage(details);
            } else {
                let noStepCompleted = false;
                let completedSteps = details.filter(x => x.stepStatusID == this.constants.STEP_STATUS.COMPLETED).length;
                if (completedSteps === noOfSteps) {
                    this.isAllStepsCompleted = true;
                } else if (completedSteps === 0) {
                    noStepCompleted = true;
                }
                let inProgressStep = details.find(x => x.stepStatusID == this.constants.STEP_STATUS.IN_PROGRESS);
                if (inProgressStep !== undefined && inProgressStep !== null) {
                    this.activeStep = inProgressStep.stepID;
                } else {
                    this.activeStep = this.constants.STEP_STATUS.IN_PROGRESS;
                }
                this.isFirstStep = this.activeStep == this.constants.STEP.CLIENT_DATA ? true : false;
                this.isLastStep = this.activeStep == this.constants.STEP.KPI_SETUP ? true : false;

                if (this.isAllStepsCompleted) {
                    details.forEach(x => {
                        x.stepStatusClass = x.stepID === this.constants.STEP.CLIENT_DATA ? 'completed active' : 'completed';
                    });
                } else if (noStepCompleted) {
                    details.forEach(x => {
                        x.stepStatusClass = x.stepID === this.constants.STEP.CLIENT_DATA ? 'active' : 'disabled';
                    });
                } else {
                    details.forEach(x => {
                        if (x.stepStatusID === this.constants.STEP_STATUS.IN_PROGRESS) {
                            x.stepStatusClass = 'active';
                        } else if (x.stepStatusID == this.constants.STEP_STATUS.COMPLETED) {
                            x.stepStatusClass = 'completed';
                        } else {
                            x.stepStatusClass = '';
                        }
                    });
                }
            }
        }
    }

    // Prepare the client step bubbles (labels & icons)
    prepareClientStepperStructure(details) {
        let noOfVisibleSteps = details.length;
        details.forEach((x, i) => {
            if (x.stepID === this.constants.STEP.CLIENT_DATA) {
                x.stepShortName = this.constants.STEP_SHORTNAME.CLIENT_DATA;
                x.stepLink = '#' + x.stepShortName;
                x.stepIconClass = this.constants.CLASS.USERS_ICON;
            } else if (x.stepID === this.constants.STEP.LOG_SETUP) {
                x.stepShortName = this.constants.STEP_SHORTNAME.LOG_SETUP;
                x.stepLink = '#' + x.stepShortName;
                x.stepIconClass = this.constants.CLASS.COGS_ICON;
            } else if (x.stepID === this.constants.STEP.ASSIGN_USER) {
                x.stepShortName = this.constants.STEP_SHORTNAME.ASSIGN_USER;
                x.stepLink = '#' + x.stepShortName;
                x.stepIconClass = this.constants.CLASS.USER_ICON;
            } else if (x.stepID === this.constants.STEP.MONTHLY_TARGETS) {
                x.stepShortName = this.constants.STEP_SHORTNAME.MONTHLY_TARGETS;
                x.stepLink = '#' + x.stepShortName;
                x.stepIconClass = this.constants.CLASS.BULLS_EYE_ICON;
            } else if (x.stepID === this.constants.STEP.KPI_SETUP) {
                x.stepShortName = this.constants.STEP_SHORTNAME.KPI_SETUP;
                x.stepLink = '#' + x.stepShortName;
                x.stepIconClass = this.constants.CLASS.BAR_CHART_ICON;
            }
            x.stepBubbleType = this.constants.CLASS.BUBBLE;
            if (i == (noOfVisibleSteps - 1)) {
                x.stepBubbleType = this.constants.CLASS.LAST_BUBBLE;
            }
        });
        this.constructClassesAndFlagsForStepper(details);
        if (jQuery('.resp_client')) {
            if (!this.isClientAlreadyActivated) {
                jQuery('.resp_client').addClass('res_padding');
            } else {
                jQuery('.resp_client').removeClass('res_padding');
            }
        }

        this.ref.detectChanges();
        this.initStepperClicks();
    }

    // Prepare the current step details to be saved
    prepareCurrentStepDetails() {
        let stepID = this.getActiveStepID();
        this.currentStepDetail = new ClientStepDetailViewModel();
        this.currentStepDetail.clientCode = this.clientCode;
        this.currentStepDetail.stepID = stepID;
        this.currentStepDetail.stepStatusID = this.constants.STEP_STATUS.COMPLETED;
    }

    // Get current client step id
    getActiveStepID() {
        let stepID = 1;
        let currentStep = jQuery('.progress-indicator li.active').attr('id');
        if (currentStep) {
            stepID = Number(currentStep);
        }
        return stepID;
    }

    // Save and exit button click
    saveAndExit() {
        this.isSaveAndExit = true;
        this.prepareCurrentStepDetails();
        if (this.currentStepDetail.stepID === this.constants.STEP.CLIENT_DATA) {
            this.ClientDataStep.saveClientDataFromParent();
        } else if (this.currentStepDetail.stepID === this.constants.STEP.LOG_SETUP || this.currentStepDetail.stepID === this.constants.STEP.KPI_SETUP) {
            this.router.navigateByUrl('/depositlog'); // No need to save step status details on Save&Exit, just redirect to landing page
        } else if (this.currentStepDetail.stepID === this.constants.STEP.ASSIGN_USER) {
            this.saveCurrentStepStatusDetail(true); // Place this code after save of StepID-3 in future
        } else if (this.currentStepDetail.stepID === this.constants.STEP.MONTHLY_TARGETS) {
            if (this.MonthlyTargetsStep.graphView) {
                this.MonthlyTargetsStep.saveTargetGraphDataFromParent();
            } else {
                this.MonthlyTargetsStep.saveManuallyEditedTargets();
            }
        }
    }

    // Save current step status details in db
    saveCurrentStepStatusDetail(isExit) {
        this.prepareCurrentStepDetails();
        this._clientDataService.saveClientStepStatus(this.currentStepDetail).subscribe(
            (resp) => {
                if (resp) {
                    if (isExit) {
                        this.router.navigateByUrl('/depositlog');
                    }
                    let currentStep = jQuery('.progress-indicator li.active');
                    let currentStepID = currentStep.attr('id');
                    if (currentStepID == this.constants.STEP.CLIENT_DATA) {
                        this.router.navigateByUrl(this.router.url);
                    }
                    currentStep.addClass('completed');
                    this.checkForAllStepsCompletion();
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: 'Save Client', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }

    // Check whether all client steps are completed or not. Based on this show Client Success Creation Popup
    checkForAllStepsCompletion() {
        let completedSteps = jQuery('.progress-indicator li.completed').length;
        if (completedSteps === this.constants.NO_OF_STEPS && !this.isAllStepsCompleted) {
            this.clientCreationSuccessMsg = this.labels.CLIENT_SUCCESS_CREATION.replace('XXXX', this.clientName);
            this.showCompletedMsg = true;
            this._clientDataService.activateClient(this.clientCode).subscribe(
                data => {
                    if (!data) {
                        this.msgs = [];
                        this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: 'Activate Client', detail: SHARED.ERROR_SAVE_MESSAGE });
                    }
                },
                err => { });
        }
    }

    // Redirect to Landing page on success creation of client
    redirectToLandingPage() {
        this.showCompletedMsg = false;
        this._globalEventsManager.setGlobalClientCode({ label: this.clientName, value: this.clientCode });
        this._globalEventsManager.setClientDropdown(true);
        this.router.navigateByUrl('/depositlog');
    }

    // save the step status of Monthly Targets (StepID-4)
    saveTargetStepStatus(isSaveAndExit) {
        this.saveCurrentStepStatusDetail(isSaveAndExit);
    }

    // save Log setup (StepID-2)
    saveLogSetup(logSetupData) {
        this._payerService.saveClientPayers(logSetupData).subscribe(
            (success) => {
                if (success) {
                    this.LogSetupStep.getClientPayers();
                    this.saveCurrentStepStatusDetail(false);

                }
            }
        );
    }

    // save Assigned Users (StepID-3)
    saveAssignedUsers(assignedUserData) {
        this._assignClientUserService.saveClientUsers(assignedUserData).subscribe(
            data => {
                if (data == true) {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_SUCCESS, summary: 'Client Assign', detail: SHARED.SUCCESS_MESSAGE });
                    this.saveCurrentStepStatusDetail(false);
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: 'Client Assign', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }

    // save KPI Setup (StepID-5)
    saveKPISetup(clientKpi) {
        this._KPIService.saveClientKPIDetails(clientKpi).subscribe(
            data => {
                if (data) {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_SUCCESS, summary: 'KPI Assign', detail: SHARED.SUCCESS_MESSAGE });
                    this.KpiSetupStep.displayDialog = false;
                    this.KpiSetupStep.getAssignedKPIsForClient();
                } else {
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: 'KPI Assign', detail: SHARED.ERROR_SAVE_MESSAGE });
                }
            }
        );
    }

    // To go to next step on click of Next icon
    onNext() {
        this.onNextPreviousClick(true);
    }

    // To go to next step on click of Previous icon
    onPrevious() {
        this.onNextPreviousClick(false);
    }

    // Next/Previous Click
    onNextPreviousClick(isNextClick) {
        let currentStepID = this.getActiveStepID();
        let currentStep = jQuery('.progress-indicator li.active');
        this.clickedStepElement = isNextClick ? currentStep.next() : currentStep.prev();
        if (this.checkForEditsInClientStep(currentStepID)) {
            this.editedStepID = currentStepID;
            return false;
        } else {
            currentStep.removeClass('active');
            this.changeStep(this.clickedStepElement);
        }
    }

    // Check for any edit done in the step when navigated to other step without saving
    checkForEditsInClientStep(currentStepID) {
        let isEditInStep = false;
        if (currentStepID === this.constants.STEP.CLIENT_DATA) {
            if (!this.ClientDataStep.clientDataForm.pristine) {
                isEditInStep = true;
            }
        } else if (currentStepID === this.constants.STEP.MONTHLY_TARGETS) {
            if (this.MonthlyTargetsStep.graphView) {
                if (!this.MonthlyTargetsStep.monthlyTargetForm.pristine) {
                    isEditInStep = true;
                }
            } else {                
            }
        }

        if (isEditInStep) {
            this.warningMsg = this.labels.DATA_LOST_IF_NO_SAVE;
            this.displayWarning = true;
        }
        return isEditInStep;
    }

    // Get the step to be made as active step
    changeStep(elem) {
        jQuery(elem).find('a[data-toggle="tab"]').click();
    }

    // Remain in same step when clicked on No (for "Do you want to proceed?")
    beInCurrentStep() {
        this.displayWarning = false;
    }

    // Go to the respective selected step when clicked on Yes (for "Do you want to proceed?")
    goToClientStep() {
        this.displayWarning = false;
        jQuery('.progress-indicator li').removeClass('active');
        if (this.editedStepID === this.constants.STEP.CLIENT_DATA) {
            this.ClientDataStep.clientDataForm.form.markAsPristine();
        } else if (this.editedStepID === this.constants.STEP.MONTHLY_TARGETS) {
            if (this.MonthlyTargetsStep.graphView) {
                this.MonthlyTargetsStep.monthlyTargetForm.form.markAsPristine();
            } else {                
            }
        }
        this.changeStep(this.clickedStepElement);
    }

    // Save KPI Setup (StepID-5) status when the step is initialized
    saveKPISetupStatusDetail() {
        this.saveCurrentStepStatusDetail(false);
    }

    /*------ end region Public methods ------*/

}
