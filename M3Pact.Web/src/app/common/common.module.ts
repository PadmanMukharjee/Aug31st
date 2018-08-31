import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ValidationMessageComponent } from './components/validation-message/validation-message.component';
import { CommonService } from './common.service';

import { AutofocusDirective } from './directives/autofocus.directive';
import { ReadOnlyDirective } from './directives/readonly.directive';


@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule
    ],
    declarations: [
        ValidationMessageComponent,
        AutofocusDirective,
        ReadOnlyDirective
    ],
    exports: [
        ValidationMessageComponent,
        AutofocusDirective,
        ReadOnlyDirective
    ],
    providers: [ // expose our Services and Providers into Angular's dependency injection
        CommonService
    ]

})
export class SharedModule {
}