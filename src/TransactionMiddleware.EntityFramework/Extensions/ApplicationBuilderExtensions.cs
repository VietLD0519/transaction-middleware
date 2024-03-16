using Microsoft.AspNetCore.Builder;

namespace TransactionMiddleware.EntityFramework;

/// <summary>
/// Adds middleware to the ASP.NET Core application pipeline to automatically open and manage database transactions for each request.
/// </summary>
/// <param name="app">The current instance of the IApplicationBuilder interface.</param>
/// <returns>
/// The current instance of the IApplicationBuilder interface, allowing for method chaining.
/// </returns>
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseDbTransaction(this IApplicationBuilder app)
    {
        app.UseMiddleware(typeof(EntityFrameworkTransactionMiddleware));

        return app;
    }
}