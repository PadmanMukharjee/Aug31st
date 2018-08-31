// Third party imports
import { Message } from 'primeng/components/common/api';
import { Table } from 'primeng/table';

// Angular imports
import { Component, OnInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { NgForm } from '@angular/forms';

// Common File Imports
import { CLIENTS_HEATMAP } from '../shared/utilities/resources/labels';
import { SystemsModel } from '../admin/systems/systems.model';
import { BusinessUnitViewModel } from '../admin/business-units/business-units.model';
import { SystemsService } from '../admin/systems/systems.service';
import { BusinessUnitsService } from '../admin/business-units/busienss-units.service';
import { SpecialitiesService } from '../admin/specialities/specialities.service';
import { SpecialitiesModel } from '../admin/specialities/specialities.model';
import { AllUsersService } from '../admin/users/all-users/all-users.service';
import { AllUsersViewModel } from '../admin/users/all-users/models/all-users-viewmodel.model';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { ClientsHeatMapService } from './heatmap.service';
import { ClientViewModel } from '../shared/models/DepositLog/client.model';
import { ClientsHeatMapRequestViewModel } from './models/heatmap-request.model';
import { User } from '../shared/models/user';
import { UserService } from '../shared/services/user.service';
import { HeatMapDropdown } from './models/heatmap-dropdown.model';
import { ADMIN_SHARED } from '../shared/utilities/resources/labels';


@Component({
    selector: 'client-heatmap',
    templateUrl: './heatmap.component.html',
    styleUrls: ['./heatmap.component.css'],
    providers: [SystemsService, BusinessUnitsService, SpecialitiesService, AllUsersService]
})
export class ClientsHeatMapComponent implements OnInit, OnDestroy {

    /*------ region public properties ------*/
    public labels = CLIENTS_HEATMAP;
    public sharedLabels = ADMIN_SHARED;
    public clientHeatMaps: any[];
    public metricRiskColumns: any[];
    public systemOptions: SystemsModel[];
    public selectedSystem: SystemsModel;
    public businessUnitsOptions: BusinessUnitViewModel[];
    public selectedBusinessUnit: BusinessUnitViewModel;
    public specialityOptions: SpecialitiesModel[];
    public employeeOptions: AllUsersViewModel[];
    public selectedSpeciality: SpecialitiesModel;
    public allUsers: AllUsersViewModel[];
    public allUsersData: AllUsersViewModel[];
    public selectedEmployee: AllUsersViewModel;
    public heatMapFilters: HeatMapDropdown[];
    public disableBusinessUnit = false;
    public disableSystem = false;
    public disableEmployee = false;
    public disableSpeciality = false;
    public unselectedDropdown: Array<string> = ['System', 'BusinessUnit', 'Speciality', 'Employee'];
    public globalData: Array<AllUsersViewModel>;
    public firstFilterData: Array<AllUsersViewModel>;
    public secondFilterData: Array<AllUsersViewModel>;
    public thirdFilterData: Array<AllUsersViewModel>;
    public fourthFilterData: Array<AllUsersViewModel>;
    public weeklyRiskFactorCount = 0;
    public monthlyRiskFactorCount = 0;
    public checklistColumnCount = 0;
    public showHeatMapGrid = false;
    public currentUser: User;
    public messages: Message[] = [];
    @ViewChild('filterForm') public filterForm: NgForm;
    @ViewChild('heatmapGrid') dataTable: Table;
    /*------ end region public properties ------*/

    /*------ region constructor ------*/
    constructor(private _globalEvents: GlobalEventsManager, private systemsService: SystemsService, private businessUnitsService: BusinessUnitsService,
        private specialtiesService: SpecialitiesService, private allUsersService: AllUsersService, private heatMapService: ClientsHeatMapService, private userService: UserService,
        private ref: ChangeDetectorRef) {
        this._globalEvents.setClientDropdown(false);
        this.getHeatMapFilterData();
        this.heatMapFilters = new Array<HeatMapDropdown>();
        this.unselectedDropdown = new Array<string>();
        this.allUsers = new Array<AllUsersViewModel>();
        this.firstFilterData = new Array<AllUsersViewModel>();
        this.secondFilterData = new Array<AllUsersViewModel>();
        this.thirdFilterData = new Array<AllUsersViewModel>();
        this.fourthFilterData = new Array<AllUsersViewModel>();
        this.currentUser = new User();
    }
    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.currentUser = this.userService.getCurrentUser();
        this.defaultHeatMapData();
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.detach();
        }
    }
    /*------ end region life cycle hooks ------*/



    /*------ region public methods ------*/


    /**
     * To get all the data for the heat map filters.
     */
    getHeatMapFilterData() {
        this.heatMapService.getHeatMapFilterData().subscribe(
            (response) => {
                if (response.success === true) {
                    let data: any = response;
                    this.businessUnitsOptions = data.businessUnitList;
                    this.systemOptions = data.systemList;
                    this.specialityOptions = data.specialityList;
                    let users = data.allUsersViewModel;
                    this.globalData = data.allUsersViewModel;
                    this.allUsersData = new Array<AllUsersViewModel>();
                    for (let user of users) {
                        user.fullName = user.lastName + ' ' + user.firstName;
                        this.allUsersData.push(user);
                    }
                    this.employeeOptions = this.allUsersData;
                    this.removeLoggedInEmployeeFromUserList();
                    this.createAllEmployeeObject();
                    this.formDataBasedOnFilter();
                    this.createAllSystemObject();
                    this.createAllSpecialityObject();
                    this.createAllBusinessUnitObject();
                } else {
                    this.messages = [];
                    this.messages.push({ severity: 'error', summary: this.sharedLabels.ERROR, detail: response.errorMessages[0] });
                }
            }
        );
    }

    /**
     * To form a new object for AllUsersViewModel model.
     * @param completeUserData
     */
    formData(completeUserData: Array<AllUsersViewModel>) {
        this.allUsersData = null;
        this.allUsersData = new Array<AllUsersViewModel>();
        for (let userData in completeUserData) {
            let user = new AllUsersViewModel();
            user = Object.assign({}, completeUserData[userData]);
            user.selectedClients = [];
            for (let client in completeUserData[userData].selectedClients) {
                let clientData = new ClientViewModel();
                clientData = Object.assign({}, completeUserData[userData].selectedClients[client]);
                user.selectedClients.push(clientData);
            }
            this.allUsersData.push(user);
        }
        this.formDataBasedOnFilter();
    }

    /**
     * To remove logged in employee from employee list.
     */
    removeLoggedInEmployeeFromUserList() {
        let index = this.employeeOptions.findIndex(i => i.email === this.currentUser.email);
        if (index >= 0) {
            this.employeeOptions.splice(index, 1);
        }
    }

    /**
     * To form the data for the first filter.
     */
    formDataBasedOnFilter() {
        this.allUsers = null;
        this.allUsers = new Array<AllUsersViewModel>();
        for (let userData in this.allUsersData) {
            let user = new AllUsersViewModel();
            user = Object.assign({}, this.allUsersData[userData]);
            user.selectedClients = [];

            for (let client in this.allUsersData[userData].selectedClients) {
                let clientData = new ClientViewModel();
                clientData = Object.assign({}, this.allUsersData[userData].selectedClients[client]);
                user.selectedClients.push(clientData);
            }
            this.allUsers.push(user);
        }
    }

    /**
     * To create all system object.
     */
    createAllSystemObject() {
        let allSystemOption = new SystemsModel();
        allSystemOption.systemCode = 'All Systems';
        allSystemOption.systemName = 'All Systems';
        this.systemOptions.unshift(allSystemOption);
        this.selectedSystem = allSystemOption;
    }

    /**
     * To create all speciality object.
     */
    createAllSpecialityObject() {
        let allSpecialityOption = new SpecialitiesModel();
        allSpecialityOption.specialityCode = 'All Specialities';
        allSpecialityOption.specialityName = 'All Specialties';
        this.specialityOptions.unshift(allSpecialityOption);
        this.selectedSpeciality = allSpecialityOption;
    }

    /**
     * To create all employee object.
     */
    createAllEmployeeObject() {
        let allEmployeeOption: AllUsersViewModel = new AllUsersViewModel();
        allEmployeeOption.fullName = 'All Employees';
        allEmployeeOption.email = 'All Employees';
        this.employeeOptions.unshift(allEmployeeOption);
        this.selectedEmployee = allEmployeeOption;
    }

    /**
     * To create all business unit object.
     */
    createAllBusinessUnitObject() {
        let allbusinessUnitOption = new BusinessUnitViewModel();
        allbusinessUnitOption.businessUnitCode = 'All Business Units';
        allbusinessUnitOption.businessUnitName = 'All Business Units';
        this.businessUnitsOptions.unshift(allbusinessUnitOption);
        this.selectedBusinessUnit = allbusinessUnitOption;
    }

    /**
     * To check and disable the previous filter.
     * @param filterName
     */
    callFilterDisable(filterName: string) {
        for (let filter in this.heatMapFilters) {
            if (this.heatMapFilters[filter].dropdownName.indexOf(filterName) < 0) {
                this.disableDropdown(this.heatMapFilters[this.heatMapFilters.length - 2].dropdownName);
            }
        }
    }

    /**
     * To check and enable the previous filter.
     * @param filterName
     */
    callFilterEnable(filterName: string) {
        for (let filter in this.heatMapFilters) {
            if (this.heatMapFilters[filter].dropdownName.indexOf(filterName) > 0) {
                this.enableDropdown(this.heatMapFilters[this.heatMapFilters.length - 2].dropdownName);
            }
        }
    }

    /**
     * To change data based on filter change.
     */
    dataForAllFilter() {
        if (this.heatMapFilters.length > 0) {
            switch (this.heatMapFilters.length) {
                case 1:
                    this.formData(this.globalData);
                    this.heatMapFilters.pop();
                    break;
                case 2:
                    this.enableDropdown(this.heatMapFilters[this.heatMapFilters.length - 2].dropdownName);
                    this.formData(this.globalData);
                    this.heatMapFilters.pop();
                    break;
                case 3:
                    this.enableDropdown(this.heatMapFilters[this.heatMapFilters.length - 2].dropdownName);
                    this.formData(this.secondFilterData);
                    this.heatMapFilters.pop();
                    break;
                case 4:
                    this.enableDropdown(this.heatMapFilters[this.heatMapFilters.length - 2].dropdownName);
                    this.formData(this.thirdFilterData);
                    this.heatMapFilters.pop();
                    break;
            }
        }
    }

    /**
     * Remove the duplicate filter and modify the data.
     * @param filterName
     */
    removeDuplicateFilter(filterName: string) {
        if (this.heatMapFilters.length > 0) {
            for (let filter in this.heatMapFilters) {
                if (this.heatMapFilters[filter].dropdownName.indexOf(filterName) >= 0) {
                    switch (this.heatMapFilters.length) {
                        case 1:
                            this.formData(this.globalData);
                            break;
                        case 2:
                            this.formData(this.secondFilterData);
                            break;
                        case 3:
                            this.formData(this.thirdFilterData);
                            break;
                        case 4:
                            this.formData(this.fourthFilterData);
                            break;
                    }
                    this.heatMapFilters.pop();
                }
            }
        }
    }

    /**
     * Calls when the system value is changed.
     */
    onChangeSystem() {
        this.removeDuplicateFilter('System');
        let filter = new HeatMapDropdown();
        filter.dropdownName = 'System';
        filter.dropdownValue = this.selectedSystem.systemCode;
        filter.dropdownSelected = true;
        this.heatMapFilters.push(filter);
        this.callFilterDisable('System');
        if (filter.dropdownValue === 'All Systems') {
            this.dataForAllFilter();
            this.callFilterEnable('System');
        }
        this.getGlobalDataForLastDropdown(filter, 'All Systems');
    }

    /**
     * Calls when the businessunit value is changed.
     */
    onChangeBusinessUnit() {
        this.removeDuplicateFilter('BusinessUnit');
        let filter = new HeatMapDropdown();
        filter.dropdownName = 'BusinessUnit';
        filter.dropdownValue = this.selectedBusinessUnit.businessUnitCode;
        filter.dropdownSelected = true;
        this.heatMapFilters.push(filter);
        this.callFilterDisable('BusinessUnit');
        if (filter.dropdownValue === 'All Business Units') {
            this.dataForAllFilter();
            this.callFilterEnable('BusinessUnit');
        }
        this.getGlobalDataForLastDropdown(filter, 'All Business Units');
    }

    /**
     * Calls when the speciality value is changed.
     */
    onChangeSpeciality() {
        this.removeDuplicateFilter('Speciality');
        let filter = new HeatMapDropdown();
        filter.dropdownName = 'Speciality';
        filter.dropdownValue = this.selectedSpeciality.specialityCode;
        filter.dropdownSelected = true;
        this.heatMapFilters.push(filter);
        this.callFilterDisable('Speciality');
        if (filter.dropdownValue === 'All Specialities') {
            this.dataForAllFilter();
            this.callFilterEnable('Speciality');
        }
        this.getGlobalDataForLastDropdown(filter, 'All Specialities');
    }

    /**
     * Calls when the employee value is changed.
     */
    onChangeEmployee() {
        this.removeDuplicateFilter('Employee');
        let filter = new HeatMapDropdown();
        filter.dropdownName = 'Employee';
        filter.dropdownValue = this.selectedEmployee.email;
        filter.dropdownSelected = true;
        this.heatMapFilters.push(filter);
        this.callFilterDisable('Employee');
        if (filter.dropdownValue === 'All Employees') {
            this.dataForAllFilter();
            this.callFilterEnable('Employee');
        }
        this.getGlobalDataForLastDropdown(filter, 'All Employees');
    }

    /**
     * To get the global data.
     * @param filter
     * @param dropdownValue
     */
    getGlobalDataForLastDropdown(filter: HeatMapDropdown, dropdownValue: string) {
        if (filter.dropdownValue === dropdownValue && this.heatMapFilters.length === 0) {
            this.getHeatMapFilterData();
        } else {
            this.filter();
        }
    }

    /**
     * To enable the drop down.
     * @param dropDown
     */
    enableDropdown(dropDown: string) {
        switch (dropDown) {
            case 'System':
                this.disableSystem = false;
                break;
            case 'BusinessUnit':
                this.disableBusinessUnit = false;
                break;
            case 'Employee':
                this.disableEmployee = false;
                break;
            case 'Speciality':
                this.disableSpeciality = false;
                break;
        }
    }

    /**
     * To disable the drop down.
     * @param dropDown
     */
    disableDropdown(dropDown: string) {
        switch (dropDown) {
            case 'System':
                this.disableSystem = true;
                break;
            case 'BusinessUnit':
                this.disableBusinessUnit = true;
                break;
            case 'Employee':
                this.disableEmployee = true;
                break;
            case 'Speciality':
                this.disableSpeciality = true;
                break;
        }
    }

    /**
     * To reset all the filters.
     */
    reset() {
        this.showHeatMapGrid = false;
        this.getHeatMapFilterData();
        this.defaultHeatMapData();
        this.heatMapFilters = [];
        this.unselectedDropdown = ['System', 'BusinessUnit', 'Speciality', 'Employee'];
        this.disableSystem = false;
        this.disableBusinessUnit = false;
        this.disableEmployee = false;
        this.disableSpeciality = false;
    }


    /**
     * Maintaining multiple forms of data based on number of filters.
     */
    filter() {
        this.unselectedDropdown = ['System', 'BusinessUnit', 'Speciality', 'Employee'];
        for (let filter in this.heatMapFilters) {
            if (this.heatMapFilters[filter].dropdownSelected === true) {
                let index = this.unselectedDropdown.indexOf(this.heatMapFilters[filter].dropdownName);
                if (index > -1) {
                    this.unselectedDropdown.splice(index, 1);
                }
            }
        }
        if (this.heatMapFilters.length === 1) {
            this.firstFilterData = this.allUsersData;
            this.filterData();
        } else {
            this.allUsersData = null;
            this.allUsersData = new Array<AllUsersViewModel>();
            for (let userData in this.allUsers) {
                let user = new AllUsersViewModel();
                user = Object.assign({}, this.allUsers[userData]);
                user.selectedClients = [];
                for (let client in this.allUsers[userData].selectedClients) {
                    let clientData = new ClientViewModel();
                    user.selectedClients.findIndex(i => i.clientCode === this.allUsers[userData].selectedClients[client].clientCode);
                    clientData = Object.assign({}, this.allUsers[userData].selectedClients[client]);
                    let index = user.selectedClients.findIndex(i => i.clientCode === this.allUsers[userData].selectedClients[client].clientCode);
                    if (index == -1) {
                        user.selectedClients.push(clientData);
                    }
                }
                this.allUsersData.push(user);
            }
            switch (this.heatMapFilters.length) {
                case 2:
                    this.secondFilterData = this.allUsersData;
                    break;
                case 3:
                    this.thirdFilterData = this.allUsersData;
                    break;
                case 4:
                    this.fourthFilterData = this.allUsersData;
                    break;
            }
            this.filterData();
        }
    }

    /**
     * Actual filter logic for managing data.
     */
    filterData() {
        let iterator = 0;
        let lastdropdown = this.heatMapFilters[this.heatMapFilters.length - 1].dropdownName;
        if (this.heatMapFilters.length === 1 && this.heatMapFilters[0].dropdownName === 'Employee') {
            this.businessUnitsOptions = [];
            this.systemOptions = [];
            this.specialityOptions = [];
            for (let user in this.allUsersData) {
                if (this.allUsersData[user].email === this.selectedEmployee.email) {
                    this.allUsers.push(this.allUsersData[user]);
                    for (let client in this.allUsersData[user].selectedClients) {
                        if (this.systemOptions.filter(b => b.systemCode === this.allUsersData[user].selectedClients[client].system.systemCode).length === 0) {
                            this.systemOptions.push(this.allUsersData[user].selectedClients[client].system);
                        }
                        if (this.businessUnitsOptions.filter(b => b.businessUnitCode === this.allUsersData[user].selectedClients[client].businessUnit.businessUnitCode).length === 0) {
                            this.businessUnitsOptions.push(this.allUsersData[user].selectedClients[client].businessUnit);
                        }
                        if (this.specialityOptions.filter(b => b.specialityCode === this.allUsersData[user].selectedClients[client].speciality.specialityCode).length === 0) {
                            this.specialityOptions.push(this.allUsersData[user].selectedClients[client].speciality);
                        }
                    }
                }
            }
            this.addAllSpecialityOptionToOptionsList();
            this.addAllSystemOptionToOptionsList();
            this.addAllBusinessUnitOptionToOptionsList();
        } else {
            while (iterator < this.unselectedDropdown.length) {
                let makeclientsempty = true;
                switch (this.unselectedDropdown[iterator]) {
                    case 'System':
                        this.systemOptions = [];
                        for (let user in this.allUsersData) {
                            if (makeclientsempty) {
                                this.allUsers[user].selectedClients = [];
                            }
                            for (let client in this.allUsersData[user].selectedClients) {
                                if (lastdropdown === 'Speciality') {
                                    if (this.allUsersData[user].selectedClients[client].speciality.specialityCode == this.selectedSpeciality.specialityCode) {
                                        this.addSystemToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                                if (lastdropdown === 'BusinessUnit') {
                                    if (this.allUsersData[user].selectedClients[client].businessUnit.businessUnitCode == this.selectedBusinessUnit.businessUnitCode) {
                                        this.addSystemToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                                if (lastdropdown === 'Employee') {
                                    if (this.allUsersData[user].email == this.selectedEmployee.email) {
                                        this.addSystemToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                            }
                        }
                        makeclientsempty = false;
                        this.addAllSystemOptionToOptionsList();
                        break;
                    case 'BusinessUnit':
                        this.businessUnitsOptions = [];
                        for (let user in this.allUsersData) {
                            if (makeclientsempty) {
                                this.allUsers[user].selectedClients = [];
                            }
                            for (let client in this.allUsersData[user].selectedClients) {
                                if (lastdropdown === 'System') {
                                    if (this.allUsersData[user].selectedClients[client].system.systemCode == this.selectedSystem.systemCode) {
                                        this.addBusinessUnitToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                                if (lastdropdown === 'Speciality') {
                                    if (this.allUsersData[user].selectedClients[client].speciality.specialityCode == this.selectedSpeciality.specialityCode) {
                                        this.addBusinessUnitToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                                if (lastdropdown === 'Employee') {
                                    if (this.allUsersData[user].email == this.selectedEmployee.email) {
                                        this.addBusinessUnitToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                            }
                        }
                        makeclientsempty = false;
                        this.addAllBusinessUnitOptionToOptionsList();
                        break;
                    case 'Speciality':
                        this.specialityOptions = [];
                        for (let user in this.allUsersData) {
                            if (makeclientsempty) {
                                this.allUsers[user].selectedClients = [];
                            }
                            for (let client in this.allUsersData[user].selectedClients) {
                                if (lastdropdown === 'System') {
                                    if (this.allUsersData[user].selectedClients[client].system.systemCode == this.selectedSystem.systemCode) {
                                        this.addSpecialityToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                                if (lastdropdown === 'BusinessUnit') {
                                    if (this.allUsersData[user].selectedClients[client].businessUnit.businessUnitCode == this.selectedBusinessUnit.businessUnitCode) {
                                        this.addSpecialityToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                                if (lastdropdown === 'Employee') {
                                    if (this.allUsersData[user].email == this.selectedEmployee.email) {
                                        this.addSpecialityToOptionsList(user, client);
                                        this.addClientToUser(user, client);
                                    }
                                }
                            }
                        }
                        makeclientsempty = false;
                        this.addAllSpecialityOptionToOptionsList();
                        break;
                    case 'Employee':
                        this.employeeOptions = [];
                        for (let user in this.allUsersData) {
                            let removeUser = true;
                            for (let client in this.allUsersData[user].selectedClients) {
                                if (lastdropdown === 'System') {
                                    if (this.allUsersData[user].selectedClients[client].system.systemCode == this.selectedSystem.systemCode) {
                                        this.addEmployeeToOptionsList(user);
                                        removeUser = false;
                                    }
                                }
                                if (lastdropdown === 'BusinessUnit') {
                                    if (this.allUsersData[user].selectedClients[client].businessUnit.businessUnitCode == this.selectedBusinessUnit.businessUnitCode) {
                                        this.addEmployeeToOptionsList(user);
                                        removeUser = false;
                                    }
                                }
                                if (lastdropdown === 'Speciality') {
                                    if (this.allUsersData[user].selectedClients[client].speciality.specialityCode == this.selectedSpeciality.specialityCode) {
                                        this.addEmployeeToOptionsList(user);
                                        removeUser = false;
                                    }
                                }
                            }
                            if (removeUser) {
                                this.allUsers.splice(this.allUsers.indexOf(this.allUsersData[user]), 1);
                            }
                        }
                        this.addAllEmployeeOptionToOptionsList();
                        this.removeLoggedInEmployeeFromUserList();
                        break;
                }
                iterator++;
            }
        }
    }

    /**
     * To add All Employee Option To Options List.
     */
    addAllEmployeeOptionToOptionsList() {
        let employeeIndex = this.employeeOptions.findIndex(i => i.email === 'All Employees');
        if (employeeIndex == -1) {
            this.createAllEmployeeObject();
        }
    }

    /**
     * To add All Specilaity Option To Options List.
     */
    addAllSpecialityOptionToOptionsList() {
        let specialityIndex = this.specialityOptions.findIndex(i => i.specialityCode === 'All Specialities');
        if (specialityIndex == -1) {
            this.createAllSpecialityObject();
        }
    }

    /**
    * To add All Business Unit Option To Options List.
    */
    addAllBusinessUnitOptionToOptionsList() {
        let businessUnitIndex = this.businessUnitsOptions.findIndex(i => i.businessUnitCode === 'All Business Units');
        if (businessUnitIndex == -1) {
            this.createAllBusinessUnitObject();
        }
    }

    /**
     * To add All System Option To Options List.
     */
    addAllSystemOptionToOptionsList() {
        let systemIndex = this.systemOptions.findIndex(i => i.systemCode === 'All Systems');
        if (systemIndex == -1) {
            this.createAllSystemObject();
        }
    }

    /**
     * To add client to user based on filter.
     * @param user
     * @param client
     */
    addClientToUser(user: any, client: any) {
        let index = this.allUsers[user].selectedClients.findIndex(i => i.clientCode === this.allUsersData[user].selectedClients[client].clientCode);
        if (index == -1) {
            this.allUsers[user].selectedClients.push(this.allUsersData[user].selectedClients[client]);
        }
    }

    /**
     * To add system to system options.
     * @param user
     * @param client
     */
    addSystemToOptionsList(user: any, client: any) {
        let index = this.systemOptions.findIndex(i => i.systemCode === this.allUsersData[user].selectedClients[client].system.systemCode);
        if (index == -1) {
            this.systemOptions.push(this.allUsersData[user].selectedClients[client].system);
        }
    }

    /**
     * To add business unit to business unit options.
     * @param user
     * @param client
     */
    addBusinessUnitToOptionsList(user: any, client: any) {
        let index = this.businessUnitsOptions.findIndex(i => i.businessUnitCode === this.allUsersData[user].selectedClients[client].businessUnit.businessUnitCode);
        if (index == -1) {
            this.businessUnitsOptions.push(this.allUsersData[user].selectedClients[client].businessUnit);
        }
    }

    /**
     * To add employee to employee options.
     * @param user
     */
    addEmployeeToOptionsList(user: any) {
        let index = this.employeeOptions.findIndex(i => i.email === this.allUsersData[user].email);
        if (index == -1) {
            this.employeeOptions.push(this.allUsersData[user]);
        }
    }

    /**
     * To add speciality to speciality options.
     * @param user
     * @param client
     */
    addSpecialityToOptionsList(user: any, client: any) {
        let index = this.specialityOptions.findIndex(i => i.specialityCode === this.allUsersData[user].selectedClients[client].speciality.specialityCode);
        if (index == -1) {
            this.specialityOptions.push(this.allUsersData[user].selectedClients[client].speciality);
        }
    }


    // Get Heat Map Data for applicable Clients
    getHeatMapOfClients(request) {
        this.heatMapService.getClientsHeatMap(request).subscribe((data) => {
            if (data) {
                if (data.heatmapItems) {
                    this.metricRiskColumns = [];
                    let metricAndRiskFactors = jQuery.parseJSON(data.heatmapItems);
                    this.calculateChecklistTypeColumnCount(metricAndRiskFactors);
                    this.frameMetricAndRiskFactorColumns(metricAndRiskFactors);
                    if (data.heatmaps) {
                        this.clientHeatMaps = jQuery.parseJSON(data.heatmaps);
                    } else {
                        this.clientHeatMaps = null;
                    }
                    this.showHeatMapGrid = true;
                    this.ref.detectChanges();
                    this.calculateSetMaxHeightForColumns();
                    this.dataTable.reset();
                }
            }
        });
    }

    // Calculate Max text from Heat Map Item Name and set max height
    calculateSetMaxHeightForColumns() {
        let containers = <HTMLSelectElement>document.getElementsByClassName('rotate');
        let maxWidth = 0;
        let padding = 30;
        for (let i = 0; i < containers.length; i++) {
            let textContainer = jQuery(containers[i]).find('span').width();
            if (textContainer > maxWidth) {
                maxWidth = textContainer;
            }
        }
        jQuery('th.rotate').css('height', `${maxWidth + padding}px`);

        let spanLeft = jQuery('th.rotate > div > span').css('left');
        if (spanLeft) {
            spanLeft = (maxWidth / 2) - 15;
            jQuery('th.rotate > div > span').css('left', `-${spanLeft}px`);
        }
    }

    // Calculate weekly, monthly checklist questions to show either weekly column or monthly column or both
    calculateChecklistTypeColumnCount(metricAndRiskFactors) {
        let weeklyRiskFactors = metricAndRiskFactors.filter(x => x.HeatMapItemType == this.labels.RISK_FACTOR && x.HeatMapItemMeasureType == this.labels.WEEKLY);
        if (weeklyRiskFactors) {
            this.weeklyRiskFactorCount = weeklyRiskFactors.length;
        }

        let monthlyRiskFactors = metricAndRiskFactors.filter(x => x.HeatMapItemType == this.labels.RISK_FACTOR && x.HeatMapItemMeasureType == this.labels.MONTHLY);
        if (monthlyRiskFactors) {
            this.monthlyRiskFactorCount = monthlyRiskFactors.length;
        }

        this.checklistColumnCount = this.monthlyRiskFactorCount != 0 && this.weeklyRiskFactorCount != 0 ? 2 : 1;
    }

    // Metric & Risk Factor Column headers
    frameMetricAndRiskFactorColumns(metricAndRiskFactors) {
        this.metricRiskColumns = [];
        let _self = this;

        let metrics = metricAndRiskFactors.filter(x => x.HeatMapItemType == this.labels.METRIC);
        metrics.forEach(function (element) {
            _self.metricRiskColumns.push({ header: element.HeatMapItemName, field: element.HeatMapItemName, type: element.HeatMapItemType });
        });
        this.metricRiskColumns.push({ header: 'Score', field: 'MetricScore', type: this.labels.METRIC });

        let riskFactors = metricAndRiskFactors.filter(x => x.HeatMapItemType == this.labels.RISK_FACTOR);
        riskFactors.forEach(function (element) {
            _self.metricRiskColumns.push({ header: element.HeatMapItemName, field: element.HeatMapItemName, type: element.HeatMapItemType });
        });
        this.metricRiskColumns.push({ header: 'Score', field: 'RiskFactorScore', type: this.labels.RISK_FACTOR });
    }

    // Get Heat Map for Clients based on dropdown selection.
    applyFilterToGetClientsHeatMap() {
        let request = new ClientsHeatMapRequestViewModel();
        request.BusinessUnitCode = this.selectedBusinessUnit.businessUnitCode == 'All Business Units' ? null : this.selectedBusinessUnit.businessUnitCode;
        request.SpecialtyCode = this.selectedSpeciality.specialityCode == 'All Specialities' ? null : this.selectedSpeciality.specialityCode;
        request.SystemCode = this.selectedSystem.systemCode == 'All Systems' ? null : this.selectedSystem.systemCode;
        request.EmployeeID = this.selectedEmployee.fullName == 'All Employees' ? null : this.selectedEmployee.userId;
        this.getHeatMapOfClients(request);
    }

    defaultHeatMapData() {
        this.metricRiskColumns = [];
        let request = new ClientsHeatMapRequestViewModel();
        this.getHeatMapOfClients(request);
    }

    /*------ end region public methods ------*/
}




