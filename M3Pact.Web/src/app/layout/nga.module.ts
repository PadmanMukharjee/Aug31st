import { NgModule, ModuleWithProviders }      from '@angular/core';
import { CommonModule }  from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DropdownModule } from 'primeng/dropdown';
import { ThemeConfigService } from './theme.config';

import {
  ThemeConfigProvider
} from './theme.configProvider';

import {
  PageTopComponent,
  LoaderComponent
} from './components';


import {
  ScrollPositionDirective,
  SlimScrollDirective,
  ThemeRunDirective
} from './directives';

import {
  AppPicturePipe,
  ProfilePicturePipe,
} from './pipes';

import {
  EmailValidator,
  EqualPasswordsValidator
} from '../shared/validators';

const NGA_COMPONENTS = [
  PageTopComponent,
  LoaderComponent
];

const NGA_DIRECTIVES = [
  ScrollPositionDirective,
  SlimScrollDirective,
  ThemeRunDirective
];

const NGA_PIPES = [
  AppPicturePipe,
  ProfilePicturePipe
];

const NGA_SERVICES = [
];

const NGA_VALIDATORS = [
  EmailValidator,
  EqualPasswordsValidator
];

@NgModule({
  declarations: [
    ...NGA_PIPES,
    ...NGA_DIRECTIVES,
    ...NGA_COMPONENTS
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    DropdownModule
  ],
  exports: [
    ...NGA_PIPES,
    ...NGA_DIRECTIVES,
    ...NGA_COMPONENTS
  ]
})
export class NgaModule {
  static forRoot(): ModuleWithProviders {
    return <ModuleWithProviders> {
      ngModule: NgaModule,
      providers: [
        ThemeConfigProvider,
        ThemeConfigService,
        ...NGA_VALIDATORS,
        ...NGA_SERVICES
      ],
    };
  }
}
