// src/app/core/services/auth.service.ts
import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import {
  AuthResponseDto,
  LoginDto,
  RegisterDto,
  RefreshTokenRequestDto,
  UserProfileDto
} from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = `${environment.apiUrl}/auth`;

  // Сигнали стану
  // Зберігаємо основну інфу про юзера (Email, Username)
  currentUser = signal<{username: string, email: string} | null>(this.getStoredUser());
  accessToken = signal<string | null>(localStorage.getItem('at'));
  
  isAuthenticated = computed(() => !!this.accessToken());

  login(dto: LoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/login`, dto).pipe(
      tap(res => this.handleAuthSuccess(res))
    );
  }

  register(dto: RegisterDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/register`, dto).pipe(
      tap(res => this.handleAuthSuccess(res))
    );
  }

  // Метод для отримання профілю (/me)
  getProfile(): Observable<UserProfileDto> {
    return this.http.get<UserProfileDto>(`${this.apiUrl}/me`);
  }

  refreshToken(): Observable<AuthResponseDto> {
    const at = this.accessToken();
    const rt = localStorage.getItem('rt');

    if (!at || !rt) {
      this.logout();
      return throwError(() => new Error('No tokens found'));
    }

    const dto: RefreshTokenRequestDto = { accessToken: at, refreshToken: rt };

    return this.http.post<AuthResponseDto>(`${this.apiUrl}/refresh-token`, dto).pipe(
      tap(res => this.handleAuthSuccess(res)),
      catchError(err => {
        this.logout();
        return throwError(() => err);
      })
    );
  }

  logout(): void {
    localStorage.clear();
    this.accessToken.set(null);
    this.currentUser.set(null);
    this.router.navigate(['/']);
  }

  private handleAuthSuccess(res: AuthResponseDto): void {
    localStorage.setItem('at', res.accessToken);
    localStorage.setItem('rt', res.refreshToken);
    
    const userSummary = { username: res.username, email: res.email };
    localStorage.setItem('u', JSON.stringify(userSummary));

    this.accessToken.set(res.accessToken);
    this.currentUser.set(userSummary);
  }

  private getStoredUser() {
    const u = localStorage.getItem('u');
    return u ? JSON.parse(u) : null;
  }
}