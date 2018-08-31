import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login.component';
import { MessagesModule } from 'primeng/primeng';
import { NgaModule } from '../layout/nga.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { routing } from './login.routing';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { MessageModule } from 'primeng/message';
import { ReCaptchaModule } from 'angular2-recaptcha';
import { ForgotPasswordComponent } from './forgotpassword/forgotpassword.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';
import { SharedModule } from '../common/common.module';

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
        MessageModule,
        ReCaptchaModule,
        SharedModule
    ],
    declarations: [
        LoginComponent,
        ForgotPasswordComponent,
        ResetPasswordComponent
    ]
})
export class LoginModule { }
