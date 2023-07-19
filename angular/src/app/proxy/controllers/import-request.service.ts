import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ImportRequestCreateDTO } from '../import-requests/dtos/models';
import type { IActionResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class ImportRequestService {
  apiName = 'Default';
}
