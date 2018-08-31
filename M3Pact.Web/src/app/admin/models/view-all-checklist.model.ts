export class ViewAllChecklistModel {
    constructor() {
        this.selectedSites = new Array<SelectedSites>();
        this.selectedSystems = new Array<SelectedSystems>();
        this.selectedClients = new Array<SelectedClients>();
    }

    public name: string;
    public selectedSitesCount: any;
    public selectedSites: SelectedSites[];
    public selectedSystemsCount: any;
    public selectedSystems: SelectedSystems[];
    public selectedClientsCount: any;
    public selectedClients: SelectedClients[];
    public type: string;
}

export class SelectedClients {
    public clientCode: string;
    public name: string;
    public id: number;
}

export class SelectedSystems {
    public systemCode: string;
    public systemName: string;
}

export class SelectedSites {
    public siteCode: string;
    public siteName: string;
}