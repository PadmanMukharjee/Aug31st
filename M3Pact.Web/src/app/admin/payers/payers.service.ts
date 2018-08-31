import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { PayerViewModel } from './payers.model';
import { LogSetupViewModel } from '../../client/log-setup/log-setup.model';

@Injectable()
export class PayerService {
    constructor(private httpUtility: HttpUtility) {
    }

    /**
     * Get Payers
     */
    getPayers() {
        return this.httpUtility.get('Payer/GetPayers');
    }

    /**
     * Save Payers
     * @param payers
     */
    savePayers(payers: PayerViewModel[]) {
        return this.httpUtility.post('Payer/SavePayers', payers);
    }

    /**
     * Get payers assigned to a client
     * @param clientCode
     */
    getClientPayers(clientCode) {
        return this.httpUtility.get(`Payer/GetClientPayers?clientCode=${clientCode}`);
    }

    /**
     * Save payers assgined to a client
     * @param clientPayers
     */
    saveClientPayers(clientPayers: LogSetupViewModel[]) {
        return this.httpUtility.post('Payer/SaveClientPayers', clientPayers);
    }

    /**
     * Get Active & Unassgined payers for a client
     * @param clientCode
     */
    getActivePayersToAssignForClient(clientCode) {
        return this.httpUtility.get('Payer/GetActivePayersToAssignForClient?clientCode=' + clientCode);
    }

    /**
     * Activate or Inactivate a payer
     * @param payer
     */
    activeOrInactivePayers(payer: PayerViewModel) {
        return this.httpUtility.post('Payer/ActivateOrDeactivatePayer', payer);
    }

    /**
     * Activate or Inactivate a Client Payer
     * @param clientPayer
     */
    activeOrInactiveClientPayer(clientPayer: LogSetupViewModel) {
        return this.httpUtility.post('Payer/ActivateOrDeactivateClientPayer', clientPayer);
    }

    /**
     * Get Clients Assigned to Payer
     * @param payerCode
     */
    GetClientsAssignedtoPayer(payerCode: string) {
        return this.httpUtility.get('Payer/GetClientsAssignedtoPayer?payerCode=' + payerCode);
    }
}