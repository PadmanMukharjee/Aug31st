import { Injectable } from '@angular/core';
import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { MenuItemModel } from './models/menuItem.model';


@Injectable()

export class NavMenuService {

    constructor(private httpUtility: HttpUtility) {
    }

    /**
    * To get the left nav menu screens based on role.
    */
    getNavMenuItemsBasedOnRole() {
        return this.httpUtility.get('User/GetScreensOfNavMenuBasedOnRole');
    }

}
