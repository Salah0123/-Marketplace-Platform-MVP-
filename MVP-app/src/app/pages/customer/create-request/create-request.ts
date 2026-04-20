import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';

import { Component, OnInit, inject, signal } from '@angular/core';
import { LocationService } from '../../../core/services/location/location-service';
import { CreateRequestService } from '../../../core/services/customer/create-request-service';
import { AiService, AiResponse } from '../../../core/services/ai/ai-service';
import { firstValueFrom } from 'rxjs';
import { Router } from '@angular/router';

interface CreateRequestData {
  title: string;
  description: string;
  latitude: string;
  longitude: string;
}

@Component({
  selector: 'app-create-request',
  standalone: true,
  imports: [FormsModule, InputTextModule, ButtonModule, CardModule],
  templateUrl: './create-request.html',
  styleUrl: './create-request.scss',
})
export class CreateRequest implements OnInit {
  private readonly locationService = inject(LocationService);
  private readonly createRequestService = inject(CreateRequestService);
  private readonly aiService = inject(AiService);
  private readonly router = inject(Router);

  readonly apiErrorMessage = signal<string | null>(null);

  readonly createRequestModel = signal<CreateRequestData>({
    title: '',
    description: '',
    latitude: '',
    longitude: '',
  });

  readonly loading = signal(false);

  readonly errors = signal<Partial<Record<keyof CreateRequestData, string>>>(
    {}
  );

  readonly aiLoading = signal(false);
  readonly aiResult = signal<AiResponse | null>(null);
  readonly aiError = signal<string | null>(null);

  ngOnInit() {
    this.loadLocation();
  }

  private loadLocation() {
    this.locationService
      .getPosition()
      .then((position) => {
        this.createRequestModel.update((current) => ({
          ...current,
          latitude: position.lat.toString(),
          longitude: position.lng.toString(),
        }));
      })
      .catch(() => {
        console.error('Unable to get location');
      });
  }

  updateModel(field: keyof CreateRequestData, value: string) {
    this.createRequestModel.update((current) => ({
      ...current,
      [field]: value,
    }));

    // live clear error on change
    this.errors.update((err) => ({
      ...err,
      [field]: undefined,
    }));
  }

  private validate(): boolean {
    const model = this.createRequestModel();
    const newErrors: any = {};

    if (!model.title || model.title.trim().length < 3) {
      newErrors.title = 'Title must be at least 3 characters';
    }

    if (!model.description || model.description.trim().length < 10) {
      newErrors.description = 'Description must be at least 10 characters';
    }

    if (!model.latitude || isNaN(Number(model.latitude))) {
      newErrors.latitude = 'Invalid latitude';
    }

    if (!model.longitude || isNaN(Number(model.longitude))) {
      newErrors.longitude = 'Invalid longitude';
    }

    this.errors.set(newErrors);

    return Object.keys(newErrors).length === 0;
  }

  async submit() {
    if (!this.validate()) return;
  
    this.loading.set(true);
  
    try {
      const body = {
        title: this.createRequestModel().title,
        description: this.createRequestModel().description,
        latitude: Number(this.createRequestModel().latitude),
        longitude: Number(this.createRequestModel().longitude),
      };
  
      await firstValueFrom(
        this.createRequestService.createRequest(body)
      );
  
      // ✅ success → navigate
      this.router.navigate(['/customer/my-requests']);
  
    } catch (error: any) {
      const apiError = error?.error?.errors;
  
      const message =
        apiError
          ? Object.values(apiError).flat().join(' | ')
          : 'Something went wrong while creating request';
  
      this.apiErrorMessage.set(message);
  
    } finally {
      this.loading.set(false);
    }
  }



  async enhanceWithAI() {
    const model = this.createRequestModel();
  
    if (!model.title || !model.description) {
      this.apiErrorMessage.set('Enter title and description first');
      return;
    }
  
    this.aiLoading.set(true);
    this.aiError.set(null);
  
    try {
      const result = await firstValueFrom(
        this.aiService.enhance({
          title: model.title,
          description: model.description,
        })
      );
  
      this.aiResult.set(result);
  
    } catch (err) {
      this.aiError.set('AI failed to analyze request');
    } finally {
      this.aiLoading.set(false);
    }
  }


  applyAI() {
    const ai = this.aiResult();
    if (!ai) return;
  
    this.createRequestModel.update((m) => ({
      ...m,
      description: ai.refinedDescription,
    }));
  }
}