import { Component, signal, computed, inject, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { 
  lucideSearch, 
  lucideSlidersHorizontal, 
  lucideX, 
  lucideChevronDown,
  lucideFolderSearch,
  lucideLoader2
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

import { GroupCard } from '../../components/group-card/group-card';
import { GroupService } from '../../core/services/group.service';
import { Category } from '../../core/enums/category.enum';
import { Technology } from '../../core/enums/technology.enum';
import { GroupQueryDto } from '../../core/models/query.model';

@Component({
  selector: 'app-public-groups-page',
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
    GroupCard
  ],
  providers: [
    provideIcons({ 
      lucideSearch, 
      lucideSlidersHorizontal, 
      lucideX, 
      lucideChevronDown,
      lucideFolderSearch,
      lucideLoader2
    })
  ],
  templateUrl: './public-groups-page.html',
})
export class PublicGroupsPage {
  private groupService = inject(GroupService);

  public readonly isLoading = this.groupService.isLoading;
  
  // Отримуємо списки з сервісу
  public readonly publicGroupsState = this.groupService.publicGroupsState;
  groups = computed(() => this.publicGroupsState()?.items ?? []);
  totalGroups = computed(() => this.publicGroupsState()?.totalCount ?? 0);

  // Стейт фільтрів
  searchQuery = signal('');
  selectedCategory = signal<Category | 'all'>('all');
  selectedTechs = signal<Technology[]>([]);

public readonly categories: { id: Category | 'all'; name: string }[] = [
  { id: 'all', name: 'All Groups' },
  { id: Category.Backend, name: 'Backend' },
  { id: Category.Frontend, name: 'Frontend' },
  { id: Category.DevOps, name: 'DevOps' },
  { id: Category.AI, name: 'AI' },
  { id: Category.DataScience, name: 'Data Science' },
  { id: Category.Mobile, name: 'Mobile' },
];

public readonly technologies: { id: Technology; name: string }[] = [
  { id: Technology.TypeScript, name: 'TypeScript' },
  { id: Technology.JavaScript, name: 'JavaScript' },
  { id: Technology.CSharp, name: 'C#' },
  { id: Technology.HTML, name: 'HTML' },
  { id: Technology.CSS, name: 'CSS' },
  { id: Technology.SQL, name: 'SQL' },
  { id: Technology.Go, name: 'Go' },
  { id: Technology.Python, name: 'Python' },
];

  public readonly sortOptions = [
    { label: 'Latest', value: 'latest' },
    { label: 'Oldest', value: 'oldest' },
  ];
  selectedSortValue = signal<string>('latest');

  public readonly sortItemToString = (value: string) => 
    this.sortOptions.find((opt) => opt.value === value)?.label || '';

  constructor() {
    effect(() => {
      const query: GroupQueryDto = {
        searchTerm: this.searchQuery() || undefined,
        category: this.selectedCategory() === 'all' ? undefined : (this.selectedCategory() as Category),
        technologies: this.selectedTechs().length > 0 ? this.selectedTechs() : undefined,
        sortBy: this.selectedSortValue(),
        pageNumber: 1,
        pageSize: 12
      };
      console.log(this.selectedSortValue());
      this.groupService.loadPublicGroups(query);
    });
  }

  onSortChange(value: string | null | undefined): void {
    if (value) {
      this.selectedSortValue.set(value);
    }
  }

  selectCategory(catId: Category | 'all') {
    this.selectedCategory.set(catId);
  }

  toggleTech(techId: Technology) {
    const current = this.selectedTechs();
    if (current.includes(techId)) {
      this.selectedTechs.set(current.filter((t) => t !== techId));
    } else {
      this.selectedTechs.set([...current, techId]);
    }
  }

  isTechSelected(techId: Technology): boolean {
    return this.selectedTechs().includes(techId);
  }

  clearFilters(): void {
    this.searchQuery.set('');
    this.selectedCategory.set('all');
    this.selectedTechs.set([]);
    this.selectedSortValue.set('newest');
  }
}