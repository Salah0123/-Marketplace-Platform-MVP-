import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ProviderService, ServiceRequest } from '../../../core/services/provider/provider-service';
import { LocationService } from '../../../core/services/location/location-service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-my-jobs',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-jobs.html',
})
export class MyJobs implements OnInit {

  private readonly providerService = inject(ProviderService);
  private readonly locationService = inject(LocationService);

  readonly requests = signal<ServiceRequest[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly locationNames = signal<Record<number, string>>({});

  userLat = 0;
  userLng = 0;

  ngOnInit() {
    this.loadRequests();
  }

  async loadRequests() {
    this.loading.set(true);
    this.error.set(null);

    try {
      const position = await this.locationService.getPosition();

      this.userLat = position.lat;
      this.userLng = position.lng;

      const data = await firstValueFrom(
        this.providerService.getMyRequests()
      );

      const mapped = data.map(r => ({
        ...r,
        distance: this.getDistance(
          this.userLat,
          this.userLng,
          r.latitude,
          r.longitude
        )
      }));

      // this.loadLocations(mapped);

      this.requests.set(mapped);

    } catch (err) {
      this.error.set('Failed to load requests');
    } finally {
      this.loading.set(false);
    }
  }


  loadLocations(requests: ServiceRequest[]) {
    requests.forEach((req) => {
      this.locationService
        .getPlaceName(req.latitude, req.longitude)
        .subscribe((name) => {
          this.locationNames.update((map) => ({
            ...map,
            [req.id]: name,
          }));
        });
    });
  }


  async completeRequest(id: number) {
    try {
      await firstValueFrom(this.providerService.completeRequest(id));

      // ✅ update UI بدل reload
      this.requests.update(list =>
        list.map(r =>
          r.id === id ? { ...r, status: 'Completed' } : r
        )
      );

    } catch {
      alert('Failed to complete request');
    }
  }

  // 📏 distance (km)
  getDistance(lat1: number, lon1: number, lat2: number, lon2: number): number {
    const R = 6371;
    const dLat = this.toRad(lat2 - lat1);
    const dLon = this.toRad(lon2 - lon1);

    const a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(this.toRad(lat1)) *
      Math.cos(this.toRad(lat2)) *
      Math.sin(dLon / 2) *
      Math.sin(dLon / 2);

    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

    return R * c;
  }

  private toRad(value: number): number {
    return value * Math.PI / 180;
  }
}