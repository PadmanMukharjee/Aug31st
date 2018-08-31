export class ValidationResponseViewModel {
    public errorMessages = [];
    public warningMessages = [];
    public infoMessages = [];
    public rateMessages = [];
    public successMessage: string;
    public success: boolean;
    public isExceptionOccured: boolean;
    public additionalInfo: string;
}