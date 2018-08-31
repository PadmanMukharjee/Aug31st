import { Routes, RouterModule } from '@angular/router';
import { ClientsHeatMapComponent } from './heatmap.component';

export const routes: Routes = [
    {
        path: '',
        component: ClientsHeatMapComponent
    }
];

export const routing = RouterModule.forChild(routes);