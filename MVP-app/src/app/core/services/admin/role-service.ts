import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface Role {
  roleKey: string;
  displayName: string;
}

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private readonly http = inject(HttpClient);

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(
      `${environment.apiBaseUrl}/api/auth/roles`
    );
  }
}