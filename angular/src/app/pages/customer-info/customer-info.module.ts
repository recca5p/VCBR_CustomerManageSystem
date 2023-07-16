import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerInfoRoutingModule } from './customer-info-routing.module';
import { CustomerInfoComponent } from './customer-info.component';


@NgModule({
  declarations: [
    CustomerInfoComponent
  ],
  imports: [
    CommonModule,
    CustomerInfoRoutingModule
  ]
})
export class CustomerInfoModule { }
