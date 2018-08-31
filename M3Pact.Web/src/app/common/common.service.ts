// Angular Imports
import { Injectable } from '@angular/core';

// common file imports
// import { Helper } from '../common/common.helper';
import { MessageConfigModel } from '../common/models/message-config.model';
import { CommonConstants } from './common.constants';


@Injectable()

export class CommonService {

    /*----- region private properties -----*/

    private messageConfigData: MessageConfigModel;
    private showErrorPage: boolean;

    /*----- endregion private properties -----*/

    /*----- region constructor -----*/

    constructor() {
        this.messageConfigData = new MessageConfigModel();
        this.setMessageData();
    }

    /*-----end region constructor -----*/

    /*----- region public Methods -----*/

    public getMessageConfigData(): MessageConfigModel {
        return this.messageConfigData;
    }

    /*----- endregion public Methods -----*/


    /*----- region private Methods -----*/

    /**
     * Sets message data
     */
    private setMessageData() {
        this.messageConfigData.RequiredMessage = CommonConstants.requiredErrorMsg;
        this.messageConfigData.MinLenMessage = CommonConstants.minLenMsg;
        this.messageConfigData.MaxLenMessage = CommonConstants.maxLenMsg;
        this.messageConfigData.InvalidNumberFieldMessage = CommonConstants.invalidonlyNumMsg;
        this.messageConfigData.RequiredSelectMsg = CommonConstants.requiredSelectErrorMsg;
        this.messageConfigData.emailPattern = CommonConstants.emailPattern;
        this.messageConfigData.maxValue = CommonConstants.maxValue;
        this.messageConfigData.InvalidFieldMessage = CommonConstants.invalidMsg;
    }
    /*-----end region private Methods -----*/
}
