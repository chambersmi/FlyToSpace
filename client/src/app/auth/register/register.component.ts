import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { StateService } from '../../services/states/state.service';
import { RegisterUserDto } from '../../models/auth/register-user-dto.model';
import { CommonModule } from '@angular/common';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';


@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, MatSnackBarModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  providers: [AuthService],
  encapsulation: ViewEncapsulation.None
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  apiUrl = environment.apiUrl;
  states: { [key: number]: string } = {};
  stateKeys: number[] = [];
  error: string | null = null;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private stateService: StateService,
    private router: Router,
    private notificationService:NotificationService) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]],
      firstName: ['', Validators.required],
      middleName: [''],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      streetAddress1: ['', Validators.required],
      streetAddress2: [''],
      city: ['', Validators.required],
      state: [21, Validators.required], // defaults to MI
      zipCode: ['', Validators.required],
      role: ['User', Validators.required]
    });

    this.stateService.getStates().subscribe((data) => {
      this.states = data;
      this.stateKeys = Object.keys(data).map(Number);
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid) return;

    const formValue = this.registerForm.value;

    const dto: RegisterUserDto = {
      ...formValue,
      state: Number(formValue.state)
    };

    this.authService.register(dto).subscribe({
      next: () => {
        this.notificationService.success('Account successfully created!');
        this.router.navigate(['/login'], {
          queryParams: { email: dto.email }          
        });
      },
      error: (err) => {
        console.error("Registration Failed:\n", err);

        if (err.error) {
          if (typeof err.error === 'object' && err.error.message) {
            this.notificationService.error('Error: Something went wrong.');
          } else if (typeof err.error === 'string') {
            this.notificationService.error('Error: Account already exists with that email.');
          } else {
            this.notificationService.error('An unknown error occured.');
          }
        } else {
          this.notificationService.error('Could not connect to the server.');
        }
      }
    });
  }

}