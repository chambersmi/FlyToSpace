using API.Application.Interfaces.IServices;
using API.Application.Mapping;
using API.Application.Services;
using API.Data;
using API.Domain.Entities;
using API.Infrastructure;
using API.Infrastructure.Services;
using API.Utilities;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Register services and repositories and sets up Database DBContext
            builder.Services.AddInfrastructure(builder.Configuration);

            // Sets up JWT Authentication and Authorization
            builder.Services.AddJwtAuthenticationAndAuthorization(builder.Configuration);
            builder.Services.AddHttpContextAccessor();


            // Non-Infrastructure Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<GetStatesService>();
            builder.Services.AddScoped<ITourService, TourService>();
            builder.Services.AddScoped<IItineraryService, ItineraryService>();
            builder.Services.AddScoped<IStripeService, StripeService>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration(builder.Configuration);
            builder.Services.AddControllers();

            // Redis
            var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
            if(string.IsNullOrEmpty(redisConnectionString))
            {
                throw new InvalidOperationException("Redis connection string is not configured.");
            }

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // CORS Policy
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularApp", policy =>
                {
                    policy.WithOrigins(allowedOrigins!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // Allowing username characters so email can be used as a username
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            // Take out after dev
            IdentityModelEventSource.ShowPII = true;

            var app = builder.Build();

            // Initialize database and seed data (if applicable)
            await DbInitializer.InitDb(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "FlyToSpace API V1");
                    s.RoutePrefix = string.Empty;
                });

                app.UseSwagger();
            }

            app.UseHttpsRedirection();
            app.UseCors("AngularApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //app.Urls.Add("http://*:80");
            app.Use(async (context, next) =>
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Authorization Header: {authHeader}");
                await next.Invoke();
            });

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
