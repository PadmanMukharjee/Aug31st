import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TodoListComponent } from './todo-list.component';

// Routes
export const routes: Routes = [
    {
        path: '',
        component: TodoListComponent
    }
];
export const routing = RouterModule.forChild(routes);
