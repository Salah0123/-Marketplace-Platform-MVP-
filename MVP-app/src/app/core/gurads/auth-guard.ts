import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LoginService } from '../services/login/login-service';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(LoginService);

  if (!authService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }
  if (authService.isTokenExpired()) {
    authService.logout();
    router.navigate(['/login']);
    return false;
  }

  const requiredRoles = route.data?.['roles'] as string[];
  const userRoles = authService.getUserRoles();


  const hasAccess =
  !requiredRoles ||
  requiredRoles.some(role => userRoles.includes(role));

if (!hasAccess) {
  router.navigate(['/unauthorized']);
  return false;
}
  return true;
};
