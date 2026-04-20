import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { CustomerService } from '../../../core/services/customer/customer-service';
import { LoginService } from '../../../core/services/login/login-service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-subscription',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './subscription.html',
})
export class Subscription {
  private readonly customerService = inject(CustomerService);
  private readonly auth = inject(LoginService);

  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly successMessage = signal<string | null>(null);

  readonly isFree = signal(this.auth.isFreeSubscription());

  async subscribe() {
    this.loading.set(true);
    this.errorMessage.set(null);
    this.successMessage.set(null);

    try {
      await firstValueFrom(this.customerService.addSubscription());

      this.successMessage.set('Subscription activated successfully 🎉');

      // 🔥 مهم جدًا: حدث الحالة بعد الاشتراك
      this.isFree.set(false);

    } catch (err: any) {
      const apiErrors = err?.error?.errors;

      if (apiErrors) {
        const message = Object.values(apiErrors).flat().join(' | ');
        this.errorMessage.set(message);
      } else {
        this.errorMessage.set('Subscription failed');
      }
    } finally {
      this.loading.set(false);
    }
  }
}