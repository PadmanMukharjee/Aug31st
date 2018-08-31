import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientStepperComponent } from './client-stepper.component';
import { ViewAllClientsComponent } from './view-all-clients/view-all-clients.component';
import { ClientHistoryComponent } from './client-history/client-history.component';

// Routes
export const routes: Routes = [
    {
        path: '',
        component: ClientStepperComponent
    },
    {
        path: 'ViewAllClients',
        component: ViewAllClientsComponent
    },
    {
        path: 'client-history',
        component: ClientHistoryComponent
    }
];
export const routing = RouterModule.forChild(routes);
