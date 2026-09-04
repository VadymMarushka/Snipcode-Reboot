import { Category } from '../enums/category.enum';
import { Technology } from '../enums/technology.enum';

export interface SnippetQueryDto {
  searchTerm?: string;
  category?: string;
  technologies?: string[];
  tag?: string;
  sortBy?: string;
  pageNumber?: number;
  pageSize?: number;
}

export interface SnippetStatsDto {
  categoryCounts: Record<string, number>;
  technologyCounts: Record<string, number>;
  totalCount: number;
}

export interface GroupQueryDto {
  searchTerm?: string;
  category?: Category;
  technologies?: Technology[];
  sortBy?: string;
  pageNumber?: number;
  pageSize?: number;
}