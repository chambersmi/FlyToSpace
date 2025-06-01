import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { TokenUserDto } from '../../models/auth/token-dto.model';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  isLoggedIn = false;
  userEmail: string  | null = null;
  user: TokenUserDto | null = null;

  constructor(private authService:AuthService) {}

  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((status) => {
      this.isLoggedIn = status;
      this.user = this.authService.getUserFromToken();
      this.userEmail = status && this.user ? this.user.email : null;      
      console.log(this.userEmail);
    });
  }

  logout(): void {
    this.authService.logout();
  }

}
