import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { UserDto } from '../../models/auth/user-dto.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiUrl = `${environment.apiUrl}/api`;

  constructor(private httpClient:HttpClient) {}
  
  getUserById(id:string): Observable<UserDto> {
    return this.httpClient.get<UserDto>(`${this.apiUrl}/user/${id}`);
  }

  updateUser(id:string, user: Partial<UserDto>):Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/user/${id}`, user);
  }
  
}
