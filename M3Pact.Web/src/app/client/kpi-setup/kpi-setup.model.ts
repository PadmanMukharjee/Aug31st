import { CheckListType } from '../../admin/models/checklist-type.model';
import { AllUsersViewModel } from '../../admin/users/all-users/models/all-users-viewmodel.model';
import { MeasureViewModel } from '../../admin/kpi/create-kpi/measure.model';

export class KPIViewModel {
    constructor() {
        this.checklistTypeViewModel = new CheckListType();
    }

    public kpiId: number;
    public kpiDescription: string;
    public checklistTypeViewModel: CheckListType;
    public companyStandard: string;
    public isUniversal: boolean;
    public endDate: Date;
    public m3MeasureViewModel: MeasureViewModel;
}
export class ClientKPISetupViewModel {
    constructor() {
        // this.clientData = new ClientData();
        this.kpiQuestions = new Array<KPISetupViewModel>();
    }

    public clientCode: string;
    public kpiQuestions: KPISetupViewModel[];

}
export class KPISetupViewModel {

    constructor() {
        this.kpi = new KPIViewModel();
        this.sendTo = new Array<AllUsersViewModel>();
    }
    public clientKPIMapId: number;
    public kpi: KPIViewModel;
    public clientStandard: string;
    public alertLevel: string;
    public alertValue: string;
    public sla = false;
    public info: string;
    public futureRemoverOrUniversal: boolean;
    public sendTo: AllUsersViewModel[];
}

export class ClientData {
    public clientCode: string;
    public name: string;
    public id: number;
}
