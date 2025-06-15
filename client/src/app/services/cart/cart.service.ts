import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartItem } from '../../models/cart/cart.model';
import { BehaviorSubject, catchError, map, Observable, of, tap } from 'rxjs';
import { AddToCartRequest } from '../../models/cart/add-to-cart-request.model';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly apiUrl = `${environment.apiUrl}/cart`;
  private cartItemCount = new BehaviorSubject<number>(0);
  cartItemCount$ = this.cartItemCount.asObservable();

  constructor(private httpClient: HttpClient, private authService: AuthService) { }



  addToCart(req: AddToCartRequest): Observable<void> {
    const userId = this.getUserId();
    return this.httpClient.post<void>(`${this.apiUrl}/add/${userId}`, req).pipe(tap(() => this.updateCartCount()));
  }

  getCart(): Observable<CartItem[]> {
    const userId = this.getUserId();
    return this.httpClient.get<CartItem[]>(`${this.apiUrl}/get-all/${userId}`);
  }

  removeFromCart(bookingId: string): Observable<void> {
    const userId = this.getUserId();
    return this.httpClient.delete<void>(`${this.apiUrl}/remove/${userId}/${bookingId}`).pipe(tap(() => this.updateCartCount()));
  }

  clearCart(): Observable<void> {
    const userId = this.getUserId();
    return this.httpClient.delete<void>(`${this.apiUrl}/clear/${userId}`).pipe(tap(() => this.updateCartCount()));
  }

  private getUserId(): any {
    const user = this.authService.getUserFromToken();
    if (!user || !user.id) {
      throw new Error('User is not authenticated');
    }

    return user.id;
  }

  updateCartCount(): void {
  this.getCart().subscribe(cart => {
    this.cartItemCount.next(cart.length);
  });
}

  getCartItemCount(): Observable<number> {
    return this.getCart().pipe(
      map(cartItems => cartItems?.length || 0),
      catchError(error => {
        console.error('Failed fetching cart count: ', error);
        return of(0);
      })
    );
  }
}
