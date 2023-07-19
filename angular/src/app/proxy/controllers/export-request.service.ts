import { RestService, Rest, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExportRequestCreateDTO } from '../export-requests/dtos/models';
import type { IActionResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class ExportRequestService {
  apiName = 'Default';

  createExportRequestByModel = (model: ExportRequestCreateDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IActionResult>(
      {
        method: 'POST',
        url: '/api/ExportRequest',
        body: model,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<any>>(
      {
        method: 'GET',
        url: '/api/app/export-request',
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );
}
