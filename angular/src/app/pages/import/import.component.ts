import { saveAs } from 'file-saver';
import { FileService } from './../../proxy/controllers/file.service';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ImportRequestService } from '@proxy/import-requests';
import { ImportCRUDDTO } from '@proxy/import-requests/dtos';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const customerStreamCreator = query => this.importService.getList(query);
    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.importRequests = response;
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
      response => {
        window.location.reload();
      },
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
