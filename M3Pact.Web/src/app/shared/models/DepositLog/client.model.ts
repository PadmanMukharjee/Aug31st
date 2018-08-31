import { ClientPayerDepositViewModel } from '../../../deposit-log/models/client-payer-deposit.model';
import { SpecialitiesModel } from '../../../admin/specialities/specialities.model';
import { SystemsModel } from '../../../admin/systems/systems.model';
import { BusinessUnitViewModel } from '../../../admin/business-units/business-units.model';
import { AllUsersViewModel } from '../../../admin/users/all-users/models/all-users-viewmodel.model';
import { ValueLabel } from '../../../admin/models/checklist-item-table.model';

export class ClientViewModel {
    public clientCode: string;
    public name: string;
    public acronym: string;
    public contactName: string;
    public contactPhone: string;
    public contactEmail: string;
    public percentageOfCash: string;
    public flatFee: string;
    public numberOfProviders: number;
    public contractStartDate: Date;
    public contractEndDate: Date;
    public noticePeriod: number;
    public sendAlertsUsers: string[];
    public relationShipManager: AllUsersViewModel;
    public billingManager: AllUsersViewModel;
    public contractFilePath: string;
    public businessUnit: BusinessUnitViewModel;
    //  public CheckListViewModel MonthlyCheckList { get; set; }
    public speciality: SpecialitiesModel;
    public system: SystemsModel;
    public site: number | any;
    // public UserLogin User { get; set; }
    // public CheckListViewModel WeeklyCheckList { get; set; }
    public isActive: string;
    public completedClient: boolean;
    public clientActive: boolean;
    public clientPayer: ClientPayerDepositViewModel[];
    public recordStatus: string;
    public clientExist = false;
    public modifiedBy: string;
    public modifiedDate: string;
    public contractFileName: string;
    public weeklyChecklist: number;
    public monthlyChecklist: number;
    public weeklyChecklistEffectiveDate: Date;
    public monthlyChecklistEffectiveDate: Date;

    constructor() {
        this.speciality = new SpecialitiesModel();
        this.businessUnit = new BusinessUnitViewModel();
        this.system = new SystemsModel();
        this.relationShipManager = new AllUsersViewModel();
        this.billingManager = new AllUsersViewModel();
    }
}

