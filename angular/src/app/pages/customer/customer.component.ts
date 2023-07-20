import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { CustomerDTO, CustomerFilterListDTO } from '@proxy/customers/dtos';
import { CustomerService } from '@proxy/customers';
import { ConfigStateService } from '@abp/ng.core';
import { webSocket } from 'rxjs/webSocket';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-author',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class CustomerComponent implements OnInit {
  customer = { items: [], totalCount: 0 } as PagedResultDto<CustomerDTO>;

  isModalOpen = false;
  isModalOpenEdit = false;

  form: FormGroup;

  selectedCustomer = {} as CustomerDTO;

  genderList = [
    { name: 'Male', value: 'Male' },
    { name: 'Female', value: 'Female' },
  ];

  searchParams = {} as CustomerFilterListDTO;

  constructor(
    public readonly list: ListService<CustomerFilterListDTO>,
    private customerService: CustomerService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private configState: ConfigStateService,
    private changeDetectorRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const customerStreamCreator = query =>
      this.customerService.getList({ ...query, ...this.searchParams });
    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.customer = response;
    });

    let token: string = `${localStorage.getItem('access_token')}`;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apis.default.url}/signalr-hub/customer`, {
        accessTokenFactory: () => token,
        // Add any other headers you want to send with the connection
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }) // Replace with the correct URL
      .build();

    // Start the SignalR connection
    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR hub connection started successfully.');
      })
      .catch(error => {
        console.error('Error starting SignalR hub connection:', error);
      });

    // Subscribe to the received messages
    this.hubConnection.on('added', (message: any) => {
      this.customer.items.unshift(message);
      this.customer.totalCount++;
      this.changeDetectorRef.detectChanges();
      this.changeDetectorRef.markForCheck(); // Add this line for OnPush strategy
    });

    // Subscribe to the received messages
    this.hubConnection.on('delete', (message: any) => {
      // Filter out the item with the matching UserId
      this.customer.items = this.customer.items.filter(item => item.userId != message);

      // Decrement the totalCount since an item is deleted
      this.customer.totalCount--;
      this.changeDetectorRef.detectChanges();
      this.changeDetectorRef.markForCheck(); // Add this line for OnPush strategy
    });

    this.hubConnection.on('update', (message: any) => {
      // Assuming 'message' contains the updated object with the same UserId
      const updatedItem = message;
      // Find the index of the item with the matching UserId in the items array
      const indexToUpdate = this.customer.items.findIndex(
        item => item.userId === updatedItem.userId
      );

      // If the item exists in the array, update it with the updated object
      if (indexToUpdate !== -1) {
        this.customer.items[indexToUpdate] = updatedItem;
      }
      // Decrement the totalCount since an item is deleted
      this.changeDetectorRef.detectChanges();
      this.changeDetectorRef.markForCheck(); // Add this line for OnPush strategy
    });
  }
  private hubConnection: HubConnection;

  // In your component's initialization or relevant method:

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
      firstName: [this.selectedCustomer.firstName],
      lastName: [this.selectedCustomer.lastName],
      gender: [this.selectedCustomer.gender || Validators.required],
      address: [this.selectedCustomer.address],
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
