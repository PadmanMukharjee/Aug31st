import { Injectable } from '@angular/core';

import { HttpUtility } from '../shared/utilities/http/http-utility.service';

@Injectable()
export class TodoListService {

    constructor(private httpUtility: HttpUtility) {}

    /**
     * To get all tasks and clients of a user
     */
    GetClientsForAllToDoTasks() {
        return this.httpUtility.get('Todo/GetClientsForAllToDoTasks');
    }
}
