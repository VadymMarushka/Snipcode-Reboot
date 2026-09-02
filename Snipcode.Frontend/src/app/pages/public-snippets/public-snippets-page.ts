import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { 
  lucideSearch, 
  lucideSlidersHorizontal, 
  lucideX, 
  lucideChevronDown,
  lucideBraces
} from '@ng-icons/lucide';

import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmInputImports } from '@spartan-ng/helm/input';
import { HlmCheckboxImports } from '@spartan-ng/helm/checkbox';
import { HlmBadgeImports } from '@spartan-ng/helm/badge';

// Sheet Imports (Brain + Helm)
import { BrnSheetImports } from '@spartan-ng/brain/sheet';
import { HlmSheetImports } from '@spartan-ng/helm/sheet';

// Select Imports
import { HlmSelectImports } from '@spartan-ng/helm/select';

import { SnippetCard } from '../../components/snippet-card/snippet-card';
import { Snippet } from '../../core/models/snippet.model';
import { Technology } from '../../core/models/technology.enum';

@Component({
  selector: 'app-public-snippets-page',
  standalone: true,
  imports: [
    CommonModule,
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
      lucideBraces
    })
  ],
  templateUrl: './public-snippets-page.html',
})
export class PublicSnippetsPage {
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
    { id: 'all', name: 'All', count: 912 },
    { id: 'backend', name: 'Backend', count: 284 },
    { id: 'frontend', name: 'Frontend', count: 196 },
    { id: 'devops', name: 'DevOps', count: 143 },
    { id: 'ai', name: 'AI', count: 97 },
    { id: 'data-science', name: 'Data Science', count: 118 },
    { id: 'mobile', name: 'Mobile', count: 74 },
  ];

  technologies = [
    { id: 'csharp', name: 'C#', count: 47 },
    { id: 'python', name: 'Python', count: 38 },
    { id: 'typescript', name: 'TypeScript', count: 35 },
    { id: 'javascript', name: 'JavaScript', count: 29 },
    { id: 'sql', name: 'SQL', count: 24 },
    { id: 'go', name: 'Go', count: 18 },
    { id: 'html-css', name: 'HTML/CSS', count: 14 },
  ];

  //snippets = signal<Snippet[]>([]);
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

snippets = signal<Snippet[]>([
    {
      id: '1',
      title: 'EF Core Bulk Insert Optimization',
      description: 'High-performance bulk insertion pattern using EFCore.BulkExtensions for large SQL datasets.',
      codeContent: 'await context.BulkInsertAsync(entities, config => {\n  config.BatchSize = 5000;\n});',
      technology: Technology.CSharp, // Або значення за вашим Enum (наприклад Technology.CSharp / 'CSharp')
      isPublic: true,
      createdAt: '2026-03-01T10:00:00Z',
      updatedAt: '2026-03-01T10:00:00Z',
      authorId: 'user-1',
      authorUsername: 'vmarushka',
      groupId: null,
      tags: ['EF Core', 'Performance', 'SQL']
    },
    {
      id: '2',
      title: 'FastAPI JWT Auth Middleware',
      description: 'Lightweight dependency injection for verifying JWT tokens in protected FastAPI routes.',
      codeContent: '@app.get("/me")\ndef get_me(user: User = Depends(get_current_user)):\n    return user',
      technology: Technology.Python,
      isPublic: true,
      createdAt: '2026-03-01T11:30:00Z',
      updatedAt: '2026-03-01T11:30:00Z',
      authorId: 'user-2',
      authorUsername: 'alex_dev',
      groupId: null,
      tags: ['FastAPI', 'Auth', 'JWT']
    },
    {
      id: '3',
      title: 'Angular Signal Store Pattern',
      description: 'Clean state management using Angular Signals with computed reactivity and async updates.',
      codeContent: 'export const SnippetStore = signalStore(\n  withState({ items: [] }),\n  withMethods(...)\n);',
      technology: Technology.TypeScript,
      isPublic: true,
      createdAt: '2026-02-28T09:15:00Z',
      updatedAt: '2026-02-28T09:15:00Z',
      authorId: 'user-3',
      authorUsername: 'sarah_ng',
      groupId: null,
      tags: ['Angular', 'Signals', 'State']
    },
    {
      id: '4',
      title: 'Docker Multi-Stage Go Build',
      description: 'Minimalistic Alpine scratch image setup for production Go binaries under 15MB size.',
      codeContent: 'FROM golang:1.22-alpine AS builder\nRUN go build -o app .\nFROM scratch\nCOPY --from=builder /app /app',
      technology: Technology.Python,
      isPublic: true,
      createdAt: '2026-02-27T14:20:00Z',
      updatedAt: '2026-02-27T14:20:00Z',
      authorId: 'user-4',
      authorUsername: 'devops_guru',
      groupId: null,
      tags: ['Docker', 'Go', 'DevOps']
    },
    {
      id: '5',
      title: 'SQL Recursive CTE Tree Query',
      description: 'Retrieve nested organizational hierarchy or comments thread in a single SQL execution.',
      codeContent: 'WITH RECURSIVE CategoryTree AS (\n  SELECT id, parent_id, name FROM categories\n  UNION ALL ...\n)',
      technology: Technology.CSharp,
      isPublic: true,
      createdAt: '2026-02-26T16:45:00Z',
      updatedAt: '2026-02-26T16:45:00Z',
      authorId: 'user-5',
      authorUsername: 'data_knight',
      groupId: null,
      tags: ['SQL', 'CTE', 'Database']
    },
    {
      id: '6',
      title: 'PyTorch AMP Training Loop',
      description: 'Boilerplate for PyTorch model training with CUDA automatic mixed precision scaling.',
      codeContent: 'scaler = torch.cuda.amp.GradScaler()\nfor x, y in dataloader:\n    with torch.cuda.amp.autocast():\n        loss = model(x)',
      technology: Technology.Python,
      isPublic: true,
      createdAt: '2026-02-25T12:00:00Z',
      updatedAt: '2026-02-25T12:00:00Z',
      authorId: 'user-6',
      authorUsername: 'ml_expert',
      groupId: null,
      tags: ['PyTorch', 'AI', 'Machine Learning']
    },
    {
      id: '7',
      title: 'Tailwind Glassmorphism Card',
      description: 'Modern backdrop-blur card utilities with subtle border gradients for sleek UI design.',
      codeContent: '<div class="bg-card/60 backdrop-blur-md border border-border/70 rounded-xl p-4">',
      technology: Technology.HTML,
      isPublic: true,
      createdAt: '2026-02-24T18:30:00Z',
      updatedAt: '2026-02-24T18:30:00Z',
      authorId: 'user-7',
      authorUsername: 'css_wizard',
      groupId: null,
      tags: ['Tailwind', 'CSS', 'UI']
    },
    {
      id: '8',
      title: 'Pandas Vectorized Cleaning',
      description: 'Fast vectorized operation to clean missing values and normalize text columns in DataFrames.',
      codeContent: 'df[\'clean_title\'] = df[\'title\'].str.strip().str.lower().fillna(\'unknown\')',
      technology: Technology.Python,
      isPublic: true,
      createdAt: '2026-02-20T08:10:00Z',
      updatedAt: '2026-02-20T08:10:00Z',
      authorId: 'user-8',
      authorUsername: 'pandas_pro',
      groupId: null,
      tags: ['Pandas', 'Data Science', 'Python']
    },
    {
      id: '9',
      title: 'Custom React Query Wrapper',
      description: 'Re-usable hook around useQuery with automatic toast notification on request failure.',
      codeContent: 'export const useSnippet = (id: string) => {\n  return useQuery([\'snippet\', id], () => fetchSnippet(id));\n};',
      technology: Technology.JavaScript,
      isPublic: true,
      createdAt: '2026-02-18T15:00:00Z',
      updatedAt: '2026-02-18T15:00:00Z',
      authorId: 'user-9',
      authorUsername: 'react_dev',
      groupId: null,
      tags: ['React', 'React Query', 'Frontend']
    },
    {
      id: '10',
      title: 'Responsive Breakpoint Helper',
      description: 'Extension methods on BuildContext to easily handle responsive mobile and desktop layouts.',
      codeContent: 'extension Responsive on BuildContext {\n  bool get isDesktop => MediaQuery.of(this).size.width >= 1024;\n}',
      technology: Technology.TypeScript,
      isPublic: true,
      createdAt: '2026-02-15T11:20:00Z',
      updatedAt: '2026-02-15T11:20:00Z',
      authorId: 'user-10',
      authorUsername: 'mobile_dev',
      groupId: null,
      tags: ['Mobile', 'Responsive', 'UI']
    }
  ]);
}