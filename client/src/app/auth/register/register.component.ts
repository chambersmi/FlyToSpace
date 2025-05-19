import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { StateService } from '../../services/states/state.service';
import { RegisterUserDto } from '../../models/auth/register-user-dto.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  providers: [AuthService]
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  apiUrl = environment.apiUrl;

  states: { [key: number]: string } = {};
  stateKeys: number[] = [];

  constructor(private fb:FormBuilder, 
    private authService: AuthService,
    private stateService: StateService,
    private router: Router,
    private route: ActivatedRoute) {}

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
      zipCode: ['', Validators.required]
    });

    this.stateService.getStates().subscribe((data) => {
      this.states = data;
      this.stateKeys = Object.keys(data).map(Number);
    });
  }

  onSubmit():void {
    if(this.registerForm.invalid)
      return;

    const dto: RegisterUserDto = this.registerForm.value;

    this.authService.register(dto).subscribe({
      next: (data) => {
        alert("Registeration Successful!");
        this.router.navigateByUrl(`${this.apiUrl}/login`);
      },
      error: (err) => {
        console.error("Registration Failed:\n" + err);
        alert("Something went wrong with registration.");
      }
    });
  }
}
