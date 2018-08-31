// Third party imports
import { Message } from 'primeng/components/common/api';

// Angular imports
import { Component, OnInit, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

// Common File Imports
import { NavMenuService } from './nav-menu.service';
import { MenuItemModel } from './models/menuItem.model';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { ADMIN_SHARED } from '../shared/utilities/resources/labels';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css'],
    host: {
        '(document:click)': 'functionClick($event)',
    },
    providers: [NavMenuService ]
})
export class NavMenuComponent implements OnInit {

    /*------ region private properties ------*/
    private router: Router;
    /*------ end region private properties ------*/

    /*---------- region constructor ----------*/
    constructor(private navMenuService: NavMenuService,
        private routerIn: Router = null,
        private element: ElementRef,
        private _globalEventsManager: GlobalEventsManager,
        private location: Location
    ) {
        this.router = routerIn;
    }
    /*-------- end region constructor --------*/

    /*------  region public properties ------*/

    public MenuItems: MenuItemModel[];
    public selectedMenu: MenuItemModel = new MenuItemModel();
    public selectedchildMenu: MenuItemModel = new MenuItemModel();
    public messages: Message[] = [];
    public sharedLabels = ADMIN_SHARED;

    /*------ end region public properties ------*/

    /*------ region life cycle hooks ------*/

    ngOnInit() {
        this.getMenuItems();
    }

    /*------end region life cycle hooks ------*/

    /*-------- region public methods --------*/

   functionClick(event) {
        if (event.target.parentNode.tagName == 'A' || event.target.parentNode.tagName == 'LI') {

        } else if (document.getElementsByClassName('nav-sm').length == 1) {
            this.parentMenuClick(this.selectedMenu);
        }
    }
    public parentMenuClick(menuItem: MenuItemModel) {
        this.expandCollapseSubNodes(menuItem);
        return this.menuClick(menuItem);
    }
    public childMenuClick(menuItem: MenuItemModel) {
        if (this.selectedchildMenu.nodeName != menuItem.nodeName) {
            this.selectedchildMenu.expand = !this.selectedchildMenu.expand;
            menuItem.expand = !menuItem.expand;
            this.selectedchildMenu = menuItem;
        } else {
            this.selectedchildMenu.expand = !this.selectedchildMenu.expand;
            this.selectedchildMenu = new MenuItemModel();
        }

        return this.menuClick(menuItem);
    }

    public menuClick(menuItem: MenuItemModel) {
        if (menuItem.url) {
            if (menuItem.url.indexOf('administration') >= 0 || menuItem.url.indexOf('dashboard') >= 0 || menuItem.url.indexOf('reports') >= 0  || menuItem.info == 'create' || menuItem.info == 'viewClient') {
                this._globalEventsManager.setClientDropdown(false);
            } else {
                this._globalEventsManager.setClientDropdown(true);
            }
            if (menuItem.info == 'create') {
                this._globalEventsManager.setClientMode('create');
                this.location.replaceState('/client');
            } else if (menuItem.info == 'edit') {
                this._globalEventsManager.setClientMode('edit');
                this.location.replaceState('/client');
            }
            // let url = menuItem.url + "/" + menuItem.nodeId;
            this.router.navigateByUrl(menuItem.url);
        }
        return false;
    }


    /**
       * show/hide caret symbol
       * @param menuItem
       */
    public showCaret(menuItem: MenuItemModel) {
        if (menuItem && menuItem.subNodes && menuItem.subNodes.length > 0) {
            return true;
        }
        return false;
    }

    /**
     * gets caret class
     * @param menuItem
     */
    public getCaretClass(menuItem: MenuItemModel) {
        if (menuItem.expand == true) {
            return 'fa fa-angle-down';
        }
        return 'fa fa-angle-left';
    }

    /*------ end region public methods------*/

    /*------ region private methods------*/

    /**
      * Toggles expand collapse
      * @param menuItem
      */
    private expandCollapseSubNodes(menuItem: MenuItemModel) {
        if (this.selectedMenu.nodeName != menuItem.nodeName) {
            this.selectedMenu.expand = !this.selectedMenu.expand;
            menuItem.expand = !menuItem.expand;
            this.selectedMenu = menuItem;
        } else {
            this.selectedMenu.expand = !this.selectedMenu.expand;
            this.selectedMenu = new MenuItemModel();

        }
    }

    /*------ end region private methods------*/

    /*------ region service calls------*/

    /**
      * Gets Menu Items
      */
    private getMenuItems() {
        this.navMenuService.getNavMenuItemsBasedOnRole().subscribe(
           (data) => {
                if (data.success === true) {
                    this.MenuItems = data.listOfMenuItemViewModel;
                } else {
                    this.messages = [];
                    this.messages.push({ severity: 'error', summary: ADMIN_SHARED.ERROR, detail: ADMIN_SHARED.ERROR_GET_DETAILS });
                }
            },
            err => { },
            () => { }
        );
    }

    /*------ end region service calls ------*/
}
