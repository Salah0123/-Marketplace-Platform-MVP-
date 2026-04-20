import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface CreateServiceRequestDto {
  title: string;
  description: string;
  latitude: number;
  longitude: number;
}

@Injectable({
  providedIn: 'root',
})
export class CreateRequestService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/api/servicerequests`;

  createRequest(body: CreateServiceRequestDto): Observable<any> {
    return this.http.post(this.baseUrl, body);
  }
  
}