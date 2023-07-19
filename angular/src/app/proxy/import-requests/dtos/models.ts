import type { AuditedEntityDto } from '@abp/ng.core';
import type { ImportRequestStatusEnum } from '../../files/import-request-status-enum.enum';

export interface ImportRequestCreateDTO {}

export interface ImportCRUDDTO extends AuditedEntityDto<string> {
  filename?: string;
  extension?: string;
  fileId?: string;
  requestStatus: ImportRequestStatusEnum;
  result?: string;
  reportId?: string;
}
