import { Component, inject, signal, OnInit } from '@angular/core';
import { ServiceRequest, CustomerService } from '../../../core/services/customer/customer-service';
import { CommonModule } from '@angular/common';
import { LocationService } from '../../../core/services/location/location-service';


@Component({
  selector: 'app-my-requests',
  imports: [CommonModule],
  templateUrl: './my-requests.html',
  styleUrl: './my-requests.scss',
})
export class MyRequests implements OnInit {

  private readonly customerService = inject(CustomerService);
  private readonly locationService = inject(LocationService);

  readonly requests = signal<ServiceRequest[]>([]);
  readonly loading = signal(false);
  readonly locationNames = signal<Record<number, string>>({});

  ngOnInit() {
    this.loadRequests();
  }

  loadRequests() {
    this.loading.set(true);

    this.customerService.getMyRequests().subscribe({
      next: (data) => {
        this.requests.set(data);
        // this.loadLocations(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
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
}
