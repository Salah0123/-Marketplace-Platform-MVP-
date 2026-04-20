import { CommonModule } from '@angular/common';
import { Component, inject, signal, OnInit } from '@angular/core';
import { UserService, User } from '../../../core/services/admin/user-service';
import { Role, RoleService } from '../../../core/services/admin/role-service';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-management.html',
})
export class UserManagement implements OnInit {
  private readonly userService = inject(UserService);
  private readonly roleService = inject(RoleService);

  readonly users = signal<User[]>([]);
  readonly roles = signal<Role[]>([]);
  readonly loading = signal(false);
  readonly activeUserId = signal<string | null>(null);
  readonly errorMessage = signal<string | null>(null);

  ngOnInit() {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers() {
    this.loading.set(true);

    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }
  loadRoles() {
    this.roleService.getRoles().subscribe({
      next: (data) => this.roles.set(data),
    });
  }

  getRoleClass(role: string): string {
    switch (role.toLowerCase()) {
      case 'admin':
        return 'bg-danger';
      case 'provider':
        return 'bg-primary';
      case 'customer':
        return 'bg-success';
      default:
        return 'bg-secondary';
    }
  }
  toggleAssign(userId: string) {
    this.activeUserId.set(
      this.activeUserId() === userId ? null : userId
    );
  }


  assignRole(user: any, newRole: string) {
    if (!newRole) return;
  
    this.loading.set(true);
    this.errorMessage.set(null);
  
    const updatedRoles = [newRole];
  
    this.userService.assignRoles(user.id, updatedRoles).subscribe({
      next: () => {
        user.roles = updatedRoles;
        this.loading.set(false);
      },
      error: (err) => {
        this.loading.set(false);
  
        const apiErrors = err?.error?.errors;
  
        if (apiErrors) {
          const message = Object.values(apiErrors).flat().join(' | ');
          this.errorMessage.set(message);
        } else {
          this.errorMessage.set('Failed to assign role');
        }
      }
    });
  }
}