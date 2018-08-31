import { HttpUtility } from '../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { ForgotPasswordModel } from './forgotpassword.model';


@Injectable()
export class ForgotPasswordService {
    constructor(
        private httpUtility: HttpUtility) {
    }

    /**
 * send user data to savenewpassword method
 * @param user
 */
    public sendEmailService(forgotPasswordModel: ForgotPasswordModel) {
        return this.httpUtility.get('AllUsers/ValidateUsername?email=' + forgotPasswordModel.email);
    }
}