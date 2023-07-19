import { HttpClient, HttpHeaders } from '@angular/common/http';
import type { ImportCRUDDTO, ImportRequestCreateDTO } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ImportRequestService {
  apiName = 'Default';
  baseUrl: string = `${environment.apis.default.url}`;

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ImportCRUDDTO>>(
      {
        method: 'GET',
        url: '/api/app/import-request',
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );

  createImportRequest(file: File) {
    const params: FormData = new FormData();

    params.append('File', file, file.name);

    let token: string = localStorage.getItem('access_token');

    const option = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`,
      }),
    };

    return this.httpClient.post(`${this.baseUrl}/api/ImportRequest`, params);
  }
  constructor(private restService: RestService, private httpClient: HttpClient) {}
}
