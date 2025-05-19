import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { RegisterUserDto } from '../../models/auth/register-user-dto.model';
import { LoginDto } from '../../models/auth/login-dto.model';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginResponse } from '../../models/auth/login-response.model';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/api/auth`;
  private jwtHelper = new JwtHelperService();
  //private isUserLoggedIn = new BehaviorSubject<boolean>(this.hasValidToken());


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
