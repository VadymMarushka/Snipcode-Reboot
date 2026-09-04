import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { provideIcons } from '@ng-icons/core';
import { lucideMenu, lucideSun, lucideMoon, lucideCode, lucideFolder, lucideLogIn, lucideUserPlus } from '@ng-icons/lucide';
import { provideHttpClient, withFetch } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withFetch()),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    provideIcons({ lucideMenu, lucideSun, lucideMoon, lucideCode, lucideFolder, lucideLogIn, lucideUserPlus })
  ]
};
