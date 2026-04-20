import { isPlatformBrowser, isPlatformServer } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../../environments/environment';

const TOKEN_STORAGE_KEY = 'mvp_auth_token';

export interface LoginRequest {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private readonly http = inject(HttpClient);
  private readonly platformId = inject(PLATFORM_ID);


  private getApiBaseUrl(): string {
  // إذا كنا داخل الحاوية (Server)، نستخدم اسم الخدمة
  if (isPlatformServer(this.platformId)) {
    return 'http://mvp-api:8080';
  }
  // إذا كنا في المتصفح (Browser)، نستخدم الرابط العام (أو relative path)
  return 'http://localhost:8080'; 
}

  login(body: LoginRequest): Observable<unknown> {
    const url = `${this.getApiBaseUrl()}/api/auth/login`;
    return this.http.post<unknown>(url, body).pipe(
      tap((response) => {
        const token = this.extractTokenFromLoginResponse(response);
        if (token) {
          this.saveToken(token);
          this.decodeJwtPayload(token);
        }
      }),
    );
  }

  getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    return localStorage.getItem(TOKEN_STORAGE_KEY);
  }

  /** Returns JWT payload claims, or `null` if there is no token or it is not a decodable JWT. */
  getPayloadFromToken(): Record<string, unknown> | null {
    const token = this.getToken();
    if (!token) {
      return null;
    }
    return this.decodeJwtPayload(token);
  }

  private saveToken(token: string): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }
    localStorage.setItem(TOKEN_STORAGE_KEY, token);
  }


  getUserRoles(): string[] {
    const payload = this.getPayloadFromToken();
    if (!payload) return [];
  
    const raw =
      payload['roles'] ??
      payload['role'] ??
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (Array.isArray(raw)) {
      return raw.filter((x) => typeof x === 'string') as string[];
    }

    if (typeof raw === 'string') {
      // Some APIs send comma-separated roles, others send a single role string
      return raw
        .split(',')
        .map((r) => r.trim())
        .filter(Boolean);
    }

    return [];
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }


  isTokenExpired(): boolean {
    const payload = this.getPayloadFromToken();
    if (!payload || !payload['exp']) return true;
  
    const exp = payload['exp'] as number;
    const now = Math.floor(Date.now() / 1000);
  
    return exp < now;
  }

  logout(): void {
    if (!isPlatformBrowser(this.platformId)) return;
    localStorage.removeItem(TOKEN_STORAGE_KEY);
  }

  private extractTokenFromLoginResponse(response: unknown): string | null {
    if (typeof response === 'string' && response.length > 0) {
      return response;
    }
    if (!response || typeof response !== 'object') {
      return null;
    }
    const r = response as Record<string, unknown>;
    const direct =
      r['token'] ?? r['accessToken'] ?? r['access_token'] ?? r['jwt'] ?? r['idToken'];
    if (typeof direct === 'string' && direct.length > 0) {
      return direct;
    }
    const data = r['data'];
    if (data && typeof data === 'object') {
      const d = data as Record<string, unknown>;
      const nested = d['token'] ?? d['accessToken'] ?? d['access_token'] ?? d['jwt'];
      if (typeof nested === 'string' && nested.length > 0) {
        return nested;
      }
    }
    return null;
  }

  private decodeJwtPayload(token: string): Record<string, unknown> | null {
    try {
      const parts = token.split('.');
      if (parts.length < 2 || !parts[1]) {
        return null;
      }
      const base64Url = parts[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=');
      const json = atob(padded);
      const payload = JSON.parse(json) as Record<string, unknown>;
      return payload;
    } catch {
      return null;
    }
  }


  isFreeSubscription(): boolean {
    const payload = this.getPayloadFromToken();
    if (!payload) return false;
  
    const value =
      payload['is_free_subscribtion'] ??
      payload['is_free_subscription'] ??
      payload['isFreeSubscription'];
  
    // JWT غالبًا بيرجع string مش boolean
    return value === true || value === 'true';
  }
}
