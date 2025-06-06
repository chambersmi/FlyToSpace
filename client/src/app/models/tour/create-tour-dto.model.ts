export interface CreateTourDto {
    tourName: string;
    tourDescription: string;
    tourPrice: number;
    maxSeats: number;
    seatsOccupied:number;
    imageUrl?:string;
}