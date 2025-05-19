export interface RegisterUserDto {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  middleName?: string;
  lastName: string;
  dateOfBirth: string; 
  streetAddress1: string;
  streetAddress2?: string;
  city: string;
  state: number;
  zipCode: string;
}