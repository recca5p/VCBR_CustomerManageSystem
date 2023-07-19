import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ImportRoutingModule } from './import-routing.module';
import { ImportComponent } from './import.component';
import { NgxDatatableDefaultDirective } from '@abp/ng.theme.shared';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
  declarations: [ImportComponent],
  imports: [CommonModule, ImportRoutingModule, CommonModule, SharedModule, MatSnackBarModule],
})
export class ImportModule {}
