import { Component, OnInit } from '@angular/core';
import { TourDto } from '../../../models/tour/tour-dto.model';
import { TourService } from '../../../services/tour/tour.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
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
  isAdmin: boolean = false;

  constructor(private tourService: TourService, private cartService: CartService, private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.loadTours();
    this.isLoading = true;
    this.isAdmin = this.authService.getUserRole() === 'Admin';


  }

  bookNow(tourId: number) {
    const user = this.authService.getUserFromToken();
    const seatsBooked = 0;

    if (!user) {
      this.router.navigate(['/login'], {
        queryParams: {
          returnUrl: `/create-itinerary/${tourId}`
        }
      });
      return;
    }
    this.router.navigate(['/create-itinerary', tourId]);
  }

  loadTours() {
    this.tourService.getAllTours().subscribe({
      next: (data) => {
        this.tours = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.log('Error with getting Tour data!\n', err);
        this.isLoading = false;
      }
    });
  }


  removeTour(tourId: number) {
    if (!confirm('Are you sure you want to delete this tour?')) return;

    this.tourService.removeTour(tourId).subscribe({
      next: () => {
        console.log("Tour removed");
        this.loadTours();
      },
      error: (err) => {
        console.log('Error deleting tour: ', err)
      }
    });
  }

  updateTour(tourId: number): void {
    this.router.navigate(['/admin/edit-tour', tourId]);
  }

}
