import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IActionResult } from '../microsoft/asp-net-core/mvc/models';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  apiName = 'Default';
  baseUrl: string = `${environment.apis.default.url}`;
  // api-service.ts

  getFileFromApi(reportId: string): Observable<HttpResponse<Blob>> {
    return this.httpClient.get<Blob>(`${this.baseUrl}/api/File/key?key=${reportId}`, {
      observe: 'response',
      responseType: 'blob' as 'json',
    });
  }

  constructor(private restService: RestService, private httpClient: HttpClient) {}
}
