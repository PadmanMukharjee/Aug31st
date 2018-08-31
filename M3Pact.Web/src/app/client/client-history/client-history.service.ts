import { Injectable } from '@angular/core';
import { HttpUtility } from '../../shared/utilities/http/http-utility.service';

@Injectable()
export class ClientHistoryService {
    constructor(private httpUtility: HttpUtility) { }

    /**
     * To get the client history.
     * @param clientCode
     * @param startDate
     * @param endDate
     */
    getClientHistory(clientCode, startDate, endDate) {
        return this.httpUtility.get('ClientData/GetClientHistory?clientCode=' + clientCode + '&startDate=' + startDate + '&endDate=' + endDate);
    }

    /**
     * To get the client creation details.
     * @param clientCode
     */
    getClientCreationDetails(clientCode) {
        return this.httpUtility.get('ClientData/GetClientCreationDetails?clientCode=' + clientCode)
    }
}
