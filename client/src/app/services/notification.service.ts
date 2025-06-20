import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})

export class NotificationService {

  constructor(private snackBar:MatSnackBar) { }

  success(message:string): void {
    this.snackBar.open(message, 'X', {
      duration: 3000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: ['success-snackbar']
    });
  }

    error(message:string): void {
    this.snackBar.open(message, 'X', {
      duration: 3000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: ['error-snackbar'] // I tried this with ['error-snackbar'] also. neither make a difference.
    });
  }
}
