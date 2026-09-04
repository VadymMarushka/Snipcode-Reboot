import { Component, signal, computed, inject, input, effect } from '@angular/core';
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
import { BrnSheetImports } from '@spartan-ng/brain/sheet';
import { HlmSheetImports } from '@spartan-ng/helm/sheet';
import { HlmSelectImports } from '@spartan-ng/helm/select';

import { SnippetCard } from '../../components/snippet-card/snippet-card';
import { GroupService } from '../../core/services/group.service';
import { GroupDetailResponseDto } from '../../core/models/group.model';
import { Technology } from '../../core/enums/technology.enum';
import { SnippetResponseDto } from '../../core/models/snippet.model';

export interface TechFilterOption {
  id: Technology;
  name: string;
  count: number;
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
  private groupService = inject(GroupService);

  // Router param input: /groups/:id
  id = input<string>();

  // Стан групи та завантаження
  groupDetail = signal<GroupDetailResponseDto | null>(null);
  isLoading = signal<boolean>(false);

  // Фільтри
  searchQuery = signal<string>('');
  selectedTechs = signal<Technology[]>([]);
  selectedSortValue = signal<string>('latest');

  public readonly sortOptions = [
    { label: 'Latest', value: 'latest' },
    { label: 'Oldest', value: 'oldest' },
  ];

  public readonly sortItemToString = (value: string) => 
    this.sortOptions.find((opt) => opt.value === value)?.label || '';

  private readonly techLabels: Record<Technology, string> = {
    [Technology.CSharp]: 'C#',
    [Technology.Python]: 'Python',
    [Technology.TypeScript]: 'TypeScript',
    [Technology.JavaScript]: 'JavaScript',
    [Technology.SQL]: 'SQL',
    [Technology.Go]: 'Go',
    [Technology.HTML]: 'HTML',
    [Technology.CSS]: 'CSS',
  };

  constructor() {
    // Автоматичне завантаження деталей групи при зміні id
    effect(() => {
      const groupId = this.id();
      if (groupId) {
        this.fetchGroupDetails(groupId);
      }
    });
  }

  private fetchGroupDetails(groupId: string): void {
    this.isLoading.set(true);
    this.groupService.getGroupDetails(groupId).subscribe({
      next: (data) => {
        this.groupDetail.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load group details:', err);
        this.isLoading.set(false);
      }
    });
  }

  // Обчислення списку доступних технологій групи
  technologiesList = computed<TechFilterOption[]>(() => {
    const group = this.groupDetail();
    if (!group || !group.snippets) return [];

    const counts = new Map<Technology, number>();
    group.snippets.forEach(s => {
      counts.set(s.technology, (counts.get(s.technology) || 0) + 1);
    });

    return Array.from(counts.entries()).map(([tech, count]) => ({
      id: tech,
      name: this.techLabels[tech] || tech,
      count
    }));
  });

  // Локальна фільтрація та сортування сніпетів групи
  filteredSnippets = computed<SnippetResponseDto[]>(() => {
    const group = this.groupDetail();
    if (!group || !group.snippets) return [];

    let result = [...group.snippets];

    // Пошук
    const search = this.searchQuery().trim().toLowerCase();
    if (search) {
      result = result.filter(s => 
        s.title.toLowerCase().includes(search) || 
        s.codeContent.toLowerCase().includes(search) ||
        (s.description && s.description.toLowerCase().includes(search))
      );
    }

    // Чекбокси технологій
    const techs = this.selectedTechs();
    if (techs.length > 0) {
      result = result.filter(s => techs.includes(s.technology));
    }

    // Сортування
    const sort = this.selectedSortValue();
    result.sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime();
      const dateB = new Date(b.createdAt).getTime();
      return sort === 'oldest' ? dateA - dateB : dateB - dateA;
    });

    return result;
  });

  onSortChange(value: string | null | undefined): void {
    if (value) {
      this.selectedSortValue.set(value);
    }
  }

  toggleTech(tech: Technology): void {
    const current = this.selectedTechs();
    if (current.includes(tech)) {
      this.selectedTechs.set(current.filter((t) => t !== tech));
    } else {
      this.selectedTechs.set([...current, tech]);
    }
  }

  isTechSelected(tech: Technology): boolean {
    return this.selectedTechs().includes(tech);
  }

  handleCopy(id: string): void {
    console.log('Copied snippet ID:', id);
  }

  clearFilters(): void {
    this.searchQuery.set('');
    this.selectedTechs.set([]);
    this.selectedSortValue.set('latest');
  }
}