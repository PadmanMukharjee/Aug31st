
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MessagesModule } from 'primeng/primeng';
import { NgaModule } from '../layout/nga.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { routing } from './client-data.routing';
import { DialogModule } from 'primeng/dialog';
import { ClientStepperComponent } from './client-stepper.component';
import { ClientDataComponent } from './client-data/client-data.component';
import { LogSetupComponent } from './log-setup/log-setup.component';
import { KPISetupComponent } from './kpi-setup/kpi-setup.component';
import { ClientDataService } from './client-data.service';
import { SharedModule } from '../common/common.module';
import { CommonService } from '../common/common.service';
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from 'primeng/table';
import { PayerService } from '../admin/payers/payers.service';
import { HttpModule } from '@angular/http';
import { NgSelectModule } from '@ng-select/ng-select';
import { DropdownModule } from 'primeng/dropdown';
import { FileUploadModule } from 'primeng/fileupload';
import { ModalModule } from 'ngx-modal';
import { PDFViewerInlineComponent } from '../common/components/pdfviewer/pdf-viewer.component';
import { TargetComponent } from './targets/targets.component';
import { TargetService } from './targets/targets.service';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { GrowlModule } from 'primeng/growl';
import { ViewAllClientsComponent } from './view-all-clients/view-all-clients.component';
import { ClientHistoryComponent } from './client-history/client-history.component';
import { ViewAllClientsService } from './view-all-clients/view-all-clients.service';
import { ClientHistoryService } from './client-history/client-history.service';
import { AssignClientUserService } from './assign-client-user/assign-client-user.service';
import { AssignClientUsersComponent } from './assign-client-user/assign-client-user.component';
import { AngularDualListBoxModule } from 'angular-dual-listbox';
import { MultiSelectModule } from 'primeng/multiselect';
import { CheckListService } from '../admin/services/checklist.service';
import { SitesService } from '../admin/sites/sites.service';
import { KPIService } from '../admin/kpi/create-kpi/kpi.service';
import { Daterangepicker } from 'ng2-daterangepicker';
import { StorageServiceModule } from 'angular-webstorage-service';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        HttpModule,
        NgaModule,
        MessagesModule,
        routing,
        Daterangepicker,
        DialogModule,
        CalendarModule,
        TableModule,
        DropdownModule,
        NgSelectModule,
        FileUploadModule,
        ModalModule,
        NgxChartsModule,
        GrowlModule,
        SharedModule,
        AngularDualListBoxModule,
        MultiSelectModule,
        StorageServiceModule
    ],
    declarations: [
        ClientStepperComponent,
        ClientDataComponent,
        LogSetupComponent,
        KPISetupComponent,
        TargetComponent,
        PDFViewerInlineComponent,
        ViewAllClientsComponent,
        AssignClientUsersComponent,
        ClientHistoryComponent
    ],
    providers: [ // expose our Services and Providers into Angular's dependency injection
        ClientDataService,
        CommonService,
        PayerService,
        TargetService,
        ViewAllClientsService,
        AssignClientUserService,
        CheckListService,
        SitesService,
        KPIService,
        ClientHistoryService,
    ]
})
export class ClientStepperModule { }
