import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TableModule } from 'primeng/table';
import { GrowlModule } from 'primeng/growl';

import { TodoListComponent } from './todo-list.component';
import { routing } from './todo-list.routing';
import { TodoListService } from './todo-list.service';

@NgModule({
    imports: [
        CommonModule,
        routing,
        TableModule,
        GrowlModule
    ],
    declarations: [TodoListComponent],
    providers: [TodoListService]
})
export class TodoListModule { }
