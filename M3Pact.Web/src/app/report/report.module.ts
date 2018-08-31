import { AuthenticationService } from '../shared/services/authentication.service';
import { CommonModule } from '@angular/common';
import { DashBoardService } from '../dashboard/dashboard.service';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgaModule } from '../layout/nga.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DropdownModule } from 'primeng/dropdown';
import { ReportComponent } from './report.component';
import { routing } from './report.routing';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
      routing,
      RouterModule,
      DropdownModule
  ],
  declarations: [
      ReportComponent
  ],
  providers: [
    AuthenticationService,
    DashBoardService
  ]
})
export class ReportModule {
}
