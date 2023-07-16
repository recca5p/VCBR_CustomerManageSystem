import type { CustomerGenderEnum } from '../customer-gender-enum.enum';
import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CustomerCreateDTO {
  firstName?: string;
  lastName?: string;
  gender: CustomerGenderEnum;
  address?: string;
  email?: string;
  identityNumber: string;
  phoneNumber?: string;
  balance?: number;
  password: string;
}

export interface CustomerDTO extends EntityDto<string> {
  firstName?: string;
  lastName?: string;
  gender?: string;
  address?: string;
  email?: string;
  identityNumber?: string;
  phoneNumber?: string;
  balance?: number;
  createdTime?: string;
  isActive?: boolean;
}

export interface CustomerFilterListDTO extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface CustomerUpdateDTO {
  firstName?: string;
  lastName?: string;
  gender?: CustomerGenderEnum;
  address?: string;
  email?: string;
  phoneNumber?: string;
  balance?: number;
  isActive?: boolean;
}
