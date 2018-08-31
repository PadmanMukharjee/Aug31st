import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';

import { TodoListModel, TodoListClient } from './models/todo-list.model';
import { TodoListService } from './todo-list.service';
import { RedirectURL } from './enums/redirect-url.enum';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { TODO_LIST, SHARED } from '../shared/utilities/resources/labels'

import { Message } from 'primeng/components/common/message';

@Component({
    templateUrl: './todo-list.component.html',
    styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

    /* region Properties */
    public todoList: TodoListModel[];
    public collapsed: boolean[] = [];
    public labels = TODO_LIST;
    public msgs: Message[] = [];
    /*end region properties */

    /* region Constructor */
    constructor(private _service: TodoListService, private router: Router,
        private _global: GlobalEventsManager) { }
    /* end region Constuctor */

    /* region Life cycle hooks */
    ngOnInit() {
        this._global.setClientDropdown(false);
        this._service.GetClientsForAllToDoTasks().subscribe(
            (resp) => {
                if (resp != null && resp.length > 0) {
                    this.divideTasksFromResponse(resp);
                } else if (resp != null && resp.length == 0) {
                    jQuery('#todo').removeClass('alert1');
                } else {
                    jQuery('#todo').removeClass('alert1');
                    this.msgs = [];
                    this.msgs.push({ severity: SHARED.SEVERITY_ERROR, summary: '', detail: SHARED.ERROR_GET_DETAILS });
                }
            }
        )
    }
    /* end region life cycle hooks */


    /* region public methods

    /**
     * expand or collapse user tasks
     * @param index
     */
    public expandOrCollapse(index) {
        let existingState = this.collapsed[index];
        this.collapsed.fill(false);
        this.collapsed[index] = !existingState;
    }

    /**
     * To redirect to other pages
     * @param taskName
     * @param clientCode
     * @param clientName
     */
    public redirect(taskName: string, clientCode, clientName) {
        let URL = RedirectURL[taskName];
        if (clientCode != undefined || clientName != undefined) {
            if (taskName == 'Unclosed Month') {
                this._global.showCloseMonthPage(true);
            }
            if (taskName == 'Pending Monthly Checklist') {
                this._global.showPendingChecklistMonthly(true);
            }
            if (taskName != 'Partially Completed Clients') {
                this._global.setGlobalClientCode({ label: clientName, value: clientCode });
            }
            else {
                this._global.setClientMode('editPartial');
                this._global.setClientCode({ label: clientName, value: clientCode });
            }
        }
        this.router.navigateByUrl(URL);
    }
    /* end region public methods */


    /* region Private methods */

    /**
     * tasks will be divided based on task name
     * @param resp
     */
    private divideTasksFromResponse(resp) {
        let tasks = new Set(resp.map(item => item.taskName));
        this.todoList = [];
        let self = this;
        tasks.forEach(function (task) {
            let todoItem = new TodoListModel;
            todoItem.taskName = task.toString();
            let found = resp.filter(function (element) {
                return element.taskName == todoItem.taskName;
            });
            let todoListClients = [];
            for (let client of found) {
                if (client.clientName != "") {
                    let todoListClient = new TodoListClient;
                    todoListClient.clientName = client.clientName;
                    todoListClient.clientCode = client.clientCode;
                    todoListClients.push(todoListClient);
                }
            }
            todoItem.clients = todoListClients;
            self.todoList.push(todoItem);
        });
    }
    /* end region private methods */
}
