import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartItem } from '../../models/cart/cart.model';
import { Observable } from 'rxjs';
import { AddToCartRequest } from '../../models/cart/add-to-cart-request.model';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly apiUrl = `${environment.apiUrl}/cart`;
  constructor(private httpClient:HttpClient, private authService:AuthService) { }



  addToCart(req:AddToCartRequest):Observable<void> {
    const userId = this.getUserId();
    return this.httpClient.post<void>(`${this.apiUrl}/add/${userId}`, req)
  }
    
  getCart():Observable<CartItem[]> {
    const userId = this.getUserId();
    return this.httpClient.get<CartItem[]>(`${this.apiUrl}/get-all/${userId}`);
  }

  removeFromCart(bookingId:string):Observable<void> {
    const userId = this.getUserId();
    return this.httpClient.delete<void>(`${this.apiUrl}/remove/${userId}/${bookingId}`);
  }

  clearCart():Observable<void> {
    const userId = this.getUserId();
    return this.httpClient.delete<void>(`${this.apiUrl}/clear/${userId}`);
  }

  private getUserId(): any {
    const user = this.authService.getUserFromToken();
    if(!user || !user.id) {
      throw new Error('User is not authenticated');
    }

    return user.id;
  }
}
