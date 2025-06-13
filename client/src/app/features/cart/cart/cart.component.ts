import { Component, OnInit } from '@angular/core';
import { CartService } from '../../../services/cart/cart.service';
import { Router } from '@angular/router';
import { CartItem } from '../../../models/cart/cart.model';
import { environment } from '../../../../environments/environment';
import { CommonModule } from '@angular/common';
import { ItineraryService } from '../../../services/itinerary/itinerary.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-cart',
  imports: [CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})

export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  total = 0;
  
  constructor(
    private cartService:CartService, 
    private router:Router,
    private itineraryService:ItineraryService) {}
  
  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.cartService.getCart().subscribe(items => {      
      this.cartItems = items;

      const priceObserables = items.map(i =>
        this.itineraryService.getTotalPriceOfItinerary(i.tourId)
      );

      forkJoin(priceObserables).subscribe(prices => {
        this.total = prices.reduce((sum, price) => sum + price, 0);
      })
    });
  }

  removeItem(bookingId:string) {
    this.cartService.removeFromCart(bookingId).subscribe(() => {
      this.ngOnInit();
    });
  }

  clearCart() {
    this.cartService.clearCart().subscribe(() => {
      this.ngOnInit();
    });
  }

  proceedToCheckout() {
    this.router.navigate(['/cart/checkout']);
  }

}
