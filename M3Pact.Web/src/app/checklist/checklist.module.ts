import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routing } from './checklist.routing';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { Daterangepicker } from 'ng2-daterangepicker';
import { TableModule } from 'primeng/table';
import { DpDatePickerModule } from 'ng2-date-picker';
import { GrowlModule } from 'primeng/growl';
import { DialogModule } from 'primeng/dialog';

import { PendingChecklistComponent } from './pending-checklist/pending-checklist.component';
import { CompletedChecklistComponent } from './completed-checklist/completed-checklist.component';
import { ChecklistService } from './checklist.service';
import { CanDeactivateGuard } from '../shared/services/deactivate-guard';

@NgModule({
  imports: [
      CommonModule,
      RouterModule,
      routing,
      Daterangepicker,
      RadioButtonModule,
      InputTextModule,
      FormsModule,
      DropdownModule,
      TableModule,
      DpDatePickerModule,
      GrowlModule,
      DialogModule
  ],
  declarations: [PendingChecklistComponent, CompletedChecklistComponent],
  providers: [
      ChecklistService,
      CanDeactivateGuard
  ]
})
export class ChecklistModule { }
