using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionMiddleware.EntityFramework;

public class EntityFrameworkTransactionMiddleware : TransactionMiddlewareBase
{
    private readonly RequestDelegate _next;

    public EntityFrameworkTransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider, ILogger<EntityFrameworkTransactionMiddleware> logger)
    {
        if (!ShouldOpenTransaction(httpContext))
        {
            await _next(httpContext);
            return;
        }

        IDbContextTransaction? transaction = null;

        try
        {
            var dbContext = (DbContext)serviceProvider.GetRequiredService(TransactionOptions.DbContextType);

            transaction = TransactionOptions.IsolationLevel != null
                ? await dbContext.Database.BeginTransactionAsync(TransactionOptions.IsolationLevel.Value)
                : await dbContext.Database.BeginTransactionAsync();

            await _next(httpContext);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to commit the opened transaction. Error: {Error}", ex.Message);
            throw;
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
        }
    }
}