import { NgModule } from '@angular/core';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GrowlModule } from 'primeng/growl';
import { ClientsHeatMapComponent } from './heatmap.component';
import { ClientsHeatMapService } from './heatmap.service';
import { routing } from './heatmap.routing';

@NgModule({
    imports: [
        CommonModule,
        TableModule,
        routing,
        DropdownModule,
        FormsModule,
        ReactiveFormsModule,
        TooltipModule,
        GrowlModule
    ],
    declarations: [ClientsHeatMapComponent],
    providers: [ClientsHeatMapService]
})
export class ClientsHeatMapModule { }
