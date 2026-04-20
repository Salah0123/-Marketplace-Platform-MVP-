import { mergeApplicationConfig, ApplicationConfig } from '@angular/core';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { appConfig } from './app.config';
import { serverRoutes } from './app.routes.server';

import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { APP_BASE_HREF } from '@angular/common';
import { authInterceptor } from './core/interceptors/auth-interceptor';

const serverConfig: ApplicationConfig = {
  providers: [
    provideServerRendering(withRoutes(serverRoutes)),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor])),
    { provide: APP_BASE_HREF, useValue: 'http://mvp-api:8080' },
  ]
};


export const config = mergeApplicationConfig(appConfig, serverConfig);