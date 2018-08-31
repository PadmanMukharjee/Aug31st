import { RouterModule, Routes } from '@angular/router';

import { PendingChecklistComponent } from './pending-checklist/pending-checklist.component';
import { CompletedChecklistComponent } from './completed-checklist/completed-checklist.component';
import { CanDeactivateGuard } from '../shared/services/deactivate-guard';

export const routes: Routes = [
    {
        path: 'pending',
        component: PendingChecklistComponent,
        canDeactivate: [CanDeactivateGuard]
    },
    {
        path: 'completed',
        component: CompletedChecklistComponent
    },
];

export const routing = RouterModule.forChild(routes);
