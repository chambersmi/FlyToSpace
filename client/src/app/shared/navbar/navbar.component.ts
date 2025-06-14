import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { TokenUserDto } from '../../models/auth/token-dto.model';
import { CartService } from '../../services/cart/cart.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  isLoggedIn = false;
  userEmail: string | null = null;
  user: TokenUserDto | null = null;
  itemsInCart = 0;
  isAdmin: boolean = false;

  constructor(
    private authService: AuthService, 
    private cartService: CartService,
    private router: Router) { }

  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((status) => {
      this.isLoggedIn = status;
      this.user = this.authService.getUserFromToken();
      this.userEmail = status && this.user ? this.user.email : null;
      console.log(this.userEmail);
    });

    this.isAdmin = this.authService.getUserRole() === 'Admin';

    if(this.user) {
      this.cartService.cartItemCount$.subscribe(count => {
        this.itemsInCart = count
      });
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

}
