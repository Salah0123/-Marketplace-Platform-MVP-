import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { email, form, FormField, FormRoot, minLength, required } from '@angular/forms/signals';
import { firstValueFrom } from 'rxjs';
import { Router } from '@angular/router';

import { LoginService } from '../../../core/services/login/login-service';

interface LoginData {
  email: string;
  password: string;
}

@Component({
  selector: 'app-login',
  imports: [FormField, FormRoot],
  templateUrl: './login.html',
  styleUrl: './login.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Login {
  private readonly loginService = inject(LoginService);
  private readonly router = inject(Router);
  readonly errorMessage = signal<string | null>(null);
  readonly loading = signal(false);

  readonly loginModel = signal<LoginData>({
    email: '',
    password: '',
  });

  readonly loginForm = form(
    this.loginModel,
    (path) => {
      required(path.email, { message: 'Email is required.' });
      email(path.email, { message: 'Enter a valid email address.' });
      required(path.password, { message: 'Password is required.' });
      minLength(path.password, 8, { message: 'Use at least 8 characters.' });
    },
    {
      submission: {
        action: async () => {
          try {
            this.errorMessage.set(null);
            this.loading.set(true);
      
            const body = this.loginModel();
            await firstValueFrom(this.loginService.login(body));
      
            const roles = this.loginService.getUserRoles();
      
            let targetRoute = '/home';
      
            if (roles.includes('Admin')) {
              targetRoute = '/admin/users';
            } else if (roles.includes('Provider')) {
              targetRoute = '/provider/nearby';
            } else if (roles.includes('Customer')) {
              targetRoute = '/customer/my-requests';
            }
      
            this.router.navigate([targetRoute]);
          } catch (error: any) {
            const apiErrors = error?.error?.errors;
      
            if (apiErrors) {
              const messages = Object.values(apiErrors).flat().join(' | ');
              this.errorMessage.set(messages);
            } else {
              this.errorMessage.set('Login failed. Please try again.');
            }
          } finally {
            this.loading.set(false);
          }
      
          return undefined;
        },
      }
    },
  );
}
