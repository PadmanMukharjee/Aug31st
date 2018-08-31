import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NgxGaugeModule } from 'ngx-gauge';
import { MessagesModule, AutoCompleteModule, RadioButtonModule } from 'primeng/primeng';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MultiSelectModule } from 'primeng/multiselect';
import { AngularDualListBoxModule } from 'angular-dual-listbox';

import { routing } from './admin.routing';
import { SharedModule } from '../common/common.module';

import { SitesComponent } from './sites/sites.component';
import { BusinessUnitsComponent } from './business-units/business-units.component';
import { SpecialitiesComponent } from './specialities/specialities.component';
import { SystemsComponent } from './systems/systems.component';
import { PayersComponent } from './payers/payers.component';
import { GrowlModule } from 'primeng/growl';
import { MthreeUsersComponent } from './users/m3user/m3-user.component';
import { AllUsersComponent } from './users/all-users/all-users.component';
import { ClientUsersComponent } from './users/clientuser/client-user.component';
import { BusinessDaysComponent } from './business-days/business-days.component';
import { ChecklistComponent } from './checklist/checklist.component';
import { ChecklistitemComponent } from './checklistitem/checklistitem.component';
import { CheckListService } from './services/checklist.service';
import { ChecklistResolve } from './guards/resolve/checklist.resolve';
import { ViewAllChecklistComponent } from './view-all-checklist/view-all-checklist.component';
import { KPIComponent } from './kpi/create-kpi/kpi.component';
import { ViewKPIComponent } from './kpi/view-kpi/view-kpi.component';
import { HeatmapComponent } from './heatmap/heatmap.component';
import { HeatmapService } from './heatmap/heatmap.service';
import { ConfigurationComponent } from './configuration/configuration.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        MessagesModule,
        routing,
        DialogModule,
        TableModule,
        NgxGaugeModule,
        DropdownModule,
        GrowlModule,
        MultiSelectModule,
        CalendarModule,
        NgxChartsModule,
        SharedModule,
        AutoCompleteModule,
        HttpModule,
        RadioButtonModule,
        ButtonModule,
        AngularDualListBoxModule
    ],
    declarations: [
        SitesComponent,
        BusinessUnitsComponent,
        SpecialitiesComponent,
        PayersComponent,
        SystemsComponent,
        MthreeUsersComponent,
        AllUsersComponent,
        ClientUsersComponent,
        BusinessDaysComponent,
        ChecklistitemComponent,
        ChecklistComponent,
        ViewAllChecklistComponent,
        KPIComponent,
        ViewKPIComponent,
        HeatmapComponent,
        ConfigurationComponent,
    ],
    providers: [
        CheckListService,
        ChecklistResolve,
        HeatmapService
    ],
    exports: [ChecklistitemComponent]
})
export class AdministrationModule { }
