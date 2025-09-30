using Planne.Client.Domain.Services.Interfaces;

namespace Planne.UI.Config;

public static class DependencyInjection
{
    public static IServiceCollection ResolveDependency(this IServiceCollection services)
    {
        var serviceAssembly = typeof(IHttpClientService).Assembly;

        var registrationsServices = serviceAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                ImplementationType = t,
                ServiceType = t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}")
            })
            .Where(t => t.ServiceType != null);

        foreach (var registration in registrationsServices)
        {
            services.AddScoped(registration.ServiceType!, registration.ImplementationType);
        }

        return services;
    }
}
