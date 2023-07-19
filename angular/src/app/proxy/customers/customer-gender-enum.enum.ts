import { mapEnumToOptions } from '@abp/ng.core';

export enum CustomerGenderEnum {
  Male = 0,
  Female = 1,
}

export const customerGenderEnumOptions = mapEnumToOptions(CustomerGenderEnum);
