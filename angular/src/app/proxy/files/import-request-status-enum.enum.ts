import { mapEnumToOptions } from '@abp/ng.core';

export enum ImportRequestStatusEnum {
  Created = 0,
  Success = 1,
  Failed = 2,
  Executing = 3,
}

export const importRequestStatusEnumOptions = mapEnumToOptions(ImportRequestStatusEnum);
