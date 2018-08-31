import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ConfigurationService } from './configuration.service';
import { AttributesViewModel } from './models/attributes.model';
import { ADMIN_CONFIG } from '../../shared/utilities/resources/labels';
import { VALIDATION_MESSAGES } from '../../shared/utilities/resources/constants';
import { UserService } from '../../shared/services/user.service';
import { SCREEN_CODE } from '../../shared/utilities/resources/screencode';

@Component({
    selector: 'app-configuration',
    templateUrl: './configuration.component.html',
    styleUrls: ['./configuration.component.css'],
    providers: [ConfigurationService]
})
export class ConfigurationComponent implements OnInit {
    /*------ region properties ------*/
    public attributes: AttributesViewModel[];
    public labels = ADMIN_CONFIG;
    public displayDialog: boolean;
    public controlType: string;
    public selectedAttribute: any;
    public updatedConfigValue: any;
    public msgs: any[];
    public growlMsgs: any[];
    public validationConstants = VALIDATION_MESSAGES;
    public canEdit = false;
    public disableSave = false;
    public disableCancel = false;
    public showProjectedCashInformation = false;
    public paginationCount: any;
    public rowsIn: number = 10;
    /*------ end properties ------*/

    /*------ region constructor ------*/
    constructor(private _service: ConfigurationService, private ref: ChangeDetectorRef,
        private userService: UserService) {
    }
    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.getAttributes();
        this.getScreenActions();
    }
    /*------ end region life cycle hooks ------*/

    /**
     * Called when pagechange event occurs
     */
    onTablePageChange(e) {
        let start = e.first + 1;
        let end = (e.first + e.rows) > this.attributes.length ? this.attributes.length : (e.first + e.rows);
        this.paginationCount = 'Showing ' + start + ' to ' + end + ' of ' + this.attributes.length;
    }
    /**
     * To get the attributes
     */
    getAttributes() {
        this._service.getAttributes().subscribe(
            (resp) => {
                if (resp.length > 0) {
                    this.attributes = resp;
                } else {
                    this.growlMsgs = [];
                    this.growlMsgs.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.ERROR_OCCURED_GET });
                }
                let e = { first: 0, rows: this.rowsIn };
                this.onTablePageChange(e);
            },
            err => { }
        );
    }

    /**
     * to show the popup to edit the attribute value
     * @param attribute
     */
    onAttributeEdit(attribute) {
        this.showProjectedCashInformation = false;
        this.controlType = attribute.control;
        this.selectedAttribute = attribute;
        this.updatedConfigValue = attribute.attributeValue;
        if (attribute.attributeCode === this.labels.ATTRIBUTE_LASTBUSINESS_CODE) {
            this.showProjectedCashInformation = true;
        }
        this.displayDialog = true;
        this.focusConfigurationTextBox();
    }

    /**
     * To update the attribute value
     */
    updateAttributeValue() {
        if (this.isConfigValueValid()) {
            this.selectedAttribute.attributeValue = this.updatedConfigValue;
            this.disableSave = true;
            this.disableCancel = true;
            if (this.selectedAttribute.attributeCode === this.labels.ATTRIBUTE_LASTBUSINESS_CODE) {
                this.msgs = [];
                this.msgs.push({ severity: this.validationConstants.INFO, summary: '', detail: this.labels.UPDATE_PROGRESS });
            }
            this._service.updateAttributeValue(this.selectedAttribute).subscribe(
                resp => {
                    if (resp) {
                        this.closePopUp();
                        this.growlMsgs = [];
                        this.growlMsgs.push({ severity: this.validationConstants.SUCCESS, summary: '', detail: this.validationConstants.SAVED_SUCCESS });
                        this.getAttributes();
                    } else {
                        this.growlMsgs = [];
                        this.growlMsgs.push({ severity: this.validationConstants.ERROR, summary: '', detail: this.validationConstants.ERROR_OCCURED_SAVING });
                    }
                    this.disableButtons();
                    this.msgs = [];
                },
                err => {
                    this.growlMsgs = [];
                    this.growlMsgs.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.ERROR_OCCURED_SAVING });
                    this.disableButtons();
                    this.msgs = [];
                }
            );
        } else {
            this.msgs = [];
            this.msgs.push({ severity: this.validationConstants.WARN, summary: '', detail: this.validationConstants.VALID_VALUE });
        }
    }

    /**
     * To enable buttons.
     */
    disableButtons() {
        this.disableSave = false;
        this.disableCancel = false;
    }

    /**
     * to close the popup
     */
    closePopUp() {
        this.displayDialog = false;
        this.msgs = [];
        this.growlMsgs = [];
    }

    /**
     * Validating entered config value
     */
    isConfigValueValid() {
        let controlType = this.selectedAttribute.attributeType.toLowerCase();
        let regex;
        switch (controlType) {
            case 'integer':
                regex = /[+]?[1-9][0-9]*$/;
                break;
            case 'decimal':
                regex = /^\d+(\.\d{1,2})?$/;
                break;
            default:
                regex = /(.*)/;
        }
        return regex.test(this.updatedConfigValue);
    }

  /**
   * focus on input field after the popup is shown
   */

    focusConfigurationTextBox() {
        this.ref.detectChanges();
        jQuery('#tbConfigValue').focus();
    }

    /**
     * To get the screen actions based on screen code
     */
    getScreenActions() {
        this.userService.getUserScreenActions(SCREEN_CODE.ADMIN_CONFIG)
            .subscribe((actions) => {
                if (actions) {
                    this.canEdit = actions.length > 0;
                }
            });
    }
}
