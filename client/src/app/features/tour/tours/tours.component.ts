import { Component, OnInit } from '@angular/core';
import { TourDto } from '../../../models/tour/tour-dto.model';
import { TourService } from '../../../services/tour/tour.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { CartService } from '../../../services/cart/cart.service';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-tours',
  imports: [CommonModule],
  templateUrl: './tours.component.html',
  styleUrl: './tours.component.css'
})
export class ToursComponent implements OnInit {
  tours: TourDto[] = [];
  error: string | null = null;
  isLoading = false;

  constructor(private tourService:TourService, private cartService: CartService, private router:Router, private authService:AuthService) {}

  ngOnInit(): void {
    this.isLoading = true;

    this.tourService.getAllTours().subscribe({
      next: (data) => {
        this.tours = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.log('Error with getting Tour data!\n', err); 
        this.isLoading = false;               
      }
    })
  }

  bookNow(tourId:number) {
    const user = this.authService.getUserFromToken();
    const seatsBooked = 0;

    if(!user) {
      this.router.navigate(['/login']);      
      return;
    }
    
    this.router.navigate(['/create-itinerary', tourId]);    
  }
}



  //   this.cartService.addToCart(user.id, {
  //     tourId: tourId,
  //     seatsBooked: seatsBooked
  //   }).subscribe({
  //     next: () => {
  //       console.log(`Successfully added tour ${tourId} to cart`)
  //       this.router.navigate(['/cart']);
  //     },
  //     error: (err) => {
  //       this.error = 'Could not add to cart.';
  //     }
  //   });
  // }