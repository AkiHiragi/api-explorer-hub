using api.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace api.Extensions;

public static class AddServices
{
    public static IServiceCollection AddServiceCollection(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API списка контактов"
            });
        });
        services.AddControllers();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddSingleton<IStorage>(new SqLiteStorage(connectionString));

        services.AddCors(opt =>
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithOrigins(configuration["client"]!);
            })
        );

        return services;
    }
}