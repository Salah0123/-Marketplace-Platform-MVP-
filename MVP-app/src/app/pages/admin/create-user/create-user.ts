import { Component, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { UserService } from '../../../core/services/admin/user-service';

interface RegisterModel {
  email: string;
  password: string;
  name: string;
}

@Component({
  selector: 'app-create-user',
  standalone: true,
  templateUrl: './create-user.html',
})
export class CreateUser {
  private readonly service = inject(UserService);

  readonly model = signal<RegisterModel>({
    email: '',
    password: '',
    name: '',
  });

  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly successMessage = signal<string | null>(null);

  update(field: keyof RegisterModel, value: string) {
    this.model.update((m) => ({ ...m, [field]: value }));
  }

  validate(): boolean {
    const m = this.model();

    if (!m.name.trim()) {
      this.errorMessage.set('Name is required');
      return false;
    }

    if (!m.email.includes('@')) {
      this.errorMessage.set('Invalid email');
      return false;
    }

    if (m.password.length < 8) {
      this.errorMessage.set('Password must be at least 8 characters');
      return false;
    }

    return true;
  }

  async submit() {
    if (!this.validate()) return;

    this.loading.set(true);
    this.errorMessage.set(null);
    this.successMessage.set(null);

    try {
      await firstValueFrom(this.service.registerUser(this.model()));

      this.successMessage.set('User created successfully ✅');

      // 🔥 Reset form (ناس كتير بتنسى دي)
      this.model.set({
        email: '',
        password: '',
        name: '',
      });

    } catch (err: any) {
      const apiErrors = err?.error?.errors;

      if (apiErrors) {
        const message = Object.values(apiErrors).flat().join(' | ');
        this.errorMessage.set(message);
      } else {
        this.errorMessage.set('Failed to create user');
      }
    } finally {
      this.loading.set(false);
    }
  }
}