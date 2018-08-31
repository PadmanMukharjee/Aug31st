import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DepositLogComponent } from './deposit-log.component';
import { CanDeactivateGuard } from '../shared/services/deactivate-guard';

// Routes
export const routes: Routes = [
    {
        path: '',
        component: DepositLogComponent,
        canDeactivate: [CanDeactivateGuard]
    }
];
export const routing = RouterModule.forChild(routes);
