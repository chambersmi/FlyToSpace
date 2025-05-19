import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { RegisterUserDto } from '../../models/auth/register-user-dto.model';
import { LoginDto } from '../../models/auth/login-dto.model';
import { Observable } from 'rxjs';
import { LoginResponse } from '../../models/auth/login-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/api/auth`;

  constructor(private httpClient: HttpClient) { }

  register(dto: RegisterUserDto): Observable<string> {
    return this.httpClient.post(`${this.apiUrl}/register`, dto, {
      responseType: 'text'
    });
  }

  login(dto: LoginDto): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/login`, dto);
  }
}
