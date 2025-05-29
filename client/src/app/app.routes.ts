import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ProfileComponent } from './features/user/profile/profile.component';
import { HomeComponent } from './features/home/home/home.component';
import { ToursComponent } from './features/tour/tours/tours.component';
import { CreateBookingComponent } from './features/bookTour/create-booking/create-booking.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'profile', component: ProfileComponent },
    { path: '', component: HomeComponent },
    { path: 'tours', component: ToursComponent} ,
    { path: 'create-booking/:tourId', component: CreateBookingComponent}   
];
