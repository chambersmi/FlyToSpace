using API.Application.Interfaces.IRepositories;
using API.Application.Interfaces.IServices;
using API.Application.Mapping;
using API.Infrastructure.Auth;
using API.Infrastructure.Repositories;
using API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure
{

    public static class InfrastructureDependencyInjection
    {

        /// <summary>
        /// Adds repositories, AutoMapper, FluentValidation, DB connection separately.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">Database connection</param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add AppDbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));


            // Add Services and Repositories

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITourRepository, TourRepository>();
            services.AddScoped<IItineraryRepository, ItineraryRepository>();


                        

            return services;
        }
    }
}
