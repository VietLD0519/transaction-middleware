using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TransactionMiddleware.Ado.Abstraction;

namespace TransactionMiddleware.Ado;

public class AdoTransactionMiddleware : TransactionMiddlewareBase
{
    private readonly RequestDelegate _next;

    public AdoTransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, SqlConnectionProvider connectionProvider, ILogger<AdoTransactionMiddleware> logger)
    {
        if (!ShouldOpenTransaction(httpContext))
        {
            await _next(httpContext);
            return;
        }

        IDbTransaction? transaction = null;

        try
        {
            transaction = connectionProvider.CreateTransaction(TransactionOptions.IsolationLevel);

            await _next(httpContext);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to commit the opened transaction. Error: {Error}", ex.Message);
            throw;
        }
        finally
        {
            transaction?.Dispose();
        }
    }
}