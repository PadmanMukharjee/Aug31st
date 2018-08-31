import { ClientChecklistResponse} from './client-checklistresponse.model';
export class SubmitChecklistResponse {
    clientCode: string;
    pendingDate: any;
    isSubmit: boolean;
    clientChecklistResponse: Array<ClientChecklistResponse>;
}