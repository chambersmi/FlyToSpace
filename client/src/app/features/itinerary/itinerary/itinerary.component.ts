import { Component, OnInit } from '@angular/core';
import { ItineraryService } from '../../../services/itinerary/itinerary.service';
import { ItineraryDto } from '../../../models/itinerary/itinerary-dto.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-itinerary',
  imports: [CommonModule],
  templateUrl: './itinerary.component.html',
  styleUrl: './itinerary.component.css'
})
export class ItineraryComponent implements OnInit {
  itinerary:ItineraryDto[] = [];

  constructor(private itineraryService: ItineraryService) {}
  
  
  ngOnInit(): void {
    this.loadBookings();
  }

  
    private loadBookings() {
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
