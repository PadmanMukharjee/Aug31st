export class AllUsersModel {
    constructor() {
        this.firstName = true;
        this.email = true;
        this.reportsTo = true;
        this.businessUnit = true;
        this.site = true;
        this.role = true;
        this.title = true;
        this.Clients = true;
        this.Status = true;
        this.displayParams = [
            { field: 'firstName', header: 'First Name' },
            { field: 'email', header: 'Email' },
            { field: 'reportsTo', header: 'Reports To' },
            { field: 'businessUnit', header: 'Business Unit' },
            { field: 'site', header: 'Site' },
            { field: 'title', header: 'Title' },
            { field: 'role', header: 'Role' },
            { field: 'Clients', header: 'Clients' },
            { field: 'Status', header: 'Status' }
        ];
    }
    public userId: boolean;
    public firstName: boolean;
    public lastName: boolean;
    public email: boolean;
    public reportsTo: boolean;
    public businessUnit: boolean;
    public site: boolean;
    public title: boolean;
    public role: boolean;
    public Clients: boolean;
    public Status: boolean;
    public displayParams: any[];
}