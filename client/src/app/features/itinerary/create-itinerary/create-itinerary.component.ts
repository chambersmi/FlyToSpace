import { Component, OnInit } from '@angular/core';
import { TourDto } from '../../../models/tour/tour-dto.model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ItineraryService } from '../../../services/itinerary/itinerary.service';
import { TourService } from '../../../services/tour/tour.service';
import { AuthService } from '../../../services/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateItineraryDto } from '../../../models/itinerary/create-itinerary-dto.model';
import { CommonModule } from '@angular/common';
import { CreateTourDto } from '../../../models/tour/create-tour-dto.model';
import { CartService } from '../../../services/cart/cart.service';
import { CartItem } from '../../../models/cart/cart.model';

@Component({
  selector: 'app-create-itinerary',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-itinerary.component.html',
  styleUrl: './create-itinerary.component.css'
})
export class CreateItineraryComponent implements OnInit {
  tourId!: number;
  itineraryForm!: FormGroup;
  maxSeats: number[] = [];
  tour: CreateTourDto | null = null;
  seatsAvailable: number = 0;
  totalPrice: number = 0;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private itineraryService: ItineraryService,
    private router: Router,
    private authService: AuthService,
    private tourService: TourService,
    private cartService:CartService
  ) { }

  ngOnInit(): void {
    this.tourId = Number(this.route.snapshot.paramMap.get('tourId'));
    this.loadTourDetails();

    this.itineraryForm = this.fb.group({
      seatsBooked: [1, [Validators.required, Validators.min(1)]]

    });
  }

  submitBooking(): void {
    // if (this.itineraryForm.invalid) {
    //   console.log("Invalid booking form.");
    //   return;
    // }

    // const user = this.authService.getUserFromToken();
    
    // if (!user) {      
    //   this.router.navigate(['/login']);
    //   return;
    // }

    // const dto: CreateItineraryDto = {
    //   tourId: this.tourId,
    //   userId: user.id,
    //   seatsBooked: this.itineraryForm.value.seatsBooked
    // };

    // this.itineraryService.createItinerary(dto).subscribe({
    //   next: res => {
    //     console.log('Booking successful.');
    //     this.router.navigate(['/']);
    //   },
    //   error: err => {
    //     console.error('Failed to book tour:\n', err);
    //     alert("Booking failed.");
    //   }
    // });

    if(this.itineraryForm.invalid) {
      console.log("Invalid itinerary.");
      return;
    }

    const user = this.authService.getUserFromToken();
    if(!user) {
      this.router.navigate(['/login']);
      return;
    }

    if(!this.tour) {
      console.error('Tour data did not load.');
      return;
    }

    const seats = this.itineraryForm.value.seatsBooked;

    const cartItem: CartItem = {
      tourId: this.tourId,
      tourName: this.tour.tourName,
      seats: seats,
      tourPrice: this.tour.tourPrice,
      totalPrice: seats * this.tour.tourPrice
    };

    this.cartService.addToCart(user.id, this.tourId, seats).subscribe({
      next:(data) => {
        console.log('Item added to cart.\n', data);
        this.router.navigate(['/cart']);
      },
      error: err => {
        console.error('Failed to add to cart:\n', err);
        alert("Add to cart failed!\n");
      }
    });
  }

  private loadTourDetails(): void {
    this.tourService.getTourById(this.tourId).subscribe(tour => {
      this.tour = tour;
      this.seatsAvailable = tour.maxSeats - tour.seatsOccupied;
      this.maxSeats = Array.from({ length: this.seatsAvailable }, (_, i) => i + 1);
    });
  }

  public onSeatsChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    const targetValue = target.value;

    if(targetValue) {
      const seats = Number(targetValue);
      console.log('Seats changed to ', targetValue);
      this.updateTotalPrice(seats);
    } else {
      console.warn('No value selected');
    }
  }

  private updateTotalPrice(seatsBooked:number) {
    if(this.tour) {
      this.totalPrice = seatsBooked * this.tour.tourPrice;
    }
  }

}
