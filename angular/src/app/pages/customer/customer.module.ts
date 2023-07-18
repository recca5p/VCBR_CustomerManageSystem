import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerComponent } from './customer.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NzTableModule } from 'ng-zorro-antd/table';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';

@NgModule({
  declarations: [CustomerComponent],
  imports: [
    SharedModule,
    CommonModule,
    CustomerRoutingModule,
    NgbDatepickerModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatFormFieldModule,
  ],
})
export class CustomerModule {}
