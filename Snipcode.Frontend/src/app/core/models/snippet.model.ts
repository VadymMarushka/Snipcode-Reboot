// src/app/core/models/snippet.model.ts
import { Technology } from '../enums/technology.enum';
import { GroupSummaryDto } from './group.model';

export interface SnippetResponseDto {
  id: string;
  title: string;
  description: string | null;
  codeContent: string;
  technology: Technology;
  isPublic: boolean;
  createdAt: string;
  updatedAt: string;
  authorId: string;
  authorUsername: string;
  tags: string[];
  group: GroupSummaryDto | null;
}