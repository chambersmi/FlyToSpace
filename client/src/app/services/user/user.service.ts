import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { UserDto } from '../../models/auth/user-dto.model';
import { Observable } from 'rxjs';
import { CheckoutDto } from '../../models/cart/checkout-dto.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiUrl = `${environment.apiUrl}/user`;

  constructor(private httpClient:HttpClient) {}
  
  getUserById(id:string): Observable<UserDto> {
    return this.httpClient.get<UserDto>(`${this.apiUrl}/${id}`);
  }

  updateUser(id:string, user: Partial<UserDto>):Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/${id}`, user);
  }

  getAllUserInformation(id:string): Observable<CheckoutDto> {
    return this.httpClient.get<CheckoutDto>(`${this.apiUrl}/get-all-information`);
  }
  
}

