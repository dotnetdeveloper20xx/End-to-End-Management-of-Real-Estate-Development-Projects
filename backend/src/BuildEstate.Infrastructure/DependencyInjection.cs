using BuildEstate.Application.Abstractions;
using BuildEstate.Application.Common.Repositories;
using BuildEstate.Application.Land.Repositories;
using BuildEstate.Infrastructure.Persistence;
using BuildEstate.Infrastructure.Repositories.Land;
using BuildEstate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildEstate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<ILandOpportunityRepository, LandOpportunityRepository>();

        return services;
    }
}