import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { LoginDto } from '../../models/auth/login-dto.model';
import { LoginResponse } from '../../models/auth/login-response.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  returnUrl: string = '/';
  loginForm!: FormGroup;

  constructor(private fb:FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}    

  ngOnInit(): void {
        this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    // If user is coming from registration, prefill email
    this.route.queryParams.subscribe(params => {
      const paramEmail = params['email'];
      if(paramEmail) {
        this.loginForm.patchValue({
          email: paramEmail
        });
      }
    });
    
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
  
  onSubmit(): void {
    if(this.loginForm.invalid)
      return;

    const dto: LoginDto = this.loginForm.value;

    this.authService.login(dto).subscribe({
     next: (response: LoginResponse) => {
      localStorage.setItem('authToken', response.token);
      this.router.navigateByUrl(this.returnUrl);
     },
     error: (err) => {
      console.error(err);
     }
    });
  }

}
