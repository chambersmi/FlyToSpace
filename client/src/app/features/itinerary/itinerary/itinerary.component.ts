import { Component, OnInit } from '@angular/core';
import { ItineraryService } from '../../../services/itinerary/itinerary.service';
import { ItineraryDto } from '../../../models/itinerary/itinerary-dto.model';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { UserDto } from '../../../models/auth/user-dto.model';
import { TokenUserDto } from '../../../models/auth/token-dto.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-itinerary',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './itinerary.component.html',
  styleUrl: './itinerary.component.css'
})
export class ItineraryComponent implements OnInit {
  itinerary: ItineraryDto[] = [];
  user: TokenUserDto | null = null;

  constructor(
    private itineraryService: ItineraryService,
    private authService: AuthService,
    private router:Router) {}


  ngOnInit(): void {
    this.loadBookings();

    this.user = this.authService.getUserFromToken();    

    if(!this.user) {
      this.router.navigate(['/login']);
      return;
    }
  }

  private loadBookings():void {
    this.itineraryService.getAllItinerariesByUserIdAsync().subscribe({
      next: (data) => {
        this.itinerary = data;
        console.log("Users bookings:");
        console.log(this.itinerary);
      },
      error: (err) => {
        console.error('Error loading bookings:\n', err)
      }
    });
  }
}
