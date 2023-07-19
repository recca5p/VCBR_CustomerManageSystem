import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExportRoutingModule } from './export-routing.module';
import { ExportComponent } from './export.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [ExportComponent],
  imports: [CommonModule, ExportRoutingModule, SharedModule],
})
export class ExportModule {}
