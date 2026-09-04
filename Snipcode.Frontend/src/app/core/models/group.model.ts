import { Category } from '../enums/category.enum';
import { Technology } from '../enums/technology.enum';
import { SnippetResponseDto } from './snippet.model';

export interface GroupSummaryDto {
  id: string;
  name: string;
  category: Category;
}

export interface GroupResponseDto {
  id: string;
  name: string;
  description: string | null;
  category: Category;
  isPublic: boolean;
  createdAt: string;
  ownerId: string;
  ownerUsername: string;
  snippetCount: number;
  technologies: Technology[];
}

export interface GroupDetailResponseDto {
  id: string;
  name: string;
  description: string | null;
  category: Category;
  isPublic: boolean;
  createdAt: string;
  ownerId: string;
  ownerUsername: string;
  snippets: SnippetResponseDto[];
  technologies: Technology[];
}