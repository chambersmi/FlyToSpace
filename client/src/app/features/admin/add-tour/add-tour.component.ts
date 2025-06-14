import { Component, OnInit } from '@angular/core';
import { TourService } from '../../../services/tour/tour.service';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateTourDto } from '../../../models/tour/create-tour-dto.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-tour',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-tour.component.html',
  styleUrl: './add-tour.component.css'
})
export class AddTourComponent {
  tourForm: FormGroup;
  selectedFile: File | null = null;

  constructor(
    private tourService:TourService,
    private route: ActivatedRoute,
    private fb: FormBuilder) 
    {
      this.tourForm = this.fb.group({
        tourName: ['', Validators.required],
        tourDescription: ['', Validators.required],
        tourPrice: ['', Validators.required],
        maxSeats: [1, Validators.required],
        imageFile: [null],
      });    
    }

    onFileChange(event:any) {
      const file = event.target.files?.[0];
      if(file) {
        this.selectedFile = file;
      }
    }

    submitTour() {
      console.log("Submit clicked");
      if(this.tourForm.invalid) {
        console.log("Form invalid");
        return;
      }


      const createTourDto: CreateTourDto = {
        tourName: this.tourForm.get('tourName')?.value,
        tourDescription: this.tourForm.get('tourDescription')?.value,
        tourPrice: this.tourForm.get('tourPrice')?.value,
        maxSeats: this.tourForm.get('maxSeats')?.value,
        seatsOccupied: 0,
        imageUrl: ''
      };

      console.log(createTourDto);

      const formData = new FormData();

      Object.entries(createTourDto).forEach(([key, value]) => {
        if(key !== 'imageUrl' && value !== null && value !== undefined) {
          formData.append(key, value.toString());
        }
      });

      if(this.selectedFile) {
        formData.append('imageFile', this.selectedFile, this.selectedFile.name);
      }

      this.tourService.createTour(formData).subscribe({
        next: (res) => {
          console.log('Success', res);
          this.tourForm.reset();
          this.selectedFile = null;
        },
        error: (err) => {
          console.error('Error', err);
        }
      });
      
    }
}
