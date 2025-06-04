import { Component, OnInit } from '@angular/core';
import { CartService } from '../../../services/cart/cart.service';
import { Router } from '@angular/router';
import { CartItem } from '../../../models/cart/cart.model';
import { environment } from '../../../../environments/environment';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  imports: [CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})

export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  total = 0;
  
  constructor(private cartService:CartService, private router:Router) {}
  
  ngOnInit(): void {
    const userId = localStorage.getItem(environment.getUserId)!;
    this.cartService.getCart(userId).subscribe(item => {
      this.cartItems = this.cartItems;
      this.total = this.cartItems.reduce((sum, i) => sum + i.totalPrice, 0);
    });
  }

  removeItem(tourId:number) {
    const userId = localStorage.getItem(environment.getUserId)!;
    this.cartService.removeFromCart(userId, tourId).subscribe(() => {
      this.ngOnInit();
    });
  }

  clearCart() {
    const userId = localStorage.getItem(environment.getUserId)!;
    this.cartService.clearCart(userId).subscribe(() => {
      this.ngOnInit();
    });
  }

}
