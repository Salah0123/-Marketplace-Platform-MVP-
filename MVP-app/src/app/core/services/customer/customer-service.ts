import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';


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
}

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/api/auth`;

  getMyRequests(): Observable<ServiceRequest[]> {
    return this.http.get<ServiceRequest[]>(
      `${environment.apiBaseUrl}/api/servicerequests/customer`
    );
  }

  addSubscription(): Observable<any> {
    return this.http.post(`${this.baseUrl}/add-subscription`, {});
  }
}
