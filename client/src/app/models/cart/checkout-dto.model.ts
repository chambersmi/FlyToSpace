 export interface CheckoutDto {
    userId:string;
    email:string;
    firstName:string;
    middleName?:string;
    lastName:string;
    dateOfBirth:string;
    streetAddress1:string;
    streetAddress2?:string;
    city:string;
    state:string;
    zipCode:string;
    paymentMethod:string;
 }
 