export interface UpdateTourDto {
    tourName: string;
    tourDescription: string;
    tourPackagePrice: number;
    maxSeats: number;
    seatsOccupied: number;
    imageUrl:string;
}