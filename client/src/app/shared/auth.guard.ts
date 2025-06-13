
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

export class AuthGuard implements CanActivate {
  
  constructor(
    private router:Router,
    private authService:AuthService
  ) {}

  canActivate(route:ActivatedRouteSnapshot, state:RouterStateSnapshot): boolean {
    const isLoggedIn = this.authService.getUserFromToken();
    if(isLoggedIn) {
      return true;
    }
    else
    {
      this.router.navigate(['/login'], {
        queryParams: {
          returnUrl: state.url
        }
      });
      return false;
    }
  }

}