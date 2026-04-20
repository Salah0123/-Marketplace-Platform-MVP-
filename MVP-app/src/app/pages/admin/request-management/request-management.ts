import { CommonModule } from '@angular/common';
import { Component, inject, signal, OnInit } from '@angular/core';
import { AdminService, ServiceRequest } from '../../../core/services/admin/admin-service';
import { LocationService } from '../../../core/services/location/location-service';

@Component({
  selector: 'app-request-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './request-management.html',
})
export class RequestManagement implements OnInit {
  private readonly adminService = inject(AdminService);
  private readonly locationService = inject(LocationService);

  readonly requests = signal<ServiceRequest[]>([]);
  readonly loading = signal(false);

  userLat = 0;
  userLng = 0;

  ngOnInit() {
    this.loadRequests();
  }

  async loadRequests() {
    this.loading.set(true);

    try {
      const position = await this.locationService.getPosition();

      this.userLat = position.lat;
      this.userLng = position.lng;

      this.adminService.getAllRequests().subscribe({
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

          this.requests.set(mapped);
          this.loading.set(false);
        },
        error: () => {
          this.loading.set(false);
        },
      });

    } catch {
      this.loading.set(false);
    }
  }
  getStatusClass(status: string): string {
    switch (status?.toLowerCase()) {
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