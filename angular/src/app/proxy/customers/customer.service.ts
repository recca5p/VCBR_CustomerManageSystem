import type { CustomerCreateDTO, CustomerDTO, CustomerFilterListDTO, CustomerUpdateDTO } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  apiName = 'Default';
  

  create = (input: CustomerCreateDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDTO>({
      method: 'POST',
      url: '/api/app/customer',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  deleteCustomer = (identityNumber: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/customer/customer',
      params: { identityNumber },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDTO>({
      method: 'GET',
      url: `/api/app/customer/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: CustomerFilterListDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDTO>>({
      method: 'GET',
      url: '/api/app/customer',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (identityNumber: string, input: CustomerUpdateDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/app/customer',
      params: { identityNumber },
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
