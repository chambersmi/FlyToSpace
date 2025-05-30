import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CreateBookingDto } from '../../models/bookTour/create-booking-dto.model';
import { BookingDto } from '../../models/bookTour/booking-dto.model';
import { Observable } from 'rxjs';
import { UpdateBookingDto } from '../../models/bookTour/update-booking-dto.model';

@Injectable({
  providedIn: 'root'
})
export class BookingTourService {
  private apiUrl = `${environment.apiUrl}/api/Booking`;

  constructor(private httpClient:HttpClient) { }

  createBooking(dto:CreateBookingDto):Observable<BookingDto> {
    return this.httpClient.post<BookingDto>(`${this.apiUrl}/create`, dto);
  }

  updateBooking(bookingId:number, dto:UpdateBookingDto):Observable<BookingDto> {
    return this.httpClient.put<BookingDto>(`${this.apiUrl}/update/${bookingId}`, dto);
  }

  deleteBookingById(bookingId:number): Observable<boolean> {
    return this.httpClient.delete<boolean>(`${this.apiUrl}/delete/${bookingId}`);
  }

  getBookingById(bookingId:number): Observable<BookingDto> {
    return this.httpClient.get<BookingDto>(`${this.apiUrl}/${bookingId}`);
  }

  getAllBookings():Observable<BookingDto[]> {
    return this.httpClient.get<BookingDto[]>(`${this.apiUrl}/itinerary`);
  }
}