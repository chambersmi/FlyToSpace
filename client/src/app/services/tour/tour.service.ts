import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { TourDto } from '../../models/tour/tour-dto.model';
import { CreateTourDto } from '../../models/tour/create-tour-dto.model';
import { UpdateTourDto } from '../../models/tour/update-tour-dto.model';

@Injectable({
  providedIn: 'root'
})
export class TourService {
  private readonly apiUrl = `${environment.apiUrl}/tour`;


  constructor(private httpClient: HttpClient) { }

  getAllTours(): Observable<TourDto[]> {
    return this.httpClient.get<TourDto[]>(`${this.apiUrl}/all`).pipe(
      map(tours =>
        tours.map(tour => ({
          ...tour,
          imageUrl: `${environment.apiUrl}${tour.imageUrl}`
        }))
      )
    );
  }

  getTourById(id: number): Observable<TourDto> {
    return this.httpClient.get<TourDto>(`${this.apiUrl}/${id}`)
  }

  createTour(formData:FormData): Observable<CreateTourDto> {
    return this.httpClient.post<CreateTourDto>(`${this.apiUrl}/create`, formData);
  }

  removeTour(tourId:number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/delete/${tourId}`);
  }

  updateTour(tourId:number, formData:FormData): Observable<UpdateTourDto> {
    return this.httpClient.put<UpdateTourDto>(`${this.apiUrl}/update/${tourId}`, formData);
  }

}
