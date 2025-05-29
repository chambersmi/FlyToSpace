import { TokenUserDto } from "../auth/token-dto.model";

export interface CreateBookingDto {
    tourId: number;
    userId: string;
    seatsBooked:number;
}