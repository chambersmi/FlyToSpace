using API.Application.Interfaces.IServices;
using API.Application.Mapping;
using API.Application.Services;
using API.Application.Settings;
using API.Application.Validation;
using API.Data;
using API.Domain.Entities;
using API.Infrastructure;
using API.Infrastructure.Auth;
using API.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

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

            // Register services and repositories
            builder.Services.AddInfrastructure(builder.Configuration);


            // Configure JWT
            var jwtSection = builder.Configuration.GetSection("JwtSettings");
            var jwtSettings = jwtSection.Get<JwtSettings>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.Key)
                        || string.IsNullOrWhiteSpace(jwtSettings.Issuer) || string.IsNullOrWhiteSpace(jwtSettings.Audience))
                    {
                        throw new InvalidOperationException("JWT Settings are not configured properly for this application.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience, // 
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.Key))
                    };
                });

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            // End Configure JWT


            // Non-Infrastructure Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<GetStatesService>();
            builder.Services.AddScoped<ITourService, TourService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddControllers();                                

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

            // Register Authorization
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Allowing username characters so email can be used as a username
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

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

            try
            {
                app.Run();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
