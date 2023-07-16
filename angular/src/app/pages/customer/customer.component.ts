import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { CustomerDTO } from '@proxy/customers/dtos';
import { CustomerService } from '@proxy/customers';
import { ConfigStateService } from '@abp/ng.core';

@Component({
  selector: 'app-author',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class CustomerComponent implements OnInit {
  customer = { items: [], totalCount: 0 } as PagedResultDto<CustomerDTO>;

  isModalOpen = false;

  form: FormGroup;

  selectedCustomer = {} as CustomerDTO;

  genderList = [
    { name: 'Male', value: 'Male' },
    { name: 'Female', value: 'Female' },
  ];

  constructor(
    public readonly list: ListService,
    private customerService: CustomerService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private configState: ConfigStateService
  ) {}

  ngOnInit(): void {
    const customerStreamCreator = query => this.customerService.getList(query);

    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.customer = response;
    });
  }

  get currentUserId(): string {
    return this.configState.getDeep('currentUser.id');
  }

  createCustomer() {
    this.selectedCustomer = {} as CustomerDTO;
    this.buildCreateForm();
    this.isModalOpen = true;
  }

  editCustomer(id: string) {
    this.customerService.get(id).subscribe(customer => {
      this.selectedCustomer = customer;
      this.buildEditForm();
      this.isModalOpen = true;
    });
  }

  buildCreateForm() {
    this.form = this.fb.group({
      identityNumber: [this.selectedCustomer.identityNumber || '', Validators.required],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/),
        ],
      ],
      firstName: [this.selectedCustomer.firstName],
      lastName: [this.selectedCustomer.lastName],
      gender: [this.selectedCustomer.gender || Validators.required],
      address: [this.selectedCustomer.address],
      email: [
        this.selectedCustomer.email || '',
        [Validators.required, Validators.pattern(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/)],
      ],
      phoneNumber: [this.selectedCustomer.phoneNumber],
      balance: [
        this.selectedCustomer.balance || '',
        [Validators.required, this.balanceRangeValidator.bind(this)],
      ],
    });
  }

  buildEditForm() {
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
      balance: [
        this.selectedCustomer.balance || '',
        [Validators.required, this.balanceRangeValidator.bind(this)],
      ],
    });
  }

  balanceRangeValidator(control: AbstractControl): { [key: string]: any } | null {
    const balance = parseFloat(control.value);

    if (isNaN(balance) || balance < 0 || balance > 1000000000000) {
      return { balanceRange: true };
    }

    return null;
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
    } else {
      this.customerService.create(this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.customerService.deleteCustomer(id).subscribe(() => this.list.get());
      }
    });
  }
}
