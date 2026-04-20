import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
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
  /** Present only on the client after computing distance from provider location */
  distance?: number;
}



@Injectable({
  providedIn: 'root',
})
export class ProviderService {
  private readonly http = inject(HttpClient);
  private readonly platformId = inject(PLATFORM_ID);

  private readonly baseUrl = `${environment.apiBaseUrl}/api/servicerequests/nearby`;
  private readonly requestsUrl = `${environment.apiBaseUrl}/api/servicerequests`;

  getNearbyRequests(lat: number, lng: number): Observable<ServiceRequest[]> {
    const params = new HttpParams()
      .set('providerLat', lat.toString())
      .set('providerLng', lng.toString());

    return this.http.get<ServiceRequest[]>(`${this.baseUrl}`, { params });
  }

  acceptRequest(id: number): Observable<void> {
    return this.http.post<void>(
      `${this.requestsUrl}/${id}/accept`,
      {}
    );
  }



  getMyRequests(): Observable<ServiceRequest[]> {
    return this.http.get<ServiceRequest[]>(
      `${this.requestsUrl}/provider`
    );
  }
  
  completeRequest(id: number): Observable<void> {
    return this.http.post<void>(
      `${this.requestsUrl}/${id}/complete`,
      {}
    );
  }
}
