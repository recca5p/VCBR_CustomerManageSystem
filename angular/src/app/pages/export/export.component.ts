import { ListService, PagedResultDto } from '@abp/ng.core';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ExportRequests } from '@proxy';
import { ExportRequestService, FileService } from '@proxy/controllers';
import { ExportRequestCreateDTO } from '@proxy/export-requests/dtos';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-export',
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.scss'],
  providers: [ListService],
})
export class ExportComponent implements OnInit {
  exportRequests = { items: [], totalCount: 0 } as PagedResultDto<any>;

  isModalOpen = false;
  isModalOpenEdit = false;

  file: File;

  form: FormGroup;

  selectedImport = {};

  constructor(
    public readonly list: ListService<any>,
    private exportService: ExportRequestService,
    private fb: FormBuilder,
    private fileService: FileService,
    private changeDetectorRef: ChangeDetectorRef
  ) {}

  private hubConnection: HubConnection;

  ngOnInit(): void {
    const customerStreamCreator = query => this.exportService.getList(query);
    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.exportRequests = response;
    });

    let token: string = `${localStorage.getItem('access_token')}`;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apis.default.url}/signalr-hub/export`, {
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
      console.log(message);
      // Find the index of the item with the matching id in the importRequests array
      this.exportRequests.items.unshift(message);
      // Decrement the totalCount since an item is deleted
      this.changeDetectorRef.detectChanges();
      this.changeDetectorRef.markForCheck(); // Add this line for OnPush strategy
    });

    this.hubConnection.on('update', (message: any) => {
      // Find the index of the item with the matching id in the importRequests array
      const indexToUpdate = this.exportRequests.items.findIndex(item => item.id === message.id);

      // If the item exists in the array, update it with the updated object
      if (indexToUpdate !== -1) {
        this.exportRequests.items[indexToUpdate] = message;
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

  createExport() {
    let params: ExportRequestCreateDTO = { filter: '' };
    this.exportService.createExportRequestByModel(params).subscribe(
      res => {},
      error => {
        console.error(error);
      }
    );
  }
}
