import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { HolidayModel } from './holiday.model';

@Injectable()
export class BusinessDaysService {
    constructor(private httpUtility: HttpUtility) {
    }

    /**
     * Gets Holidays For a given Year
     * @param year
     */
    getHolidaysOfYear(year) {
        return this.httpUtility.get('BusinessDays/GetHolidaysOfYear?year=' + year);
    }

    /**
     * Edits an Existing Holiday
     * @param holiday
     */
    addOrEditHoliday(holiday: HolidayModel) {
        return this.httpUtility.post('BusinessDays/AddOrEditHoliday', holiday);
    }

    /**
     * Deletes an Exiting Holiday
     * @param holiday
     */
    deleteHolidays(holiday: HolidayModel) {
        return this.httpUtility.post('BusinessDays/DeleteHoliday', holiday);
    }

    /**
    * Gets Holidays by month For a given Year
    * @param year
    */
    getHolidaysByMonth(year) {
        return this.httpUtility.get('BusinessDays/GetHolidaysByMonth?year=' + year);
    }

    /**
     * updates client targets
     * @param year
     */
    updateClientTargets(year) {
        return this.httpUtility.get('BusinessDays/UpdateClientTargets?year=' + year);
    }

}
