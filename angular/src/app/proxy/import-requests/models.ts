import type { ImportRequestStatusEnum } from '../files/import-request-status-enum.enum';

export interface ImportRequest extends AuditedEntity<string> {
  filename?: string;
  extension?: string;
  fileId?: string;
  requestStatus: ImportRequestStatusEnum;
  result?: string;
  reportId?: string;
}
