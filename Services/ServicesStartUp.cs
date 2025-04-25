using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Services;

public static class ServicesStartUp
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
