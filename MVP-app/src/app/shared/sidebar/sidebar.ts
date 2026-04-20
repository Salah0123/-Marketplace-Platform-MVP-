import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import { LoginService } from '../../core/services/login/login-service';




@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Sidebar implements OnInit {
  private readonly auth = inject(LoginService);
  private readonly router = inject(Router);

  readonly payload = computed(() => this.auth.getPayloadFromToken());
  readonly roles = computed(() => {
    const roles = this.auth.getUserRoles().map((r) => r.toLowerCase());
    return new Set(roles);
  });

  readonly isAdmin = computed(() => this.roles().has('admin'));
  readonly isCustomer = computed(() => this.roles().has('customer'));
  readonly isProvider = computed(() => this.roles().has('provider'));
  subscriptionStatus = signal<'Free' | 'Paid'>('Free');

  ngOnInit() {
  this.subscriptionStatus.set(
    this.auth.isFreeSubscription() ? 'Free' : 'Paid'
  );
}

  logout(): void {
    this.auth.logout();
    void this.router.navigate(['/login']);
  }
}
