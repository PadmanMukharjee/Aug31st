import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    Validators,
    FormControl
} from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import {
    animate,
    group,
    style,
    transition,
    trigger
} from '@angular/animations';
import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewEncapsulation,
    Inject
} from '@angular/core';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { loginAnimation } from '../shared/utilities/animations/shared-animations';
import { ResponseModel, User} from '../shared/models';
import { UserService } from '../shared/services';
import { AlertService, AuthenticationService } from '../shared/services';
import { AppConstants } from '../app.constants';
import { LOGIN, ADMIN_SHARED } from '../shared/utilities/resources/labels';
import { ReCaptchaModule } from 'angular2-recaptcha';
import { Message } from 'primeng/components/common/api';
import { UserLogin } from '../shared/models/user-login';
import { ValidationResponseViewModel } from '../common/models/validation.model';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { LoginService } from './login.service';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.css'],
    animations: [
        loginAnimation
    ],
    providers: [LoginService]
})
export class LoginComponent {

    /*------ region public properties ------*/
    public loginForm: FormGroup;
    public email: AbstractControl;
    public password: AbstractControl;
    public model: any = {};
    public loading = false;
    public returnUrl: string;
    public currentUser: User;
    public displayMessages: any = [];
    public labels = LOGIN;
    public msgs: Message[] = [];
    public userType: string;
    public userModel: UserLogin;
    public loginConstants: any;
    public testAccounts: any[];

    /*------ end region public properties ------*/

    /*------ region constructor ------*/

    constructor(

        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private _userService: UserService,
        private loginService: LoginService,
        private formBuilder: FormBuilder,
        private _globalEventsManager: GlobalEventsManager,
        @Inject(LOCAL_STORAGE) private storage: WebStorageService
    ) {
        this.testAccounts = this.authenticationService.getTestAccounts();
        this.userModel = new UserLogin();
        this.currentUser = this._userService.getCurrentUser();
        this.loginConstants = AppConstants.loginConstants;
        // Check whether user exists or not. If user exists redirect to landing page directly, dont show login page.
        // Navigate to depositlog for now as there is no landing page as of now.
        if (this.currentUser != null && (this.currentUser.userType == 'User' || this.currentUser.userType == 'Client')) {
            this.returnUrl = '/depositlog';
            this.router.navigate(['/depositlog']);

        } else if (this.currentUser != null) {
            this.returnUrl = '/dashboard';
            this.router.navigate(['/dashboard']);
        }
        let url = this.route.snapshot.queryParams['returnUrl'] || '/depositlog';
        let parameters = url.split('?');
        // Navigate to Specified URL
        this.returnUrl = parameters[0];
        if (parameters && parameters.length > 1) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: this.returnUrl }, skipLocationChange: false });
        }
    }

    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    public ngOnInit() {
        this.buildForm();
        this.userType = this.labels.M3USERVALUE;
        if (this.currentUser != null) {
            this.router.navigate([this.returnUrl]);
        } else {
            this.authenticationService.logout();
        }
    }
    /*------ end region life cycle hooks ------*/

    /*------ region service calls------*/

    /* Fires on click of Login Button*/
    onLogin(form) {
        this.markFormGroupTouched(this.loginForm);        
        if (this.testAccounts.includes(form.value.email) && form.value.password !== '') {
            this.validateUser(form);
        } else if (form.valid && this.userType) {
            this.validateUser(form);
        }
    }

    /* for validationg selected user type is correct or not*/
    public validateUser(form) {
        if (this.userType.trim() === this.labels.CLIENTUSERVALUE) {
            this.userModel.isMeridianUser = false;
        } else {
            this.userModel.isMeridianUser = true;
        }
        this.userModel.userName = form.value.email;
        this.userModel.email = form.value.email;
        this.userModel.password = form.value.password;
        this.loginService.checkUser(this.userModel).subscribe(
            (response) => {
                if (response.success) {
                    this.login({
                        email: response.additionalInfo,
                        password: this.userModel.password,
                        userType: this.userType.trim()
                    } as User);
                } else {
                    this.displayMessages.push({ severity: 'error', summary: this.loginConstants.invalid, detail: '' });
                }
            }
        );
    }

    /* To call authentication server*/
    public login(loginData: User) {
        this.loading = true;
        // severity: success,info,warn,error
        this.displayMessages = [];
        this.userModel.userName = loginData.email;
        this.authenticationService.getAuthenticationToken(loginData).subscribe(
            tokenResp => {
                if (tokenResp && tokenResp['access_token']) {
                    this.currentUser = new User();
                    this.currentUser.email = loginData.email;
                    this.currentUser.accessToken = tokenResp['access_token'];

                    // Store the user in sessionstorage
                    this._userService.setCurrentUser(this.currentUser);
                    
                    let jwt = tokenResp['access_token'];
                    let jwtData = jwt.split('.')[1];
                    let decodedJwtJsonData = window.atob(jwtData);
                    let decodedJwtData = JSON.parse(decodedJwtJsonData);
                    let role = decodedJwtData.role;
                    this._userService.setRole(role);

                    // Load layout after logged in
                    this._globalEventsManager.isUserLoggedIn(true);
                    this.loading = false;

                    // Redirect the user after login
                    if (role == 'User' || role == 'Client') {
                        this.router.navigate([this.returnUrl]);
                    } else {
                        this.router.navigate(['/dashboard']);
                    }
                } else {
                    this.loading = false;
                    this.displayMessages.push({ severity: 'error', summary: AppConstants.loginConstants.invalid, detail: '' });
                }
            },
            (error) => {
                this.loading = false;
                this.displayMessages.push({ severity: 'error', summary: AppConstants.loginConstants.invalid, detail: '' });
            },
            () => {
                this.loading = false;
            }
        );
    }

    /**
     * buids a reactive form with given controls
     */
    public buildForm(): void {
        this.loginForm = new FormGroup({
            email: new FormControl('', [
                Validators.required,
                Validators.pattern('^[0-9]*$'),
                this.checkIfEmpty,
                this.validateTestAccounts
            ]),
            password: new FormControl('', [
                Validators.minLength(8),
                Validators.required,
                this.checkIfEmpty
            ]),
        });

        this.email = this.loginForm.controls['email'];
        this.password = this.loginForm.controls['password'];
    }

    onUserTypeClient() {
        this.loginForm.get('email').clearValidators();
        this.loginForm.get('email').setValidators([Validators.required,
        Validators.pattern('[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,3}$'),
        this.checkIfEmpty,
        this.validateTestAccounts]);
        this.loginForm.get('email').updateValueAndValidity();
    }

    onUserTypeM3user() {
        this.loginForm.get('email').clearValidators();
        this.loginForm.get('email').setValidators([Validators.required,
        Validators.pattern('^[0-9]*$'),
        this.checkIfEmpty,
        this.validateTestAccounts]);
        this.loginForm.get('email').updateValueAndValidity();
    }
    // Validation Test accounts of M3 User
    validateTestAccounts(fieldControl: FormControl) {
        let isNotATestAccount = true;
        if (fieldControl.value != null) {
            if (fieldControl.value === '600000022' || fieldControl.value === '600000023' || fieldControl.value === '600000024' || fieldControl.value === '600000025') {
                isNotATestAccount = false;
            }
        }
        return isNotATestAccount ? null : { testAccount: true };
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
    /*------ end region service calls------*/

    /*------ region private methods------*/

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

    /*------ end region private methods------*/
}
