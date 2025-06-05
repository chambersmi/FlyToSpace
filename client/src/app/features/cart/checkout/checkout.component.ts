import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../services/user/user.service';

@Component({
  selector: 'app-checkout',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  private readonly apiUrl = `${environment.apiUrl}/api/cart`;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private userService: UserService) {
    this.checkoutForm = this.fb.group({
      email: ['', Validators.required],
      firstName: ['', Validators.required],
      middleName: [''],
      lastName: ['', Validators.required],
      streetAddress1: ['', Validators.required],
      streetAddress2: [''],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zipCode: ['', Validators.required],
      paymentMethod: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadUserInformation();
  }


  loadUserInformation(): void {
    var user = this.authService.getUserFromToken();
    if (!user) {
      this.router.navigate(['/login']);
      return;
    }

    this.userService.getAllUserInformation(user.id).subscribe({
      next: (user) => {
        this.checkoutForm?.patchValue({
          email: user.email,
          firstName: user.firstName,
          middleName: user.middleName,
          lastName: user.lastName,
          streetAddress1: user.streetAddress1,
          streetAddress2: user.streetAddress2,
          city: user.city,
          state: user.state,
          zipCode: user.zipCode
        });
      },
      error: (err) => {
        console.error('Could not load user data:', err);
        this.errorMessage = 'Failed to load user data';
      }
    });
  }

  submitCheckout(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formValue = this.checkoutForm.value;

    // Construct the payload with PascalCase keys expected by the API
    const checkoutPayload: any = {
      email: formValue.email,
      firstName: formValue.firstName,
      lastName: formValue.lastName,
      streetAddress1: formValue.streetAddress1,
      city: formValue.city,
      state: formValue.state,
      zipCode: formValue.zipCode
    };

    // Include optional fields if they have values
    if (formValue.middleName?.trim()) {
      checkoutPayload.MiddleName = formValue.middleName;
    }

    if (formValue.streetAddress2?.trim()) {
      checkoutPayload.StreetAddress2 = formValue.streetAddress2;
    }

    this.http.post(`${this.apiUrl}/checkout`, checkoutPayload).subscribe({
      next: (data) => {
        this.successMessage = 'Booking confirmed!';
        this.isLoading = false;
        setTimeout(() => this.router.navigate(['/itinerary']), 1500);
      },
      error: (err) => {
        this.errorMessage = 'Checkout failed. Please try again.';
        console.error('Checkout error:', err);
        if (err?.error?.errors) {
          console.table(err.error.errors); // Optional: Show validation detail
        }
        this.isLoading = false;
      }
    });
  }
}