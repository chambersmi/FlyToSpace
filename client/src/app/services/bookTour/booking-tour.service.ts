import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CreateBookingDto } from '../../models/bookTour/create-booking-dto.model';
import { BookingDto } from '../../models/bookTour/booking-dto.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookingTourService {
  private apiUrl = `${environment.apiUrl}/api/Booking`;

  constructor(private httpClient:HttpClient) { }

  createBooking(dto:CreateBookingDto):Observable<BookingDto> {
    return this.httpClient.post<BookingDto>(`${this.apiUrl}/create`, dto);
  }
}
