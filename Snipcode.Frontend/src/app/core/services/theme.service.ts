import { Injectable, inject, signal, effect, PLATFORM_ID } from '@angular/core';
import { DOCUMENT, isPlatformBrowser } from '@angular/common';

export type ThemeMode = 'dark' | 'light';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly document = inject(DOCUMENT);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly STORAGE_KEY = 'snipcode-theme';

  readonly theme = signal<ThemeMode>('dark');

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      const savedTheme = localStorage.getItem(this.STORAGE_KEY) as ThemeMode | null;
      
      if (savedTheme === 'light' || savedTheme === 'dark') {
        this.theme.set(savedTheme);
      } else {
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        this.theme.set(prefersDark ? 'dark' : 'light');
      }
    }

    effect(() => {
      const currentTheme = this.theme();
      
      if (isPlatformBrowser(this.platformId)) {
        localStorage.setItem(this.STORAGE_KEY, currentTheme);
        const root = this.document.documentElement;
        
        if (currentTheme === 'dark') {
          root.classList.add('dark');
        } else {
          root.classList.remove('dark');
        }
      }
    });
  }

  toggleTheme(): void {
    this.theme.update(current => (current === 'dark' ? 'light' : 'dark'));
  }

  isDark(): boolean {
    return this.theme() === 'dark';
  }
}