// Angular Imports
import { Component, Input, } from '@angular/core';
import { FormControl } from '@angular/forms';

// file Imports
import { MessageConfigModel } from '../../models/message-config.model';
import { CommonService } from '../../common.service';
import { Helper } from '../../common.helper';

@Component({
  selector: 'validation-message',
  templateUrl: './validation-message.component.html'
})
export class ValidationMessageComponent {

  /*----- region Constructor -----*/

  constructor(private sharedService: CommonService) {

  }

  /*----- end region Constructor -----*/

  /*----- region Input properties -----*/

  private messageData: MessageConfigModel;
  private validationMessage: string;

  /*-----endregion Input properties -----*/


  /*----- region Input properties -----*/

  @Input() frmcontrol: FormControl;
  @Input() fldText: string;
  @Input() frmSubmitted: boolean;
  @Input() cstmRequiredMessage = '';
  @Input() selectRequiredMessage: string;
  @Input() frmControlEmptyCheck: boolean;

  /*----- endregion Input properties -----*/


  /*-----region public methods -----*/

     /**
      *  Shows validation errors
      */
  public showError(): boolean {
      this.messageData = this.sharedService.getMessageConfigData();
        if (this.frmcontrol && this.frmcontrol.errors && this.frmSubmitted) {
            // this.messageData = this.sharedService.getMessageConfigData();
            if (this.frmcontrol.errors.required) {
                if (this.cstmRequiredMessage) {
                    this.validationMessage = this.cstmRequiredMessage;
                } else if (this.selectRequiredMessage) {
                    this.validationMessage = Helper.format(this.messageData.RequiredSelectMsg, this.selectRequiredMessage);
                } else {
                    this.validationMessage = Helper.format(this.messageData.RequiredMessage, this.fldText);
                }
                return true;
            } else if (this.frmcontrol.errors.minlength) {
                this.validationMessage = Helper.format(this.messageData.MinLenMessage, this.fldText, this.frmcontrol.errors.minlength.requiredLength);
                return true;
            } else if (this.frmcontrol.errors.maxlength) {
                this.validationMessage = Helper.format(this.messageData.MaxLenMessage, this.fldText);
                return true;
            } else if (this.frmcontrol.errors.pattern) {
                this.validationMessage = Helper.format(this.messageData.InvalidFieldMessage, this.fldText);
                return true;
            } else if (this.frmcontrol.hasError('max')) {
                this.validationMessage = Helper.format(this.messageData.maxValue, this.fldText, this.frmcontrol.errors.max.maxValue);
                return true;
            }
            // if (this.frmcontrol.errors.passwordMatch) {
            //    this.validationMessage = Helper.format(this.messageData.PasswordMismatchMessage, this.fldText);
            //    return true;
            // }
        }        
        if (this.frmControlEmptyCheck) {
            let length = 0;
            if (this.frmcontrol.value != null) {
                length = this.frmcontrol.value.toString().trim().length;
            }
            if (length === 0) {
                if (this.cstmRequiredMessage) {
                    this.validationMessage = this.cstmRequiredMessage;
                } else if (this.selectRequiredMessage) {
                    this.validationMessage = Helper.format(this.messageData.RequiredSelectMsg, this.selectRequiredMessage);
                } else {
                    this.validationMessage = Helper.format(this.messageData.RequiredMessage, this.fldText);
                }
                return true;
            }
        }
        this.validationMessage = '';
        return false;
  }

  /*----- endregion public methods  -----*/

}
