import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NearbyRequests } from './nearby-requests';

describe('NearbyRequests', () => {
  let component: NearbyRequests;
  let fixture: ComponentFixture<NearbyRequests>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NearbyRequests]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NearbyRequests);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
