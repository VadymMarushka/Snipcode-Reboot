import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { SnippetResponseDto } from '../models/snippet.model';
import { PagedResultDto } from '../models/paged-result.model';
import { SnippetQueryDto, SnippetStatsDto } from '../models/query.model';

@Injectable({ providedIn: 'root' })
export class SnippetService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/snippets`;

  publicSnippetsState = signal<PagedResultDto<SnippetResponseDto> | null>(null);
  mySnippetsState = signal<PagedResultDto<SnippetResponseDto> | null>(null);
  isLoading = signal<boolean>(false);
  publicStatsState = signal<SnippetStatsDto | null>(null);

  loadPublicStats(): void {
  this.http.get<SnippetStatsDto>(`${this.apiUrl}/public/stats`).subscribe({
    next: (data) => this.publicStatsState.set(data),
    error: (err) => console.error('Failed to load stats:', err)
  });
}

  loadPublicSnippets(query: SnippetQueryDto = {}): void {
    this.isLoading.set(true);
    const params = this.buildParams(query);

    this.http.get<PagedResultDto<SnippetResponseDto>>(`${this.apiUrl}/public`, { params }).subscribe({
      next: (data) => {
        this.publicSnippetsState.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load public snippets:', err);
        this.isLoading.set(false);
      }
    });
  }

  loadMySnippets(query: SnippetQueryDto = {}): void {
    this.isLoading.set(true);
    const params = this.buildParams(query);

    this.http.get<PagedResultDto<SnippetResponseDto>>(`${this.apiUrl}/my`, { params }).subscribe({
      next: (data) => {
        this.mySnippetsState.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load my snippets:', err);
        this.isLoading.set(false);
      }
    });
  }

  getById(id: string): Observable<SnippetResponseDto> {
    return this.http.get<SnippetResponseDto>(`${this.apiUrl}/${id}`);
  }

private buildParams(query: SnippetQueryDto): HttpParams {
  let params = new HttpParams();

  if (query.searchTerm && query.searchTerm.trim() !== '') {
    params = params.set('searchTerm', query.searchTerm.trim());
  }

  if (query.category) {
    params = params.set('category', query.category);
  }

  if (query.technologies && query.technologies.length > 0) {
    query.technologies.forEach(tech => {
      params = params.append('technologies', tech);
    });
  }

  if (query.tag) params = params.set('tag', query.tag);
  if (query.sortBy) params = params.set('sortBy', query.sortBy);
  if (query.pageNumber) params = params.set('pageNumber', query.pageNumber);
  if (query.pageSize) params = params.set('pageSize', query.pageSize);

  return params;
}
}