import { LoginComponent } from './login.component';
import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForgotPasswordComponent } from './forgotpassword/forgotpassword.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';

// Routes
export const routes: Routes = [
    {
        path: '',
        component: LoginComponent
    },
    {
        path: 'forgotpassword',
        component: ForgotPasswordComponent
    },
    {
        path: 'resetpassword',
        component: ResetPasswordComponent
    }
];
export const routing = RouterModule.forChild(routes);
