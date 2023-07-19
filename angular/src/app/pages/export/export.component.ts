import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ExportRequests } from '@proxy';
import { ExportRequestService, FileService } from '@proxy/controllers';
import { ExportRequestCreateDTO } from '@proxy/export-requests/dtos';

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
    private fileService: FileService
  ) {}

  ngOnInit(): void {
    const customerStreamCreator = query => this.exportService.getList(query);
    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.exportRequests = response;
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
    this.exportService.createExportRequestByModel(params).subscribe(res => {
      window.location.reload();
    });
  }
}
