using API.Application.Services;
using API.Domain.Entities;
using API.Infrastructure;
using API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Data
{
    public class DbInitializer
    {

        /// <summary>
        /// Initializes the application database by appling pending migrations and seeding initial data, if needed.
        /// </summary>
        /// <param name="app">The current web application</param>
        /// <exception cref="InvalidOperationException">Thrown if the AppDbContext cannot be resolved</exception>
        public static async Task InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (context == null)
            {
                throw new InvalidOperationException("Database Initializer failed to initialize.");
            }

            var dbPath = context.Database.GetDbConnection().DataSource;

            var getPendingMigrations = context.Database.GetPendingMigrations();
            if (getPendingMigrations.Any())
            {
                context.Database.Migrate();
                Console.WriteLine("Migration applied.");
            }
            else
            {
                Console.WriteLine("No pending migrations.");
            }

            // Seed database mock users
            await SeedUserData(context, userManager);
            await SeedTourData(context);
            //await SeedBookingData(context, userManager);
        }

        /// <summary>
        /// Seeds the database with initial test users if no user exists.
        /// </summary>
        /// <param name="context">The database context used to access the identity user table</param>
        /// <param name="userManager">The database context used to access the identity user table.</param>
        /// <returns>A task that completes once seeding is finished</returns>
        /// <exception cref="Exception"></exception>
        public static async Task SeedUserData(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var seedUsers = new List<(ApplicationUser User, string Password)>
                {
                    (new ApplicationUser
                    {
                        Email = "kaleb@nasa.gov",
                        UserName = "kaleb@nasa.gov",
                        FirstName = "Kaleb",
                        MiddleName = "",
                        LastName = "Ziggyzoggy",
                        DateOfBirth = DateTime.UtcNow.AddYears(-14),
                        StreetAddress1 = "123 Main St.",
                        City = "McTesterTown",
                        State = Domain.Enums.StateEnum.MI,
                        ZipCode = "55527",
                    }, "Test123!"),

                    (new ApplicationUser
                    {
                        Email = "pebble@catmeow.gov",
                        UserName = "pebble@catmeow.gov",
                        FirstName = "Pebble",
                        MiddleName = "RussianBlue",
                        LastName = "Cat",
                        DateOfBirth = DateTime.UtcNow.AddYears(-3),
                        StreetAddress1 = "703 Tuna St",
                        City = "Catsville",
                        State = Domain.Enums.StateEnum.FL,
                        ZipCode = "23423"
                    }, "Test433!"),

                     (new ApplicationUser
                    {
                        Email = "oreo@catmeow.gov",
                        UserName = "oreo@catmeow.gov",
                        FirstName = "Oreo",
                        MiddleName = "Blueeyes",
                        LastName = "Cat",
                        DateOfBirth = DateTime.UtcNow.AddYears(-2),
                        StreetAddress1 = "7563 Tuna St",
                        City = "Meowsington",
                        State = Domain.Enums.StateEnum.AZ,
                        ZipCode = "23213"
                    }, "Bazoinks232!"),

                      (new ApplicationUser
                    {
                        Email = "rufus@woofmeow.gov",
                        UserName = "rufus@woofmeow.gov",
                        FirstName = "Rufus",
                        MiddleName = "Roofington",
                        LastName = "Zimson",
                        DateOfBirth = DateTime.UtcNow.AddYears(-10),
                        StreetAddress1 = "4392 Temptations St",
                        City = "Feline",
                        State = Domain.Enums.StateEnum.AK,
                        ZipCode = "87555"
                    }, "MCHammer433!"),

                       (new ApplicationUser
                    {
                        Email = "sim@graycats.gov",
                        UserName = "sim@graycats.gov",
                        FirstName = "Sim",
                        MiddleName = "",
                        LastName = "Gray",
                        DateOfBirth = DateTime.UtcNow.AddYears(-7),
                        StreetAddress1 = "391 Sparrow St",
                        City = "Ludington",
                        State = Domain.Enums.StateEnum.MI,
                        ZipCode = "98662"
                    }, "VerySecure123@")
                };

                foreach (var (user, password) in seedUsers)
                {
                    var existingUser = await userManager.FindByEmailAsync(user.Email!);
                    if (existingUser == null)
                    {
                        var result = await userManager.CreateAsync(user, password);

                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(err => err.Description))}");
                        }
                        else
                        {
                            Console.WriteLine($"Created user: {user.Email}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User {user.Email} already exists!");
                    }
                }

            }
        }

        public static async Task SeedTourData(AppDbContext context)
        {
            if (!context.Tours.Any())
            {
                var tour = new List<Tour>
                {
                    new Tour {
                        TourName = "Inner Solar System Fly-By!",
                        TourDescription = "Have a chance to fly near Mercury, and a full tour around Venus and Mars!",
                        TourPrice = 999.99m,
                        MaxSeats = 20,
                        SeatsOccupied = 0,
                        ImageUrl = "/assets/tourImages/InnerSolarSystemFlyBy.png"
                    },

                    new Tour
                    {
                        TourName = "Dazzle with Saturn",
                        TourDescription = "Experience the elegance of Saturn’s iconic rings up close. Glide past icy spokes and enjoy scenic views of Titan, Rhea, and Enceladus. Don’t forget your zero-gravity camera!",
                        TourPrice = 889.99m,
                        MaxSeats = 15,
                        SeatsOccupied = 0,
                        ImageUrl = "/assets/tourImages/DazzleWithSaturn.png"
                    },
                    new Tour
                    {
                        TourName = "Mars, Phobos and Deimos: Oh my!",
                        TourDescription = "Take a journey to Mars and its moons, Phobos and Deimos, where you’ll explore their mysterious surfaces and enjoy breathtaking views of the Red Planet. This tour offers a unique opportunity to witness Mars up close and experience the vast beauty of our solar system’s most intriguing celestial bodies.",
                        TourPrice = 299.99m,
                        MaxSeats = 18,
                        SeatsOccupied = 0,
                        ImageUrl = "/assets/tourImages/MarsPhobosDeimos.png"
                    },

                    new Tour
                    {
                        TourName = "Europa N' Chill Expedition",
                        TourDescription = "Cool off on Jupiter’s ice moon Europa. Marvel at its surface cracks and frozen ridges while our sub-ice drone explores what's beneath. Includes hot chocolate in orbit.",
                        TourPrice = 499.99m,
                        MaxSeats = 10,
                        SeatsOccupied = 0,
                        ImageUrl = "/assets/tourImages/EuropaAndChill.png"
                    },

                    new Tour
                    {
                        TourName = "Volcano Views of Io",
                        TourDescription = "For thrill-seekers! Witness the most volcanically active body in the solar system. Tour includes shielded observation pods and a lava-light show like no other.",
                        TourPrice = 747.99m,
                        MaxSeats = 8,
                        SeatsOccupied = 0,
                        ImageUrl = "/assets/tourImages/VolcanoViewsOfIo.png"

                    },
                    new Tour
                    {
                        TourName = "The Grand Outer Circuit",
                        TourDescription = "A 10-day journey across the gas giants: Jupiter, Saturn, Uranus, and Neptune. Full tours of each planet’s moons, rings, and atmospheric wonders. For the ultimate explorer.",
                        TourPrice = 1999.99m,
                        MaxSeats = 5,
                        SeatsOccupied = 0,
                        ImageUrl = "/assets/tourImages/VolcanoViewsOfIo.png"
                        },
                    };


                await context.Tours.AddRangeAsync(tour);
                await context.SaveChangesAsync();

                foreach(var tours in tour)
                {
                    Console.WriteLine($"{tours.TourName} added to database.");
                }
            }
                else
                {
                    Console.WriteLine($"Tour already exists.");
                }
            }

        public static async Task SeedBookingData(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Itineraries.Any())
            {
                var user = await userManager.FindByEmailAsync("kaleb@nasa.gov");
                if (user == null)
                {
                    Console.WriteLine($"Cannot seed. User was not found.");
                    return;
                }

                var tour = await context.Tours.FirstOrDefaultAsync();

                if (tour == null)
                {
                    Console.WriteLine("Cannot seed booking: tour was not found.");
                    return;
                }

                var booking = new Itinerary
                {
                    TourId = tour.TourId,
                    Tour = tour,
                    UserId = user.Id,
                    FlightId = new Random().Next(1, 1000),
                    SeatsBooked = 1,
                    Status = "Confirmed",
                    BookingDate = DateTime.UtcNow
                };

                await context.Itineraries.AddAsync(booking);
                await context.SaveChangesAsync();

                Console.WriteLine($"Seeded booking for user {user.Email} on tour {tour.TourName}");
            }
            else
            {
                Console.WriteLine("Bookings already exist. No seeding required.");

            }
        }
    }
}