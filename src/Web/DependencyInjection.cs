using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TodoListWebApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;


namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();
        
        services.ConfigureHttpJsonOptions(opts =>{
            // serialize enums as strings for api
            opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.SerializerOptions.WriteIndented = true;
        });
        
        var appName = Assembly.GetExecutingAssembly().GetName().Name;
        services.AddSwaggerGen(opts =>
        {
            opts.DescribeAllParametersInCamelCase();
            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"{appName} API",
                Description = $"API documentation for {appName}",
                Version = "1.0.0"
            });
            opts.EnableAnnotations();

            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments
            var filePath = Path.Combine(AppContext.BaseDirectory, $"{appName}.xml");
            opts.IncludeXmlComments(filePath);
        });

        return services;
    }
}
