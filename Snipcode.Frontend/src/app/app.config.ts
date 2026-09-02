import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideIcons } from '@ng-icons/core';
import { lucideMenu, lucideSun, lucideMoon, lucideCode, lucideFolder, lucideLogIn, lucideUserPlus } from '@ng-icons/lucide';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideIcons({ lucideMenu, lucideSun, lucideMoon, lucideCode, lucideFolder, lucideLogIn, lucideUserPlus })
  ]
};
