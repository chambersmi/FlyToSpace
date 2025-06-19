import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { TokenUserDto } from '../../models/auth/token-dto.model';
import { CartService } from '../../services/cart/cart.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit, OnDestroy {
  isLoggedIn = false;
  userEmail: string | null = null;
  user: TokenUserDto | null = null;
  itemsInCart = 0;
  isAdmin: boolean = false;

  private subscriptions = new Subscription();

  @ViewChild('navbarCollapse') navbarCollapse!: ElementRef;
  
  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private router: Router) { }

  ngOnInit(): void {
    this.addSubscriptions();
  }

  hideNavbarCollapse(): void {
    if(this.navbarCollapse && this.navbarCollapse.nativeElement.classList.contains('show')) {
      this.navbarCollapse.nativeElement.classList.remove('show');

      const toggler = document.querySelector('.navbar-toggler[data-bs-target="#mainNavbar"]');

      if(toggler) {
        toggler.setAttribute('aria-expanded', 'false');
      }
    }
  }

  addSubscriptions(): void {
    this.subscriptions.add(this.authService.user$.subscribe(user => {
      this.user = user;
      this.isLoggedIn = !!user;
      this.userEmail = user?.email || null;
      this.isAdmin = user?.role === 'Admin';
    })
    );

    this.subscriptions.add(
      this.cartService.cartItemCount$.subscribe(count => {
        this.itemsInCart = count;
      })
    );
  }
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

}
