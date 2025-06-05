import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-checkout',
  imports: [CommonModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit {
  isLoading = false;
  errorMessage = '';
  private readonly apiUrl = `${environment.apiUrl}/cart`;

  constructor(
    private httpClient:HttpClient,
    private authService:AuthService,
    private router:Router
  ) {}

  ngOnInit(): void {
    this.startCheckout();
  }

  private startCheckout(): void {
    this.isLoading = true;
    const user = this.authService.getUserFromToken();
    if(!user) {
      this.router.navigate(['/login']);
      return;
    }

    this.httpClient.post<{url:string}>(this.apiUrl + '/create-checkout-session', {}).subscribe({
      next: (res) => {
        console.log("Stripe session response: ", res);
        window.location.href = res.url;
      },
      error: (err) => {
        this.errorMessage= 'Failed to start checkout process';
        console.error(err);
        this.isLoading = false;
      }
    });
  }
}
