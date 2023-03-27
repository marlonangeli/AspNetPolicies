using System.Reflection;
using AspNetPolicies.Security.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AspNetPolicies.Api.Extensions;

public static class SwaggerSetup
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration, bool useOidc = false)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AspNetPolicies.Api",
                Version = "v1"
            });
            if (useOidc) options.ConfigureOidcSwagger(configuration);
        });
    }

    public static void UseSwaggerUI(this WebApplication app, IConfiguration configuration, bool useOidc = false)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var apiVersionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
            if (apiVersionProvider is null)
                throw new ArgumentException("API Versioning not registered");

            foreach (var description in apiVersionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
            }
            options.RoutePrefix = "swagger";
            
            if (useOidc) options.ConfigureOidcSwaggerUI(configuration);
        });
    }
}