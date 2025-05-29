import { TestBed } from '@angular/core/testing';

import { BookingTourService } from './booking-tour.service';

describe('BookingTourService', () => {
  let service: BookingTourService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookingTourService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
