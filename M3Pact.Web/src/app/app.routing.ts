import { ErrorPageComponent } from './layout/components/error-page/error-page.component';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from './shared/services/auth-guard.service';

export const routes: Routes = [

    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        loadChildren: 'app/login/login.module#LoginModule'
    },
    {
        path: 'error',
        component: ErrorPageComponent
    },
    {
        path: 'depositlog',
        loadChildren: 'app/deposit-log/deposit-log.module#DepositLogModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'administration',
        loadChildren: 'app/admin/admin.module#AdministrationModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'client',
        loadChildren: 'app/client/client-stepper.module#ClientStepperModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'checklist',
        loadChildren: 'app/checklist/checklist.module#ChecklistModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'dashboard',
        loadChildren: 'app/dashboard/dashboard.module#DashboardModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'reports',
        loadChildren: 'app/report/report.module#ReportModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'heatmap',
        loadChildren: 'app/heatmap/heatmap.module#ClientsHeatMapModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'todo',
        loadChildren: 'app/todo-list/todo-list.module#TodoListModule',
        canActivate: [AuthGuardService]
    },
    { path: '**', redirectTo: 'error' }
];

export const routing = RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' ,  useHash: false });
