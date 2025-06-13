import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TourService } from '../../../services/tour/tour.service';
import { AuthService } from '../../../services/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CreateTourDto } from '../../../models/tour/create-tour-dto.model';
import { CartService } from '../../../services/cart/cart.service';
import { CartItem } from '../../../models/cart/cart.model';
import { environment } from '../../../../environments/environment';

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
    private router: Router,
    private authService: AuthService,
    private tourService: TourService,
    private cartService:CartService
  ) { }

  ngOnInit(): void {    
    this.tourId = Number(this.route.snapshot.paramMap.get('tourId'));
    this.loadTourDetails();

    this.itineraryForm = this.fb.group({
      seatsBooked: [0, [Validators.required, Validators.min(1)]]

    });
  }

  addToCart(): void {
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
   
    const seatsBooked = this.itineraryForm.value.seatsBooked;
    
    const cartItem: CartItem = {
      tourId: this.tourId,
      imageUrl: this.tour.imageUrl,
      tourName: this.tour.tourName,
      seatsBooked: seatsBooked,
      tourPrice: this.tour.tourPrice,
      totalPrice: seatsBooked * this.tour.tourPrice
    };

    this.cartService.addToCart(cartItem).subscribe({
      next: (data) => {
        console.log('Item was added to cart');
        this.router.navigate(['/cart/']);
      },
      error: (err) => {
        console.error('Failed to add to cart:\n', err);
        alert('Failed to add to cart.');
      }
    })
  }

  loadTourDetails(): void {
    this.tourService.getTourById(this.tourId).subscribe(tour => {
      this.tour = tour;
      this.seatsAvailable = tour.maxSeats - tour.seatsOccupied;
      this.maxSeats = Array.from({ length: this.seatsAvailable }, (_, i) => i + 1);
      this.tour.imageUrl = `${environment.apiUrl}${this.tour.imageUrl}`;
    });
  }

  onSeatsChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    const targetValue = target.value;

    if(targetValue) {
      const seats = Number(targetValue);
      this.updateTotalPrice(seats);
    } else {
      console.warn('No value selected');
    }
  }

  updateTotalPrice(seatsBooked:number) {
    if(this.tour) {
      this.totalPrice = seatsBooked * this.tour.tourPrice;
    }
  }

  cancel():void {
    this.router.navigateByUrl('/tours');
  }

}
