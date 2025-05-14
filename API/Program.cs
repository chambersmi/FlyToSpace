
using API.Infrastructure;
using FastEndpoints;

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

            // Add Fast Endpoints
            //builder.Services.AddFastEndpoints();

            // Add Swagger
            builder.Services.AddSwaggerDocument();

            //builder.Services.AddControllers();


            var app = builder.Build();

            //app.UseFastEndpoints();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //app.MapControllers();

            app.Run();
        }
    }
}
