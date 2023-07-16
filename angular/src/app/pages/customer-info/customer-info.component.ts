import { NgbDateAdapter, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit } from '@angular/core';
import { ConfigStateService, ListService } from '@abp/ng.core';
import { CustomerService } from '@proxy/customers';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { CustomerDTO } from '@proxy/customers/dtos';

@Component({
  selector: 'app-customer-info',
  templateUrl: './customer-info.component.html',
  styleUrls: ['./customer-info.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class CustomerInfoComponent implements OnInit {
  constructor(
    public readonly list: ListService,
    private customerService: CustomerService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private configState: ConfigStateService
  ) {}
  isModalOpen = false;
  customer: CustomerDTO = {
    lastName: '',
    firstName: '',
    userId: '',
  };
  form: FormGroup;

  selectedCustomer = {} as CustomerDTO;

  genderList = [
    { name: 'Male', value: 'Male' },
    { name: 'Female', value: 'Female' },
  ];
  x;
  async ngOnInit() {
    await this.getData();
  }
  getData() {
    this.customerService.get(this.currentUserId).subscribe(response => {
      this.customer = response;
      console.log(this.customer);
    });
  }

  get currentUserId(): string {
    return this.configState.getDeep('currentUser.id');
  }

  editCustomer() {
    this.customerService.get(this.currentUserId).subscribe(customer => {
      this.selectedCustomer = customer;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      identityNumber: [this.selectedCustomer.identityNumber || '', Validators.required],
      firstName: [this.selectedCustomer.firstName],
      lastName: [this.selectedCustomer.lastName],
      gender: [this.selectedCustomer.gender || Validators.required],
      address: [this.selectedCustomer.address],
      email: [
        this.selectedCustomer.email || '',
        [Validators.required, Validators.pattern(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/)],
      ],
      phoneNumber: [this.selectedCustomer.phoneNumber],
      balance: [this.customer.balance],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    if (this.selectedCustomer.id) {
      this.customerService
        .update(this.selectedCustomer.identityNumber, this.form.value)
        .subscribe(() => {
          this.isModalOpen = false;
          this.form.reset();
          this.list.get();
        });
    }
  }
}
