import { Component, OnInit } from '@angular/core';
import { TourDto } from '../../../models/tour/tour-dto.model';
import { TourService } from '../../../services/tour/tour.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

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

  constructor(private tourService:TourService, private router:Router) {}

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

  bookTour(tourId:number) {
    console.log("Button clicked");
    this.router.navigate(['/create-booking', tourId]);
  }

}
