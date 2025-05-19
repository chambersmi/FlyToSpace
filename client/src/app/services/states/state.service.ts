import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private readonly apiUrl = `${environment.apiUrl}/api/states`;

  constructor(private http: HttpClient) {}

  getStates(): Observable<{ [key: number]: string }> {
    return this.http.get<{ [key: number]: string }>(`${this.apiUrl}`);
  }
}
