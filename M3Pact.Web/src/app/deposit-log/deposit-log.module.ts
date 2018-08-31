import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MessagesModule } from 'primeng/primeng';
import { NgaModule } from '../layout/nga.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { routing } from './deposit-log.routing';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { ChartModule } from 'primeng/primeng';
import { DepositLogComponent } from './deposit-log.component';
import { DepositLogService } from './deposit-log.service';
import { NgxGaugeModule } from 'ngx-gauge';
import { CalendarModule } from 'primeng/calendar';
import { SharedModule } from '../common/common.module';
import { CommonService } from '../common/common.service';
import { DpDatePickerModule } from 'ng2-date-picker';
import { MomentModule } from 'angular2-moment';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { ComboChartComponent } from './combo-chart/combo-chart.component';
import { ComboSeriesVerticalComponent } from './combo-chart/combo-series-vertical.component';
import { MessageModule } from 'primeng/message';
import { GrowlModule } from 'primeng/growl';
import { MultiSelectModule } from 'primeng/multiselect';
import { CanDeactivateGuard } from '../shared/services/deactivate-guard';


import { DropdownModule } from 'primeng/dropdown';



@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        NgaModule,
        MessagesModule,
        routing,
        DialogModule,
        TableModule,
        NgxGaugeModule,
        NgxChartsModule,
        SharedModule,
        ChartModule,
        CalendarModule,
        DpDatePickerModule,
        MomentModule,
        MessageModule,
        GrowlModule,
        MultiSelectModule,
DropdownModule
    ],
    declarations: [
        DepositLogComponent,
        ComboChartComponent,
        ComboSeriesVerticalComponent
    ],
    providers: [ // expose our Services and Providers into Angular's dependency injection
        DepositLogService,
        CommonService,
        CanDeactivateGuard
    ]
})
export class DepositLogModule { }
