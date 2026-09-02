import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/hero/hero-page').then((m) => m.HeroPage),
  },
  {
    path: 'snippets',
    loadComponent: () =>
      import('./pages/public-snippets/public-snippets-page').then(
        (m) => m.PublicSnippetsPage
      ),
  },
  {
    path: 'groups',
    loadComponent: () =>
      import('./pages/public-groups/public-groups-page').then(
        (m) => m.PublicGroupsPage
      ),
  },
  {
    path: '**',
    redirectTo: '',
  },
];