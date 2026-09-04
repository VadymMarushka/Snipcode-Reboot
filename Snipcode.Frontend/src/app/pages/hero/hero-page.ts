import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import {
  lucideRefreshCw,
  lucideFolder,
  lucideLock,
  lucideHexagon,
  lucideCode2,
  lucideTerminal,
  lucideSparkles,
  lucideColumns,
  lucideSmartphone,
} from '@ng-icons/lucide';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmCardImports } from '@spartan-ng/helm/card';
import { HlmBadgeImports } from '@spartan-ng/helm/badge';
import { SnippetCard } from '../../components/snippet-card/snippet-card'; 
import { SnippetResponseDto } from '../../core/models/snippet.model';
import { Technology } from '../../core/enums/technology.enum';
import { Category } from '../../core/enums/category.enum';

@Component({
  selector: 'app-hero-page',
  standalone: true,
  imports: [
    RouterLink,
    NgIconComponent,
    HlmButtonImports,
    HlmCardImports,
    HlmBadgeImports,
    SnippetCard,
  ],
  providers: [
    provideIcons({
      lucideRefreshCw,
      lucideFolder,
      lucideLock,
      lucideHexagon,
      lucideCode2,
      lucideTerminal,
      lucideSparkles,
      lucideColumns,
      lucideSmartphone,
    }),
  ],
  templateUrl: './hero-page.html',
  styleUrl: './hero-page.css',
})
export class HeroPage {
  // Мок-сніпет для демонстрації в Hero
  demoSnippet: SnippetResponseDto = {
    id: 'demo-1',
    title: 'useDebounce.ts',
    description: 'Custom React hook for debouncing fast-changing inputs and search requests.',
    codeContent: `import { useDebounce } from '@/hooks/useDebounce';
import { searchSnippets } from '@/api/snippets';

export function SnippetSearch() {
  const [query, setQuery] = useState('');
  const debouncedQuery = useDebounce(query, 300);

  const { data, isLoading } = useQuery({
    queryKey: ['snippets', debouncedQuery],
    queryFn: () => searchSnippets(debouncedQuery),
    enabled: debouncedQuery.length > 1,
  });

  return (
    <SearchInput
      value={query}
      onChange={setQuery}
      placeholder="Search snippets... ⌘K"
      results={data?.snippets ?? []}
    />
  );
}`,
    technology: Technology.TypeScript,
    authorUsername: 'vmarushka',
    tags: ['react', 'hooks', 'debounce', 'query'],
    isPublic: false,
    createdAt: '',
    updatedAt: '',
    authorId: '',
    group: {
      id: '1',
      name: 'raaah',
      category: Category.Backend
    }
  };

  // Категорії для нижнього блоку
  categories = [
    { name: 'Backend', count: 284, icon: 'lucideHexagon', color: 'text-blue-500' },
    { name: 'Frontend', count: 196, icon: 'lucideCode2', color: 'text-sky-400' },
    { name: 'DevOps', count: 143, icon: 'lucideTerminal', color: 'text-emerald-500' },
    { name: 'AI', count: 97, icon: 'lucideSparkles', color: 'text-purple-400' },
    { name: 'Data Science', count: 118, icon: 'lucideColumns', color: 'text-amber-500' },
    { name: 'Mobile', count: 74, icon: 'lucideSmartphone', color: 'text-rose-500' },
  ];
}