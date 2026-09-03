import { Component, signal, computed, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { 
  lucideSearch, 
  lucideSlidersHorizontal, 
  lucideX, 
  lucideChevronDown,
  lucideBraces,
  lucideArrowLeft,
  lucideFiles,
  lucideFolderOpen
} from '@ng-icons/lucide';

import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmInputImports } from '@spartan-ng/helm/input';
import { HlmCheckboxImports } from '@spartan-ng/helm/checkbox';
import { HlmBadgeImports } from '@spartan-ng/helm/badge';

// Sheet Imports
import { BrnSheetImports } from '@spartan-ng/brain/sheet';
import { HlmSheetImports } from '@spartan-ng/helm/sheet';

// Select Imports
import { HlmSelectImports } from '@spartan-ng/helm/select';

import { SnippetCard } from '../../components/snippet-card/snippet-card';
import { Snippet } from '../../core/models/snippet.model';
import { Technology } from '../../core/models/technology.enum';

export interface GroupDetails {
  id: string;
  name: string;
  authorUsername: string;
  snippetCount: number;
}

@Component({
  selector: 'app-group-snippets-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    NgIconComponent,
    HlmButtonImports,
    HlmInputImports,
    HlmCheckboxImports,
    HlmBadgeImports,
    BrnSheetImports,
    HlmSheetImports,
    HlmSelectImports,
    SnippetCard
  ],
  providers: [
    provideIcons({ 
      lucideSearch, 
      lucideSlidersHorizontal, 
      lucideX, 
      lucideChevronDown,
      lucideBraces,
      lucideArrowLeft,
      lucideFiles,
      lucideFolderOpen
    })
  ],
  templateUrl: './group-snippets-page.html',
})
export class GroupSnippetsPage {
  // Отримуємо id групи з параметрів урла (Angular 16+)
  id = input<string>();

  // Мокові дані групи
  group = signal<GroupDetails>({
    id: '1',
    name: 'LLM Integration Patterns',
    authorUsername: 'vmarushka',
    snippetCount: 7
  });

  searchQuery = signal('');
  selectedCategory = signal('all');
  selectedTechs = signal<string[]>([]);
  
  public readonly sortOptions = [
    { label: 'Latest', value: 'latest' },
    { label: 'Most Starred', value: 'popular' },
    { label: 'Oldest', value: 'oldest' },
  ];
  selectedSortValue = signal<string>('latest');

  public readonly sortItemToString = (value: string) => 
    this.sortOptions.find((opt) => opt.value === value)?.label || '';

  categories = [
    { id: 'all', name: 'All', count: 7 },
    { id: 'backend', name: 'Backend', count: 4 },
    { id: 'ai', name: 'AI', count: 3 },
  ];

  technologies = [
    { id: 'typescript', name: 'TypeScript', count: 3 },
    { id: 'javascript', name: 'JavaScript', count: 2 },
    { id: 'python', name: 'Python', count: 2 },
  ];

  snippets = signal<Snippet[]>([
    {
      id: '101',
      title: 'OpenAI Streaming Response',
      description: 'Stream chat completions to the client using Server-Sent Events',
      codeContent: "import OpenAI from 'openai'\n\nconst openai = new OpenAI()\n\nexport async function streamChat(prompt: string, res: Response) {\n  const stream = await openai.chat.completions.create({\n    model: 'gpt-4o',\n  });\n}",
      technology: Technology.TypeScript,
      isPublic: true,
      createdAt: '2026-03-02T10:00:00Z',
      updatedAt: '2026-03-02T10:00:00Z',
      authorId: 'user-1',
      authorUsername: 'vmarushka',
      groupId: '1',
      tags: ['OpenAI', 'Streaming', 'SSE']
    },
    {
      id: '102',
      title: 'JWT Auth Middleware',
      description: 'Express middleware to validate JWT tokens from Authorization header',
      codeContent: "const jwt = require('jsonwebtoken');\n\nexport const authMiddleware = (req, res, next) => {\n  const token = req.headers.authorization?.split(' ')[1];\n  if (!token) return res.status(401).json({ error: 'Unauthorized' });\n  try {\n    req.user = jwt.verify(token, process.env.JWT_SECRET);\n    next();\n  } catch (err) {\n    res.status(403).json({ error: 'Invalid token' });\n  }\n};",
      technology: Technology.JavaScript,
      isPublic: true,
      createdAt: '2026-03-01T15:20:00Z',
      updatedAt: '2026-03-01T15:20:00Z',
      authorId: 'user-1',
      authorUsername: 'vmarushka',
      groupId: '1',
      tags: ['Auth', 'JWT', 'Express']
    }
  ]);

  totalSnippets = computed(() => this.snippets().length);

  onSortChange(value: string | null | undefined): void {
    if (value) {
      this.selectedSortValue.set(value);
    }
  }

  selectCategory(id: string) {
    this.selectedCategory.set(id);
  }

  toggleTech(id: string) {
    const current = this.selectedTechs();
    if (current.includes(id)) {
      this.selectedTechs.set(current.filter((t) => t !== id));
    } else {
      this.selectedTechs.set([...current, id]);
    }
  }

  isTechSelected(id: string): boolean {
    return this.selectedTechs().includes(id);
  }

  handleCopy(id: string) {
    console.log('Copied snippet ID:', id);
  }

  clearFilters(): void {
    this.searchQuery.set('');
    this.selectedCategory.set('all');
    this.selectedTechs.set([]);
    this.selectedSortValue.set('latest');
  }
}