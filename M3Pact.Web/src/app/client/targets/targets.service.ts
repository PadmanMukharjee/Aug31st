import { Injectable } from '@angular/core';
import { MonthlyTarget } from './models/monthly-target.model';
import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { ManuallyEditedTargets } from './models/manually-edited-targets.model';

@Injectable()
export class TargetService {
    constructor(private httpUtility: HttpUtility) { }

    // Service method to save the targets
    saveTargets(mt: MonthlyTarget) {
        return this.httpUtility.post('ClientData/SaveClientTargetData', mt);
    }

    // service method to get the targets
    getTargets(clientCode: string, year: number) {
        return this.httpUtility.get('ClientData/GetClientTargetData?clientCode=' + clientCode + '&year=' + year);
    }

    // Save the targets to db which are edited from Table view
    saveManuallyEditedTargets(manuallyEditedTargets: ManuallyEditedTargets) {
        return this.httpUtility.post('ClientData/SaveManuallyEditedTargetData', manuallyEditedTargets);
    }

    // To get the Business Days
    GetBusinessDays() {
        return this.httpUtility.get('BusinessDays/GetBusinessDays');
    }
}
