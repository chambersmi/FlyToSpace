
using API.Application.Mapping;
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

            // Add services and repositories
            builder.Services.AddInfrastructure(builder.Configuration);

            // Add Authorization
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            try
            {
                app.Run();
            } catch(Exception ex)
            {
                Console.WriteLine("\n\n\nERROR!!!!!!!!!!!!!!!!!");
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
