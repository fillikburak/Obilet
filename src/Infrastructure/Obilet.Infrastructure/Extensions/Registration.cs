
using Microsoft.Extensions.DependencyInjection;
using Obilet.Infrastructure.Interfaces;
using Obilet.Infrastructure.Services;

namespace Obilet.Infrastructure.Extensions;
public static class Registration
{
    public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services)
    {
        services.AddScoped<IServiceResponseHelper, ServiceResponseHelper>();
        services.AddTransient<IBusLocationService, BusLocationService>();
        services.AddTransient<IJourneyService, JourneyService>();

        return services;
    }
}
