using API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DbInitializer
    {
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if(context == null)
            {
                throw new InvalidOperationException("Database Initializer failed to initialize.");
            }

            var dbPath = context.Database.GetDbConnection().DataSource;

            if(!File.Exists(dbPath))
            {
                context.Database.Migrate();
                //SeedData(context);
            }
        }
    }
}
