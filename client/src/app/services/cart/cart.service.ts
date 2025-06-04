import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartItem } from '../../models/cart/cart.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly apiUrl = `${environment.apiUrl}/cart`;

  constructor(private httpClient:HttpClient) { }

  addToCart(userId:string, tourId:number, seats:number):Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/add/${userId}`, 
      {
        userId,
        tourId,        
        seats
      });
  }
  
  getCart(userId:string):Observable<CartItem[]> {
    return this.httpClient.get<CartItem[]>(`${this.apiUrl}/${userId}`);
  }

  removeFromCart(userId:string, tourId:number):Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/remove/${userId}/${tourId}`);
  }

  clearCart(userId:string):Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/clear/${userId}`);
  }
}
