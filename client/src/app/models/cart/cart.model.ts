export interface CartItem {
    bookingId?:string;
    tourId:number;
    tourName:string;
    seatsBooked:number;
    tourPrice:number;
    totalPrice:number;
    imageUrl?:string;
}