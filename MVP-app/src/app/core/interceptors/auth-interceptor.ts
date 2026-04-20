import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { LoginService } from '../services/login/login-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // return next(req);
  const router = inject(Router);
  const loginService = inject(LoginService);

  const token = loginService.getToken();

  // ✅ Clone request and add token
  let authReq = req;

  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  return next(authReq).pipe(
    catchError((error) => {
      const status = error?.status;

      if (status === 401) {
        loginService.logout();
        router.navigate(['/login']);
      }

      if (status === 500) {
        console.error('Server error occurred');
        router.navigate(['/login']);
      }

      return throwError(() => error);
    })
  );
};
