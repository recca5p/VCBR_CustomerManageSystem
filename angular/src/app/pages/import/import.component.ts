import { saveAs } from 'file-saver';
import { FileService } from './../../proxy/controllers/file.service';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ImportRequestService } from '@proxy/import-requests';
import { ImportCRUDDTO } from '@proxy/import-requests/dtos';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.scss'],
  providers: [ListService],
})
export class ImportComponent implements OnInit {
  importRequests = { items: [], totalCount: 0 } as PagedResultDto<ImportCRUDDTO>;

  isModalOpen = false;
  isModalOpenEdit = false;

  file: File;

  form: FormGroup;

  selectedImport = {} as ImportCRUDDTO;

  constructor(
    public readonly list: ListService<ImportCRUDDTO>,
    private importService: ImportRequestService,
    private fb: FormBuilder,
    private fileService: FileService,
    private _snackBar: MatSnackBar,
    private changeDetectorRef: ChangeDetectorRef
  ) {}
  private hubConnection: HubConnection;

  ngOnInit(): void {
    const customerStreamCreator = query => this.importService.getList(query);
    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.importRequests = response;
    });

    let token: string = `${localStorage.getItem('access_token')}`;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apis.default.url}/signalr-hub/import`, {
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

    this.hubConnection.on('added', (message: any) => {
      // Find the index of the item with the matching id in the importRequests array
      this.importRequests.items.unshift(message);
      // Decrement the totalCount since an item is deleted
      this.changeDetectorRef.detectChanges();
      this.changeDetectorRef.markForCheck(); // Add this line for OnPush strategy
    });

    this.hubConnection.on('update', (message: any) => {
      // Find the index of the item with the matching id in the importRequests array
      const indexToUpdate = this.importRequests.items.findIndex(item => item.id === message.id);

      // If the item exists in the array, update it with the updated object
      if (indexToUpdate !== -1) {
        this.importRequests.items[indexToUpdate] = message;
      }
      // Decrement the totalCount since an item is deleted
      this.changeDetectorRef.detectChanges();
      this.changeDetectorRef.markForCheck(); // Add this line for OnPush strategy
    });
  }

  download(reportId: any) {
    this.fileService.getFileFromApi(reportId).subscribe(response => {
      const downloadLink = document.createElement('a');
      downloadLink.href = URL.createObjectURL(
        new Blob([response.body], { type: response.body.type })
      );

      downloadLink.download = reportId;
      downloadLink.click();
    });
  }

  uploadFile(data: any) {
    this.file = data[0];
  }

  onSave() {
    this.isModalOpen = false;

    this.importService.createImportRequest(this.file).subscribe(
      response => {},
      err => {
        console.log(err.error);
        this._snackBar.open(err.error);
      }
    );
  }

  clearFile() {
    this.file = null;
  }

  createImport() {
    this.isModalOpen = true;
    this.buildCreateForm();
  }

  buildCreateForm() {
    this.form = this.fb.group({
      import: [],
    });
  }
}
