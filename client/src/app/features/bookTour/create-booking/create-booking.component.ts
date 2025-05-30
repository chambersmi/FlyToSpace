import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { BookingTourService } from '../../../services/bookTour/booking-tour.service';
import { AuthService } from '../../../services/auth/auth.service';
import { CreateBookingDto } from '../../../models/bookTour/create-booking-dto.model';
import { TourService } from '../../../services/tour/tour.service';
import { TourDto } from '../../../models/tour/tour-dto.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-booking',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-booking.component.html',
  styleUrl: './create-booking.component.css'
})
export class CreateBookingComponent implements OnInit {
  tourId!:number;
  bookingForm!: FormGroup;
  maxSeats: number[] = [];
  tour:TourDto | null = null;
  seatsAvailable:number = 0;

  constructor(
    private route:ActivatedRoute, 
    private fb:FormBuilder,
    private bookingService:BookingTourService,
    private router:Router,
    private authService:AuthService,
    private tourService:TourService) {}

  ngOnInit(): void {
    this.tourId = Number(this.route.snapshot.paramMap.get('tourId'));
    
    this.loadTourDetails();

    this.bookingForm = this.fb.group({
      seatsBooked:[1,[Validators.required, Validators.min(1)]]      
    });
  }

  submitBooking():void {
    if(this.bookingForm.invalid) {
      console.log("Invalid booking form.");
      return;
    }

    const user = this.authService.getUserFromToken();
    if(!user) {
      alert('You must be logged in to book a tour.');
      this.router.navigate(['/login']);
      return;
    }

    const dto:CreateBookingDto = {
      tourId: this.tourId,
      userId: user.id,
      seatsBooked: this.bookingForm.value.seatsBooked
    };

    this.bookingService.createBooking(dto).subscribe({
      next: res => {
        console.log('Booking successful.');
        this.router.navigate(['/']);
      },
      error: err => {
        console.error('Failed to book tour:\n', err);
        alert("Booking failed.");
      }
    });
  }

  private loadTourDetails(): void {    
    this.tourService.getTourById(this.tourId).subscribe(tour => {
      this.tour = tour;      
      this.seatsAvailable = tour.maxSeats - tour.seatsOccupied;
      this.maxSeats = Array.from({length:this.seatsAvailable}, (_, i) => i + 1);
    });
  }

}
