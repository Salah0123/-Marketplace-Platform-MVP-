
import { Routes } from '@angular/router';
import { authGuard } from './core/gurads/auth-guard';

export const routes: Routes = [
  // =========================
  // Public Routes (No Layout)
  // =========================

  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login/login/login').then((m) => m.Login),
  },

  // =========================
  // App Layout (Sidebar Shell)
  // =========================

  {
    path: '',
    loadComponent: () =>
      import('./layouts/app-layout/app-layout').then(
        (m) => m.AppLayout
      ),
    children: [
      // Redirect default to home
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'login',
      },

      // =========================
      // Admin Routes
      // =========================
      {
        path: 'admin',
        canActivate: [authGuard],
        data: { roles: ['Admin'] },
        children: [
          {
            path: 'requests',
            loadComponent: () =>
              import('./pages/admin/request-management/request-management')
                .then((m) => m.RequestManagement),
          },
          {
            path: 'roles',
            loadComponent: () =>
              import('./pages/admin/role-management/role-management')
                .then((m) => m.RoleManagement),
          },
          {
            path: 'users',
            loadComponent: () =>
              import('./pages/admin/user-management/user-management')
                .then((m) => m.UserManagement),
          },
          {
            path: 'create-user',
            loadComponent: () =>
              import('./pages/admin/create-user/create-user')
                .then((m) => m.CreateUser),
          },
        ],
      },

      // =========================
      // Customer Routes
      // =========================
      {
        path: 'customer',
        canActivate: [authGuard],
        data: { roles: ['Customer'] },
        children: [
          {
            path: 'new-request',
            loadComponent: () =>
              import('./pages/customer/create-request/create-request')
                .then((m) => m.CreateRequest),
          },
          {
            path: 'my-requests',
            loadComponent: () =>
              import('./pages/customer/my-requests/my-requests')
                .then((m) => m.MyRequests),
          },
          {
            path: 'subscription',
            loadComponent: () =>
              import('./pages/customer/subscription/subscription')
                .then((m) => m.Subscription),
          },
        ],
      },

      // =========================
      // Provider Routes
      // =========================
      {
        path: 'provider',
        canActivate: [authGuard],
        data: { roles: ['Provider'] },
        children: [
          {
            path: 'nearby',
            loadComponent: () =>
              import('./pages/provider/nearby-requests/nearby-requests')
                .then((m) => m.NearbyRequests),
          },
          {
            path: 'my-jobs',
            loadComponent: () =>
              import('./pages/provider/my-jobs/my-jobs')
                .then((m) => m.MyJobs),
          },
        ],
      },
    ],
  },

  // =========================
  // Wildcard
  // =========================
  {
    path: '**',
    redirectTo: 'login',
  },
];