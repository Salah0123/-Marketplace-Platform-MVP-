
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface User {
  id: string;
  userName: string;
  isActive: boolean;
  isFreeSubscription: boolean;
  displayName: string;
  email: string;
  roles: string[];
}

export interface RegisterRequest {
  email: string;
  password: string;
  name: string;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/api/auth`;

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.apiBaseUrl}/api/auth`);
  }
  assignRoles(userId: string, roles: string[]): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/${userId}/roles`,
      roles
    );
  }


  registerUser(body: RegisterRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, body);
  }
}