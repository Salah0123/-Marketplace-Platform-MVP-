import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface AiRequest {
  title: string;
  description: string;
}

export interface AiResponse {
  category: string;
  refinedDescription: string;
  suggestedPrice: number;
}

@Injectable({
  providedIn: 'root',
})
export class AiService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/api/AiIntegration`;

  enhance(data: AiRequest): Observable<AiResponse> {
    return this.http.post<AiResponse>(this.baseUrl, data);
  }
}