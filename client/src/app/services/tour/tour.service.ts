import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TourDto } from '../../models/tour/tour-dto.model';

@Injectable({
  providedIn: 'root'
})
export class TourService {
  private readonly apiUrl = `${environment.apiUrl}/api/tour`;

  constructor(private httpClient: HttpClient) { }

  getAllTours():Observable<TourDto[]> {
    return this.httpClient.get<TourDto[]>(`${this.apiUrl}/all`);
  }
}
