import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface ServiceRequest {
  id: number;
  title: string;
  description: string;
  status: string;
  latitude: number;
  longitude: number;
  createdAt: string;
  customer: string;
  provider?: string;
  distance?: number;
}

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/api/servicerequests`;

  getAllRequests(): Observable<ServiceRequest[]> {
    return this.http.get<ServiceRequest[]>(this.baseUrl);
  }

}