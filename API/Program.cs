
using API.Application.Interfaces;
using API.Application.Mapping;
using API.Application.Services;
using API.Application.Settings;
using API.Domain.Entities;
using API.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Register JwtSettings from configuration
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JwtSettings"));

            // Register services and repositories
            builder.Services.AddInfrastructure(builder.Configuration);

            // Register Authorization
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUserService, UserService>();
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
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });


            var app = builder.Build();

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
