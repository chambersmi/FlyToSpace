
using API.Application.Services;
using API.Domains.Models;
using API.Infrastructure;
using API.Infrastructure.Data.Repositories;
using API.Infrastructure.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .WriteTo.Console()
                .WriteTo.File("Logs/app.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting up application...");

                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<ITokenService, TokenService>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IUserService, UserService>();
                

                // SQLite
                builder.Services.AddDbContext<FTSDbContext>(options =>
                {
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<FTSDbContext>()
                    .AddDefaultTokenProviders();
             
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       var secretKey = builder.Configuration["JwtSettings:Key"] ?? throw new Exception("JWT Key not found!");
                       var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                        Console.WriteLine($"Key is: {secretKey}");
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = "FlyToSpaceApp",
                           ValidAudience = "FlyToSpaceApp",
                           IssuerSigningKey = symmetricKey
                       };
                   });
                

                builder.Services.AddControllers();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Add UserRoles
                using var scope = app.Services.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { UserRoles.Role_User, UserRoles.Role_Admin };
               
                foreach(var role in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed on startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
