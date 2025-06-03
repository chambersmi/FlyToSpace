import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ItineraryDto } from '../../models/itinerary/itinerary-dto.model';
import { Observable } from 'rxjs';
import { CreateItineraryDto } from '../../models/itinerary/create-itinerary-dto.model';
import { UpdateItineraryDto } from '../../models/itinerary/update-itinerary-dto.model';

@Injectable({
  providedIn: 'root'
})
export class ItineraryService {
  private readonly apiUrl = `${environment.apiUrl}/api/itinerary`;
  
  constructor(private httpClient:HttpClient) { }

  getAllItineraries():Observable<ItineraryDto[]> {
    return this.httpClient.get<ItineraryDto[]>(`${this.apiUrl}/get-all`);
  }

  getItineraryById(itineraryId:number):Observable<ItineraryDto> {
    return this.httpClient.get<ItineraryDto>(`${this.apiUrl}/${itineraryId}`);
  }

  getSingleUserItineraryById(itineraryId:number):Observable<ItineraryDto> {
    return this.httpClient.get<ItineraryDto>(`${this.apiUrl}/user/${itineraryId}`);
  }

  getAllItinerariesByUserIdAsync():Observable<ItineraryDto[]> {
    return this.httpClient.get<ItineraryDto[]>(`${this.apiUrl}/user/all`);
  }

  createItinerary(dto:CreateItineraryDto):Observable<CreateItineraryDto> {
    return this.httpClient.post<CreateItineraryDto>(`${this.apiUrl}/create`, dto);
  }

  updateItinerary(itineraryId:number, dto:UpdateItineraryDto):Observable<ItineraryDto> {
    return this.httpClient.put<ItineraryDto>(`${this.apiUrl}/update/${itineraryId}`, dto);
  }

  deleteItineraryById(itineraryId:number):Observable<boolean> {
    return this.httpClient.delete<boolean>(`${this.apiUrl}/delete/${itineraryId}`);
  }

  // Get price with taxes and fees from database
  getTotalPriceOfItinerary(totalPrice:number):Observable<number> {
    return this.httpClient.get<number>(`${this.apiUrl}/total-price/${totalPrice}`);
  }
}
