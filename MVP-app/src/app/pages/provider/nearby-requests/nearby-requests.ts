import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ProviderService, ServiceRequest } from '../../../core/services/provider/provider-service';
import { LocationService } from '../../../core/services/location/location-service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-nearby-requests',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './nearby-requests.html',
  styleUrl: './nearby-requests.scss',
})
export class NearbyRequests implements OnInit {

  private readonly providerService = inject(ProviderService);
  private readonly locationService = inject(LocationService);


  readonly requests = signal<ServiceRequest[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly acceptingId = signal<number | null>(null);

  // 📍 موقع ال provider
  private userLat = 0;
  private userLng = 0;



  ngOnInit() {
    this.loadNearbyRequests();
  }

  


  async loadNearbyRequests() {
    this.loading.set(true);
    this.error.set(null);
  
    try {
      const position = await this.locationService.getPosition();
  
      this.userLat = position.lat;
      this.userLng = position.lng;
  
      this.providerService
        .getNearbyRequests(this.userLat, this.userLng)
        .subscribe({
          next: (data) => {
  
            const mapped = data.map(r => ({
              ...r,
              distance: this.getDistance(
                this.userLat,
                this.userLng,
                r.latitude,
                r.longitude
              )
            }));
  
            // ✅ sort بالأقرب
            mapped.sort((a, b) => (a.distance ?? 0) - (b.distance ?? 0));
  
            this.requests.set(mapped);
            this.loading.set(false);
          },
          error: () => {
            this.error.set('Failed to load nearby requests');
            this.loading.set(false);
          }
        });
  
    } catch (err) {
      this.error.set('Location permission denied');
      this.loading.set(false);
    }
  }


  getStatusClass(status: string): string {
    switch ((status || '').toLowerCase()) {
      case 'pending':
        return 'bg-warning text-dark';

      case 'accepted':
        return 'bg-primary';

      case 'completed':
        return 'bg-success';

      default:
        return 'bg-secondary';
    }
  }

  

  async acceptRequest(id: number): Promise<void> {
    this.acceptingId.set(id);
    this.error.set(null);

    try {
      await firstValueFrom(this.providerService.acceptRequest(id));
      this.requests.update((items) =>
        items.map((r) => (r.id === id ? { ...r, status: 'Accepted' } : r)),
      );
    } catch {
      this.error.set('Failed to accept request');
    } finally {
      this.acceptingId.set(null);
    }
  }


  getDistance(lat1: number, lng1: number, lat2: number, lng2: number): number {
    const R = 6371; // km
    const dLat = (lat2 - lat1) * Math.PI / 180;
    const dLng = (lng2 - lng1) * Math.PI / 180;
  
    const a =
      Math.sin(dLat/2) * Math.sin(dLat/2) +
      Math.cos(lat1 * Math.PI/180) *
      Math.cos(lat2 * Math.PI/180) *
      Math.sin(dLng/2) * Math.sin(dLng/2);
  
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
  
    return R * c;
  }
}
