import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TourService } from '../../../../services/tour/tour.service';
import { UpdateTourDto } from '../../../../models/tour/update-tour-dto.model';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-edit-tour',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-tour.component.html',
  styleUrl: './edit-tour.component.css'
})
export class EditTourComponent implements OnInit {
  editTourForm!: FormGroup;
  tourId!: number;
  selectedFile: File | null = null;
  isLoading = true;
  error: string | null = null;
  imageUrl: string | null = null;

  constructor(
    private fb: FormBuilder,
    private tourService: TourService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.tourId = Number(this.route.snapshot.paramMap.get('tourId'));
    this.buildForm();
    this.loadTour();
  }

  private buildForm(): void {
    this.editTourForm = this.fb.group({
      tourName: ['', Validators.required],
      tourDescription: ['', Validators.required],
      tourPrice: [0, Validators.required],
      maxSeats: [1, Validators.required],
      imageFile: [null]
    });
  }

  private loadTour(): void {
    this.tourService.getTourById(this.tourId).subscribe({
      next: (tour: UpdateTourDto) => {
        // Patch all fields except the image
        this.editTourForm.patchValue({
          tourName: tour.tourName,
          tourDescription: tour.tourDescription,
          tourPrice: tour.tourPrice,
          maxSeats: tour.maxSeats,
        });
        this.imageUrl = tour.imageUrl ? `${environment.apiUrl}${tour.imageUrl}` : null;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load tour.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  onFileChange(event: any): void {
    const file = event.target.files?.[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onSubmit(): void {
    if (this.editTourForm.invalid) return;

    const updatedTour: UpdateTourDto = {
      tourName: this.editTourForm.get('tourName')?.value,
      tourDescription: this.editTourForm.get('tourDescription')?.value,
      tourPrice: this.editTourForm.get('tourPrice')?.value,
      maxSeats: this.editTourForm.get('maxSeats')?.value,

      imageUrl: ''
    };

    const formData = new FormData();
    Object.entries(updatedTour).forEach(([key, value]) => {
      if (value !== null && value !== undefined) {
        formData.append(key, value.toString());
      }
    });

    if (this.selectedFile) {
      formData.append('imageFile', this.selectedFile, this.selectedFile.name);
    }

    this.tourService.updateTour(this.tourId, formData).subscribe({
      next: (res) => {
        this.router.navigate(['/tours']);
      },
      error: (err) => {
        this.error = 'Failed to update tour.';
      }
    });
  }
}
