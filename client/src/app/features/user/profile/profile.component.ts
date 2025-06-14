import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { UserService } from '../../../services/user/user.service';
import { UpdateUserDto } from '../../../models/auth/update-user-dto.model';
import { UserDto } from '../../../models/auth/user-dto.model';
import { CommonModule } from '@angular/common';
import { StateService } from '../../../services/states/state.service';
import { NotificationService } from '../../../services/notification.service';
import { ItineraryDto } from '../../../models/itinerary/itinerary-dto.model';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})

export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  userId!: string;
  states: { [key: number]: string } = {};
  stateKeys: number[] = [];
  itinerary: ItineraryDto[] = [];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private stateService: StateService,
    private notification: NotificationService,    
  ) { }

  ngOnInit(): void {
    const user = this.authService.getUserFromToken();

    if (!user) {
      console.log(`${user} not found.`);
      return;
    }
    
    this.userId = user.id;
    
    this.loadUser();

  }

  // Populate form
  private initForm(user: UserDto): void {
    this.profileForm = this.fb.group({
      email: [{ value: user.email, disabled: true }],
      firstName: [user.firstName, Validators.required],
      middleName: [user.middleName],
      lastName: [user.lastName, Validators.required],
      dateOfBirth: [user.dateOfBirth, Validators.required],
      streetAddress1: [user.streetAddress1, Validators.required],
      streetAddress2: [user.streetAddress2 || ''],
      city: [user.city, Validators.required],
      state: [user.state, Validators.required],
      zipCode: [user.zipCode, Validators.required]
    });
  }

  // // Update new values
  // private updateDto(): UpdateUserDto {
  //   const formValue = this.profileForm.getRawValue();

  //   return {
  //     firstName: formValue.firstName,
  //     middleName: formValue.middleName,
  //     lastName: formValue.lastName,
  //     dateOfBirth: formValue.dateOfBirth,
  //     streetAddress1: formValue.streetAddress1,
  //     streetAddress2: formValue.streetAddress2,
  //     city: formValue.city,
  //     state: formValue.state,
  //     zipCode: formValue.zipCode
  //   };
  // }

  updateProfile(): void {
    if (this.profileForm.invalid) {
      console.warn("Invalid form.");
      return;
    }

    const formValue = this.profileForm.value;

    const updateDto: UpdateUserDto = {
      ...formValue,
      state: Number(formValue.state)
    };

    //const updateDto = this.updateDto();

    this.userService.updateUser(this.userId, updateDto).subscribe({
      next: () => {
        console.log('Profile updated.');
        this.notification.success('Profile updated!');
      },
      error: (err) => {
        console.log('Failed to update profile\n', err);
        this.notification.error('Error in updating profile.')
      }
    });
  }


  private loadUser() {
    this.userService.getUserById(this.userId).subscribe({
      next: (userData: UserDto) => {
        this.initForm(userData);
      },
      error: (err) => {
        console.error('Failed to fetch user data: ', err);
      }
    });

    this.stateService.getStates().subscribe((data) => {
      this.states = data;
      this.stateKeys = Object.keys(data).map(Number);
    });
  }
}
