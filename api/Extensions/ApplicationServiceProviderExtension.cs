using System;
using api.Seed;
using api.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.Extensions;

public static class ApplicationServiceProviderExtension
{
    public static IServiceProvider AddCustomServices(
        this IServiceProvider provider,
        IConfiguration configuration)
    {
        using var scope = provider.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IInitializer>();
        initializer.Initialize(20);

        return provider;
    }
}