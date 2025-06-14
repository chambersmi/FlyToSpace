import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ProfileComponent } from './features/user/profile/profile.component';
import { HomeComponent } from './features/home/home/home.component';
import { ToursComponent } from './features/tour/tours/tours.component';
import { CreateItineraryComponent } from './features/itinerary/create-itinerary/create-itinerary.component';
import { ItineraryComponent } from './features/itinerary/itinerary/itinerary.component';
import { CartComponent } from './features/cart/cart/cart.component';
import { CheckoutComponent } from './features/cart/checkout/checkout.component';
import { ConfirmationComponent } from './features/cart/confirmation/confirmation.component';
import { AboutUsComponent } from './features/about/about-us/about-us.component';
import { AuthGuard } from './shared/auth.guard';
import { UnauthorizedComponent } from './features/unauthorized/unauthorized/unauthorized.component';
import { AddTourComponent } from './features/admin/tour/add-tour/add-tour.component';
import { EditTourComponent } from './features/admin/tour/edit-tour/edit-tour.component';


export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
    { path: '', component: HomeComponent },
    { path: 'tours', component: ToursComponent },
    { path: 'create-itinerary/:tourId', component: CreateItineraryComponent },
    { path: 'itinerary', component: ItineraryComponent, canActivate: [AuthGuard] },
    { path: 'cart', component: CartComponent },
    { path: 'cart/checkout', component: CheckoutComponent },
    { path: 'cart/confirmation', component: ConfirmationComponent },
    { path: 'about-us', component: AboutUsComponent },
    { path: 'unauthorized', component: UnauthorizedComponent },
    { path: 'admin/add-tour', component: AddTourComponent, canActivate: [AuthGuard], data: { role: 'Admin' } },
    { path: 'admin/edit-tour/:tourId', component: EditTourComponent, canActivate: [AuthGuard], data: { role: 'Admin' } }
];
