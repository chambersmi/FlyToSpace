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
    this.cartService.getCart().subscribe(item => {
      console.log(item);
      this.cartItems = item;
      this.total = this.cartItems.reduce((sum, i) => sum + i.totalPrice, 0);
    });
  }

  removeItem(tourId:number) {
    this.cartService.removeFromCart(tourId).subscribe(() => {
      this.ngOnInit();
    });
  }

  clearCart() {
    this.cartService.clearCart().subscribe(() => {
      this.ngOnInit();
    });
  }

}
