import { Injectable } from '@angular/core';
import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { AttributesViewModel } from './models/attributes.model';

@Injectable()
export class ConfigurationService {

    constructor(private httpUtility: HttpUtility) { }

    /**
     * Service method to call API to get attributes
     */
    getAttributes() {
        return this.httpUtility.get('Configurations/GetAttributesForConfig');
    }

    /**
     * service to call API to update the attribute value
     * @param attribute
     */
    updateAttributeValue(attribute) {
        let attributeToBeUpdated = new AttributesViewModel();
        attributeToBeUpdated.AttributeId = attribute.attributeId;
        attributeToBeUpdated.AttributeValue = attribute.attributeValue;
        return this.httpUtility.post('Configurations/SaveAttributeValue', attributeToBeUpdated);
    }

}
