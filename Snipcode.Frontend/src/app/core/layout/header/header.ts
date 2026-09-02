import { Component, inject, signal } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { 
  lucideSun, 
  lucideMoon, 
  lucideChevronDown, 
  lucideMenu, 
  lucideCode, 
  lucideFolder, 
  lucideLogIn, 
  lucideUserPlus 
} from '@ng-icons/lucide';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmDropdownMenuImports } from '@spartan-ng/helm/dropdown-menu';
import { HlmSheetImports } from '@spartan-ng/helm/sheet';
import { ThemeService } from '../../services/theme.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    NgIcon,
    HlmButtonImports,
    HlmDropdownMenuImports,
    HlmSheetImports,
    RouterLink
  ],
  providers: [
    provideIcons({ 
      lucideSun, 
      lucideMoon, 
      lucideChevronDown, 
      lucideMenu, 
      lucideCode, 
      lucideFolder, 
      lucideLogIn, 
      lucideUserPlus 
    })
  ],
  templateUrl: './header.html',
})
export class Header {
  readonly themeService = inject(ThemeService);
  isAuthenticated = signal<boolean>(false);

  toggleAuthDemo(): void {
    this.isAuthenticated.update((auth) => !auth);
  }

  // Перенаправлення до ThemeService
  toggleTheme(): void {
    this.themeService.toggleTheme();
  }

  isDarkMode(): boolean {
    return this.themeService.isDark();
  }
}