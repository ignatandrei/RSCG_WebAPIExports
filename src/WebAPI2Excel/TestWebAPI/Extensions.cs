using Microsoft.AspNetCore.Rewrite;
namespace TestWebAPI;

public static class Extensions
{
    
    public static IServiceCollection AddExport(this IServiceCollection services, params Type[]? typesReturnedByActions)
    {
       
        MiddlewareExportToFile.AddReturnType(typeof(Person[]));
        MiddlewareExportToFile.AddReturnType(typeof(WeatherForecast[]));
        MiddlewareExportToFile.AddReturnTypes(typesReturnedByActions);

        return services.AddSingleton<MiddlewareExportToFile>();
    }
    public static IApplicationBuilder UseExport(this IApplicationBuilder app)
    {
        app.UseMiddleware<MiddlewareExportToFile>();
        var options = new RewriteOptions().Add(MiddlewareExportToFile.RewriteExtNeeded);
        app.UseRewriter(options);
        return app;
    }
}

