import type { Entity } from '../models';

export interface AuditedEntity<TKey> extends CreationAuditedEntity<TKey> {
  lastModificationTime?: string;
  lastModifierId?: string;
}

export interface CreationAuditedEntity<TKey> {
  creationTime?: string;
  creatorId?: string;
}
