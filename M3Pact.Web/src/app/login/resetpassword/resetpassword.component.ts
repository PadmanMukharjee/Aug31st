// Angular imports
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Message } from 'primeng/components/common/api';
import { AbstractControl, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
// Common File Imports
import { ResetPasswordService } from './resetpassword.service';
import { LOGIN, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { ResetPasswordModel } from './resetpassword.model';
import { UserLogin } from '../../shared/models/user-login';
import { NgForm } from '@angular/forms';
import { AppConstants } from '../../app.constants';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { UserService } from '../../shared/services/index';
import { User } from '../../shared/models/index';
import { GlobalEventsManager } from '../../shared/utilities/global-events-manager';

@Component({
    selector: 'ResetPassword',
    templateUrl: './resetpassword.component.html',
    providers: [ResetPasswordService],
    styleUrls: ['../login.css']
})

export class ResetPasswordComponent implements OnInit {

    /*------ region public properties ------*/
    public resetPasswordForm: FormGroup;
    public ResetPasswordModel: ResetPasswordModel;
    public resetPasswordLabels = LOGIN;
    public sharedLabels = ADMIN_SHARED;
    public msgs: Message[] = [];
    public user: UserLogin = new UserLogin();
    public misMatchMsg: string;
    public displayDialog: boolean;
    public displayLabel: string;
    public dialogueMsg: string;
    public resetPwdConstants: any;
    public newPassword: AbstractControl;
    public confirmPassword: AbstractControl;
    public currentUser: User;

    /*------ end region public properties ------*/


    /*------ region constructor ------*/
    constructor(private resetPasswordService: ResetPasswordService, private router: Router, private _userService: UserService, private _globalEventsManager: GlobalEventsManager) {
        this.resetPwdConstants = AppConstants.resetPasswordConstants;
        this.buildForm();
        this.currentUser = this._userService.getCurrentUser();
        if (this.currentUser != null) {
            localStorage.clear();
            sessionStorage.clear();
            this._userService.removeCurrentUser();
            // Load layout after logged in
            this._globalEventsManager.isUserLoggedIn(false);
        }
    }


    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.getLoginModelData();
    }

    /*------end  region life cycle hooks ------*/

    /*------ region Public methods ------*/

    /**
     * gets usertid and token from url
     */
    public getLoginModelData(): any {
        let url = decodeURIComponent(window.location.href);
        let userPosition = url.indexOf(AppConstants.resetPasswordConstants.userId) + 8;
        if (userPosition > 7) {
            let tokenPosition = url.indexOf(AppConstants.resetPasswordConstants.token);
            let userId = url.substring(userPosition, tokenPosition);
            let forgotToken = url.substring(tokenPosition + 7, url.length);
            this.user.userId = window.atob(userId);
            this.user.forgotPasswordToken = forgotToken;
        }
    }

    /**
     * validates and sends password to back end
     * @param form
     */
    public validatePassword(form) {
        this.markFormGroupTouched(this.resetPasswordForm);
        if (form.valid) {
            if (form.value.newPassword === form.value.confirmPassword) {
                this.user.password = form.value.newPassword;
                this.resetPasswordService.updatePassword(this.user).subscribe(
                    (responce) => {
                        if (responce.success) {
                            this.displayDialog = true;
                            this.dialogueMsg = responce.successMessage;
                            this.displayLabel = AppConstants.resetPasswordConstants.sucessDisplayLabel;
                        } else {
                            this.displayDialog = true;
                            this.dialogueMsg = responce.errorMessages[0];
                            this.displayLabel = AppConstants.resetPasswordConstants.errorDisplayLabel;
                        }
                    }
                );
            } else {
                this.misMatchMsg = AppConstants.resetPasswordConstants.misMatchMsg;
            }
        }
    }


    /**
     * buids a reactive form with given controls
     */
    public buildForm(): void {
        this.resetPasswordForm = new FormGroup({
            newPassword: new FormControl('', [
                Validators.minLength(8),
                Validators.required,
                Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,}'),
                this.checkIfEmpty
            ]),
            confirmPassword: new FormControl('', [
                Validators.minLength(8),
                Validators.required,
                Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,}'),
                this.checkIfEmpty
            ]),
        });

        this.newPassword = this.resetPasswordForm.controls['newPassword'];
        this.confirmPassword = this.resetPasswordForm.controls['confirmPassword'];
    }

    /**
     * checks field is empty or not
     * @param fieldControl
     */
    checkIfEmpty(fieldControl: FormControl) {
        let length = 0;
        if (fieldControl.value != null) {
            length = fieldControl.value.trim().length;
        }
        return length !== 0 ? null : { isEmpty: true };
    }

    /**
   * close and redirect.
   */
    public close() {
        this.displayDialog = false;
        this.router.navigateByUrl('login');
    }

    /**
     * clear button click clearing fileds
     */
    public clear() {
        this.buildForm();
    }



    /*------ end region Public methods ------*/

    /*------ region private methods ------*/

    /**
    * marks all controllers as touched in form
    * @param formGroup
    */
    private markFormGroupTouched(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {
            control.markAsTouched();

            if (control.controls) {
                control.controls.forEach(c => this.markFormGroupTouched(c));
            }
        });
    }

    /*------ end region private methods ------*/
}
