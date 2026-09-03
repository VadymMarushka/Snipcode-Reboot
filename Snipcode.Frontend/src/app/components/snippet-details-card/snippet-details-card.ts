import { Component, input, signal, effect, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SafeHtml } from '@angular/platform-browser';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { lucideCopy, lucideCheck } from '@ng-icons/lucide';
import { HlmBadgeImports } from '@spartan-ng/helm/badge';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { Snippet } from '../../core/models/snippet.model';
import { HighlightService } from '../../core/services/highlight.service';

@Component({
  selector: 'app-snippet-details-card',
  standalone: true,
  imports: [
    CommonModule,
    NgIconComponent,
    HlmBadgeImports,
    HlmButtonImports,
    NgScrollbarModule,
  ],
  providers: [
    provideIcons({ lucideCopy, lucideCheck })
  ],
  templateUrl: './snippet-details-card.html',
  styleUrl: './snippet-details-card.css'
})
export class SnippetDetailsCard {
  snippet = input.required<Snippet>();

  private highlightService = inject(HighlightService);

  highlightedCode = signal<SafeHtml>('');
  isCopied = signal<boolean>(false);

  constructor() {
    effect(async () => {
      const currentSnippet = this.snippet();
      if (!currentSnippet?.codeContent) return;

      // Визначаємо тему за класом на <html>
      const isDark = document.documentElement.classList.contains('dark');
      const themeMode = isDark ? 'dark' : 'light';

      const safeHtml = await this.highlightService.highlight(
        currentSnippet.codeContent,
        currentSnippet.technology.toLowerCase(),
        themeMode
      );

      this.highlightedCode.set(safeHtml);
    });
  }

  onCopy(): void {
    navigator.clipboard.writeText(this.snippet().codeContent);
    this.isCopied.set(true);
    setTimeout(() => this.isCopied.set(false), 2000);
  }
}