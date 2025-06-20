import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {

  constructor() { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem(environment.getAuthToken);
    if(token) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)        
      });
      return next.handle(cloned);
    } else {
      return next.handle(req);
    }    
  }
}
