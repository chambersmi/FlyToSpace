import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { RegisterUserDto } from '../../models/auth/register-user-dto.model';
import { LoginDto } from '../../models/auth/login-dto.model';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginResponse } from '../../models/auth/login-response.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenUserDto } from '../../models/auth/token-dto.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/api/auth`;
  private jwtHelper = new JwtHelperService();
  private readonly tokenKey = environment.getAuthToken;
  private isUserLoggedIn = new BehaviorSubject<boolean>(this.hasValidToken());

  constructor(private httpClient: HttpClient) { }


  register(dto: RegisterUserDto): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/register`, dto, {
      responseType: 'text' as 'json'
    }).pipe(
      tap((token: string) => {
        localStorage.setItem(this.tokenKey, token);
        this.isUserLoggedIn.next(true);
      })
    );
  }

  login(dto: LoginDto): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/login`, dto).pipe(
      tap((response) => {
        localStorage.setItem(this.tokenKey, response.token);
        this.isUserLoggedIn.next(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.isUserLoggedIn.next(false);
  }

  isAuthenticated(): Observable<boolean> {
    return this.isUserLoggedIn.asObservable();
  }

  hasValidToken(): boolean {
    const token = localStorage.getItem(this.tokenKey);
    return token != null && !this.jwtHelper.isTokenExpired(token);
  }

  getUserFromToken(): TokenUserDto | null {
    const token = localStorage.getItem(this.tokenKey);
    if (!token || this.jwtHelper.isTokenExpired(token)) {
      return null;
    }

    const decodedToken = this.jwtHelper.decodeToken(token);
    console.log("Decoded Token: ", decodedToken);
    
    return {
      id: decodedToken["id"],
      email: decodedToken["email"],
      role: decodedToken["role"]
    };
  }

  getUserRole(): string | null {
    const token = localStorage.getItem(this.tokenKey);
    if(!token || this.jwtHelper.isTokenExpired(token)) {
      return null;
    }

    const decodedToken = this.jwtHelper.decodeToken(token);
    return decodedToken["role"] || null;
  }
}





// register(dto: RegisterUserDto): Observable<string> {
//   return this.httpClient.post(`${this.apiUrl}/register`, dto, {
//     responseType: 'text'
//   });
// }

// login(dto: LoginDto): Observable<LoginResponse> {
//   return this.httpClient.post<LoginResponse>(`${this.apiUrl}/login`, dto);
// }