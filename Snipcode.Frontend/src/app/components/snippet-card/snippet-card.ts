import { Component, input, output, inject, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SafeHtml } from '@angular/platform-browser';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { lucideCopy, lucideArrowRight } from '@ng-icons/lucide';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmBadgeImports } from '@spartan-ng/helm/badge';
import { HlmCardImports } from '@spartan-ng/helm/card';
import { HlmScrollAreaImports } from '@spartan-ng/helm/scroll-area';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { HighlightService } from '../../core/services/highlight.service';
import { ThemeService } from '../../core/services/theme.service';
import { Snippet } from '../../core/models/snippet.model';

@Component({
  selector: 'app-snippet-card',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    NgIconComponent,
    HlmButtonImports,
    HlmBadgeImports,
    HlmCardImports,
    HlmScrollAreaImports,
    NgScrollbarModule
  ],
  providers: [provideIcons({ lucideCopy, lucideArrowRight })],
  templateUrl: './snippet-card.html',
  styleUrl: './snippet-card.css'
})
export class SnippetCard {
  private highlightService = inject(HighlightService);
  private themeService = inject(ThemeService);

  snippet = input.required<Snippet>();
  copy = output<string>();

  highlightedCode = signal<SafeHtml>('');

  constructor() {
    effect(async () => {
      const item = this.snippet();
      const currentTheme = this.themeService.theme();

      if (item) {
        const html = await this.highlightService.highlight(
          item.codeContent,
          item.technology.toLowerCase(),
          currentTheme
        );
        this.highlightedCode.set(html);
      }
    });
  }

  onCopy(event: Event) {
    event.stopPropagation();
    this.copy.emit(this.snippet().id);
  }
}