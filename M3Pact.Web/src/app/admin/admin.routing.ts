import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SitesComponent } from './sites/sites.component';
import { BusinessUnitsComponent } from './business-units/business-units.component';
import { PayersComponent } from './payers/payers.component';
import { SpecialitiesComponent } from './specialities/specialities.component';
import { MthreeUsersComponent } from './users/m3user/m3-user.component';
import { AllUsersComponent } from './users/all-users/all-users.component';
import { ClientUsersComponent } from './users/clientuser/client-user.component';
import { BusinessDaysComponent } from './business-days/business-days.component';
import { SystemsComponent } from './systems/systems.component';
import { ChecklistitemComponent } from './checklistitem/checklistitem.component';
import { ChecklistComponent } from './checklist/checklist.component';
import { KPIComponent } from './kpi/create-kpi/kpi.component';

import { ChecklistResolve } from './guards/resolve/checklist.resolve';
import { ViewAllChecklistComponent } from './view-all-checklist/view-all-checklist.component';
import { ViewKPIComponent } from './kpi/view-kpi/view-kpi.component';
import { HeatmapComponent } from './heatmap/heatmap.component';
import { AuthGuardService } from '../shared/services/auth-guard.service';
import { ConfigurationComponent } from '../admin/configuration/configuration.component';

// Routes
export const routes: Routes = [
    {
        path: 'businessdays',
        component: BusinessDaysComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'businessunits',
        component: BusinessUnitsComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'sites',
        component: SitesComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'payers',
        component: PayersComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'systems',
        component: SystemsComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'specialties',
        component: SpecialitiesComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'users/m3users',
        component: MthreeUsersComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'users/allusers',
        component: AllUsersComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'users/clientusers',
        component: ClientUsersComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'users/clientusers/:id',
        component: ClientUsersComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'users/m3users/:id',
        component: MthreeUsersComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'checklistitems',
        component: ChecklistitemComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'checklist',
        component: ChecklistComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'checklist/:id',
        component: ChecklistComponent,
        resolve: { item: ChecklistResolve },
        canActivate: [AuthGuardService]
    },
    {
        path: 'checklists/viewallchecklists',
        component: ViewAllChecklistComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'kpi/createKpi',
        component: KPIComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'kpi/createKpi/:id',
        component: KPIComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'kpi/viewKpi',
        component: ViewKPIComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'heatmap',
        component: HeatmapComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'config',
        component: ConfigurationComponent,
        canActivate: [AuthGuardService]
    }

];
export const routing = RouterModule.forChild(routes);
