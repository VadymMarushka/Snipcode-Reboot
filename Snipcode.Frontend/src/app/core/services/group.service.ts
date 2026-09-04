import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { GroupResponseDto, GroupDetailResponseDto } from '../models/group.model';
import { PagedResultDto } from '../models/paged-result.model';
import { GroupQueryDto } from '../models/query.model';

@Injectable({ providedIn: 'root' })
export class GroupService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/groups`;

  publicGroupsState = signal<PagedResultDto<GroupResponseDto> | null>(null);
  myGroupsState = signal<PagedResultDto<GroupResponseDto> | null>(null);
  isLoading = signal<boolean>(false);

  loadPublicGroups(query: GroupQueryDto = {}): void {
    this.isLoading.set(true);
    const params = this.buildParams(query);

    this.http.get<PagedResultDto<GroupResponseDto>>(`${this.apiUrl}/public`, { params }).subscribe({
      next: (data) => {
        this.publicGroupsState.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load public groups:', err);
        this.isLoading.set(false);
      }
    });
  }

  loadMyGroups(query: GroupQueryDto = {}): void {
    this.isLoading.set(true);
    const params = this.buildParams(query);

    this.http.get<PagedResultDto<GroupResponseDto>>(`${this.apiUrl}/my`, { params }).subscribe({
      next: (data) => {
        this.myGroupsState.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load my groups:', err);
        this.isLoading.set(false);
      }
    });
  }

  getGroupDetails(id: string): Observable<GroupDetailResponseDto> {
    return this.http.get<GroupDetailResponseDto>(`${this.apiUrl}/${id}`);
  }

  private buildParams(query: GroupQueryDto): HttpParams {
    let params = new HttpParams();
    if (query.searchTerm) params = params.set('searchTerm', query.searchTerm);
    if (query.category) params = params.set('category', query.category);
    if (query.pageNumber) params = params.set('pageNumber', query.pageNumber);
    if (query.pageSize) params = params.set('pageSize', query.pageSize);

    if (query.technologies && query.technologies.length > 0) {
      query.technologies.forEach((tech) => {
        params = params.append('technologies', tech);
      });
    }

    return params;
  }
}