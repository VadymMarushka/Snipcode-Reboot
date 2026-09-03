import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { 
  lucideSearch, 
  lucideSlidersHorizontal, 
  lucideX, 
  lucideChevronDown,
  lucideFolderSearch // Використовуємо іншу іконку для порожнього стану груп
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

// Імпортуємо компонент та інтерфейс картки групи
import { GroupCard } from '../../components/group-card/group-card';
import { SnippetGroup } from '../../core/models/group.model';

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
    GroupCard // Компонент групи замість сніпета
  ],
  providers: [
    provideIcons({ 
      lucideSearch, 
      lucideSlidersHorizontal, 
      lucideX, 
      lucideChevronDown,
      lucideFolderSearch
    })
  ],
  templateUrl: './public-groups-page.html',
})
export class PublicGroupsPage {
  searchQuery = signal('');
  selectedCategory = signal('all');
  selectedTechs = signal<string[]>([]);
  
  public readonly sortOptions = [
    { label: 'Latest Updated', value: 'latest' },
    { label: 'Most Snippets', value: 'popular' },
    { label: 'Recently Created', value: 'newest' },
  ];
  selectedSortValue = signal<string>('latest');

  public readonly sortItemToString = (value: string) => 
    this.sortOptions.find((opt) => opt.value === value)?.label || '';

  categories = [
    { id: 'all', name: 'All Groups', count: 142 },
    { id: 'backend', name: 'Backend', count: 56 },
    { id: 'frontend', name: 'Frontend', count: 48 },
    { id: 'devops', name: 'DevOps', count: 21 },
    { id: 'data-science', name: 'Data Science', count: 17 }
  ];

  technologies = [
    { id: 'angular', name: 'Angular', count: 28 },
    { id: 'react', name: 'React', count: 32 },
    { id: 'dotnet', name: '.NET', count: 41 },
    { id: 'python', name: 'Python', count: 19 },
    { id: 'docker', name: 'Docker', count: 15 },
    { id: 'kubernetes', name: 'Kubernetes', count: 9 },
  ];

  totalGroups = computed(() => this.groups().length);

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

  clearFilters(): void {
    this.searchQuery.set('');
    this.selectedCategory.set('all');
    this.selectedTechs.set([]);
    this.selectedSortValue.set('latest');
  }

  // Мокові дані для груп
  groups = signal<SnippetGroup[]>([
    {
      id: 'g-1',
      name: 'Enterprise Angular Patterns',
      description: 'Advanced state management, signal store architectures, and DI tricks for large-scale Angular applications.',
      author: 'vmarushka',
      snippetCount: 24,
      tags: ['Angular', 'Signals', 'RxJS', 'TypeScript', 'Architecture'],
      updatedAt: new Date('2026-03-01T14:20:00Z')
    },
    {
      id: 'g-2',
      name: '.NET Microservices Boilerplate',
      description: 'Essential snippets for setting up DDD, MediatR, and CQRS in .NET 8 microservices.',
      author: 'dotnet_ninja',
      snippetCount: 42,
      tags: ['.NET', 'C#', 'Microservices', 'MediatR', 'CQRS', 'Docker'],
      updatedAt: new Date('2026-02-28T09:15:00Z')
    },
    {
      id: 'g-3',
      name: 'Tailwind UI Components',
      description: 'A collection of beautiful, fully responsive Tailwind CSS components ready to be copy-pasted.',
      author: 'css_wizard',
      snippetCount: 128,
      tags: ['Tailwind', 'CSS', 'UI', 'Frontend'],
      updatedAt: new Date('2026-02-25T11:45:00Z')
    },
    {
      id: 'g-4',
      name: 'Python Data Engineering Pipeline',
      description: 'Scripts for ETL processes, Pandas data cleaning, and Apache Airflow DAG configurations.',
      author: 'data_knight',
      snippetCount: 15,
      tags: ['Python', 'Pandas', 'Airflow', 'Data Engineering'],
      updatedAt: new Date('2026-02-20T16:30:00Z')
    },
    {
      id: 'g-5',
      name: 'Kubernetes Manifests Collection',
      description: 'Production-ready K8s deployments, services, ingress controllers, and config maps.',
      author: 'devops_guru',
      snippetCount: 31,
      tags: ['Kubernetes', 'DevOps', 'YAML', 'Helm'],
      updatedAt: new Date('2026-02-18T10:00:00Z')
    },
    {
      id: 'g-6',
      name: 'React Performance Hooks',
      description: 'Custom React hooks focused on memoization, debouncing, and avoiding unnecessary re-renders.',
      author: 'react_dev',
      snippetCount: 19,
      tags: ['React', 'Hooks', 'Performance', 'JavaScript'],
      updatedAt: new Date('2026-02-15T08:20:00Z')
    }
  ]);
}