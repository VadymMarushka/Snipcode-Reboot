import { Injectable, inject } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { createHighlighter, Highlighter } from 'shiki';

@Injectable({ providedIn: 'root' })
export class HighlightService {
  private sanitizer = inject(DomSanitizer);
  private highlighterPromise: Promise<Highlighter>;

  constructor() {
    this.highlighterPromise = createHighlighter({
      themes: ['github-dark-default', 'github-light-default'],
      langs: ['typescript', 'javascript', 'html', 'css', 'json', 'python']
    });
  }

  async highlight(code: string, lang: string, themeMode: 'dark' | 'light' = 'dark'): Promise<SafeHtml> {
    const highlighter = await this.highlighterPromise;
    
    const loadedLangs = highlighter.getLoadedLanguages();
    if (!loadedLangs.includes(lang)) {
      try {
        await highlighter.loadLanguage(lang as any);
      } catch {
        lang = 'text';
      }
    }

    const theme = themeMode === 'dark' ? 'github-dark-default' : 'github-light-default';

    const html = highlighter.codeToHtml(code, {
      lang,
      theme
    });

    return this.sanitizer.bypassSecurityTrustHtml(html);
  }
}