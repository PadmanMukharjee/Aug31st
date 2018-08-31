// Angular imports
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Message } from 'primeng/components/common/api';
import { AbstractControl, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

// Common File Imports
import { ForgotPasswordService } from './forgotpassword.service';
import { LOGIN, ADMIN_SHARED } from '../../shared/utilities/resources/labels';
import { ForgotPasswordModel } from './forgotpassword.model';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { AppConstants } from '../../app.constants';
import { Router } from '@angular/router';

@Component({
    selector: 'ForgotPassword',
    templateUrl: './forgotpassword.component.html',
    providers: [ForgotPasswordService],
    styleUrls: ['../login.css']
})

export class ForgotPasswordComponent implements OnInit {
    /*------ region public properties ------*/
    public forgotPasswordForm: FormGroup;
    public forgotPasswordModel: ForgotPasswordModel;
    public fortgotPasswordLabels = LOGIN;
    public sharedLabels = ADMIN_SHARED;
    public msgs: Message[] = [];
    public displayDialog: boolean;
    public dialogueMsg: string;
    public displayLabel: string;
    public email: AbstractControl;
    public forgotPwdConstants: any;

    /*------end region public properties ------*/

    /*------ region constructor ------*/
    constructor(private forgotPasswordService: ForgotPasswordService, private router: Router) {
        this.displayDialog = false;
        this.forgotPasswordModel = new ForgotPasswordModel();
        this.buildForm();
        this.forgotPwdConstants = AppConstants.forgotPwdConstants;
    }

    /*------ end region constructor ------*/


    /*-----region lifecycle events -----*/
    ngOnInit() {

    }

    /*-----end region lifecycle events -----*/

    /*------ region Public methods ------*/



    /**
     * buids a reactive form with given controls
     */
    public buildForm(): void {
        this.forgotPasswordForm = new FormGroup({
            email: new FormControl('', [
                Validators.required,
                Validators.pattern('[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,3}$'),
                this.checkIfEmpty
            ])
        });

        this.email = this.forgotPasswordForm.controls['email'];
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
     * sending email
     * @param form
     */
    public sendEmail(form) {
        this.markFormGroupTouched(this.forgotPasswordForm);
        if (form.valid) {
            this.forgotPasswordModel.email = form.value.email;
            this.forgotPasswordService.sendEmailService(this.forgotPasswordModel).subscribe(
                (responce) => {
                    if (responce) {
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
                }
            );
        }
    }

    /**
 * close and redirect.
 */
    public close() {
        this.displayDialog = false;
        this.router.navigateByUrl('login');
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

    /*------end region private methods ------*/

}
