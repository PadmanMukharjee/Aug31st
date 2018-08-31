const User = {
    currentUser: 'currentUser',
    role: 'role'
};

const Common = {
    loginUrl: '/asdfasdfasdf',
    dashBoard: '/dashboard',
    token: 'connect/token'
};

const CommonErrors = {
    login_Failed: 'Login failed',
    api_Response_Failed: 'API Response failed',
    error_occured_while_login: 'Error occured while login',
    delete_confirmation: 'Are you sure you want to delete this item?',
};

export const UserInterests = {
    maxRows: 10,
    pageLinks: 3,
    searchPlaceholderText: 'Search interests eg. Cricket',
};

export const ForgotPasswodConstants = {
    invalidEmail: 'Email is invalid',
    emailRequired: 'Email is required'
};

export const ResetPasswordConstants = {
    misMatchMsg: 'Passwords don\'t Match',
    sucessDisplayLabel: 'Sucess',
    errorDisplayLabel: 'Error',
    userId: '?userid=',
    token: '&token=',
    invalidPwd: 'Password is invalid',
    passwordRequired: 'Password is required',
    pwdMinLength: 'Password must be 8 characters long',
    cnfrmPasswordRequired: 'Confirmaion Password is required'
};

export const LoginConstants = {
    invalid: 'Invalid Credentials',
    invalidEmail: 'Email is invalid',
    emailRequired: 'Email is required',
    passwordRequired: 'Password is required',
    pwdMinLength: 'Password must be 8 characters long',
    invalidSSO: 'SSO is invalid',
    ssoRequired: 'SSO is required'
};

export const ClientUserConstants = {
    confirMationMsg: 'Are you sure you want to make the client user inactive?',
    sucessMsg: 'Saved Sucessfully',
    client: 'Client',
    errorMsg: 'Somthing went wrong, Not saved'
};

export const M3UserConstants = {
    confirMationMsg: 'Are you sure you want to make the M3 user inactive?'
};

export const BusinessDaysConstants = {
    Saturday: 6,
    Sunday: 0
};

export const KPIConstants = {
    confirmationMsg: 'Selected KPI is already created. Do you want to edit?'
};

export const AppConstants = {
    common: Common,
    user: User,
    commonErrors: CommonErrors,
    userInterests: UserInterests,
    resetPasswordConstants: ResetPasswordConstants,
    loginConstants: LoginConstants,
    clientUseConstants: ClientUserConstants,
    forgotPwdConstants: ForgotPasswodConstants,
    m3UserConstants: M3UserConstants,
    kpiConstants: KPIConstants
};

