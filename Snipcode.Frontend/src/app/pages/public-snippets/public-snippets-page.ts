import { Component, signal, effect, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { 
  lucideSearch, 
  lucideSlidersHorizontal, 
  lucideX, 
  lucideChevronDown,
  lucideChevronLeft,
  lucideChevronRight,
  lucideBraces
} from '@ng-icons/lucide';

import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmInputImports } from '@spartan-ng/helm/input';
import { HlmCheckboxImports } from '@spartan-ng/helm/checkbox';
import { HlmBadgeImports } from '@spartan-ng/helm/badge';

import { BrnSheetImports } from '@spartan-ng/brain/sheet';
import { HlmSheetImports } from '@spartan-ng/helm/sheet';
import { HlmSelectImports } from '@spartan-ng/helm/select';

import { SnippetCard } from '../../components/snippet-card/snippet-card';
import { SnippetService } from '../../core/services/snippet.service';
import { Category } from '../../core/enums/category.enum';
import { Technology } from '../../core/enums/technology.enum';

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
      lucideChevronLeft,
      lucideChevronRight,
      lucideBraces
    })
  ],
  templateUrl: './public-snippets-page.html',
})
export class PublicSnippetsPage implements OnInit {
  public snippetService = inject(SnippetService);

  searchQuery = signal('');
  selectedCategory = signal<string | null>(null);
  selectedTechs = signal<string[]>([]);
  selectedSortValue = signal<string>('latest');
  pageNumber = signal<number>(1);
  pageSize = signal<number>(12);

  public readonly sortOptions = [
    { label: 'Latest', value: 'latest' },
    { label: 'Oldest', value: 'oldest' },
  ];

  public readonly categoriesList = [
    { id: null, name: 'All' },
    { id: Category.Backend, name: 'Backend' },
    { id: Category.Frontend, name: 'Frontend' },
    { id: Category.DevOps, name: 'DevOps' },
    { id: Category.AI, name: 'AI' },
    { id: Category.DataScience, name: 'Data Science' },
    { id: Category.Mobile, name: 'Mobile' },
  ];

  public readonly technologiesList = [
    { id: Technology.CSharp, name: 'C#' },
    { id: Technology.Python, name: 'Python' },
    { id: Technology.TypeScript, name: 'TypeScript' },
    { id: Technology.JavaScript, name: 'JavaScript' },
    { id: Technology.SQL, name: 'SQL' },
    { id: Technology.Go, name: 'Go' },
    { id: Technology.HTML, name: 'HTML' },
    { id: Technology.CSS, name: 'CSS' },
  ];

  constructor() {
    effect(() => {
      const query = {
        searchTerm: this.searchQuery(),
        category: this.selectedCategory() || undefined,
        technologies: this.selectedTechs(),
        sortBy: this.selectedSortValue(),
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize()
      };

      this.snippetService.loadPublicSnippets(query);
    });
  }

  ngOnInit(): void {
    this.snippetService.loadPublicStats();
  }

  public readonly sortItemToString = (value: string) => 
    this.sortOptions.find((opt) => opt.value === value)?.label || '';

  getCategoryCount(catId: string | null): number {
    const stats = this.snippetService.publicStatsState();
    if (!stats) return 0;
    if (!catId) return stats.totalCount;
    return stats.categoryCounts[catId] || 0;
  }

  getTechCount(techId: string): number {
    const stats = this.snippetService.publicStatsState();
    return stats?.technologyCounts[techId] || 0;
  }

  onSortChange(value: string | null | undefined): void {
    if (value) {
      this.selectedSortValue.set(value);
      this.pageNumber.set(1);
    }
  }

  selectCategory(id: string | null) {
    this.selectedCategory.set(id);
    this.pageNumber.set(1);
  }

  toggleTech(id: string) {
    const current = this.selectedTechs();
    if (current.includes(id)) {
      this.selectedTechs.set(current.filter((t) => t !== id));
    } else {
      this.selectedTechs.set([...current, id]);
    }
    this.pageNumber.set(1);
  }

  isTechSelected(id: string): boolean {
    return this.selectedTechs().includes(id);
  }

  handleCopy(id: string) {
    console.log('Copied snippet ID:', id);
  }

  clearFilters(): void {
    this.searchQuery.set('');
    this.selectedCategory.set(null);
    this.selectedTechs.set([]);
    this.selectedSortValue.set('latest');
    this.pageNumber.set(1);
  }

  changePage(newPage: number): void {
    const maxPage = this.snippetService.publicSnippetsState()?.totalPages || 1;
    if (newPage >= 1 && newPage <= maxPage) {
      this.pageNumber.set(newPage);
    }
  }
}