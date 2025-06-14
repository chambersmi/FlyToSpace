
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {
  
  constructor(
    private router:Router,
    private authService:AuthService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
  const user = this.authService.getUserFromToken();

  if (!user) {
    this.router.navigate(['/login'], {
      queryParams: { returnUrl: state.url }
    });
    return false;
  }

  const expectedRole = route.data['role'] as string;

  if (expectedRole && user.role.toLowerCase() !== expectedRole.toLowerCase()) {
    this.router.navigate(['/unauthorized']);
    return false;
  }

  return true;
}

}