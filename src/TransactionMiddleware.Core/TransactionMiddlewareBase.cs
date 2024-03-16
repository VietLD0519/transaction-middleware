using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace TransactionMiddleware;

public abstract class TransactionMiddlewareBase
{
    protected bool ShouldOpenTransaction(HttpContext httpContext)
    {
        // For HTTP GET opening transaction is not required
        if (httpContext.Request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        }

        // If action is not decorated with TransactionAttribute then skip opening transaction
        var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<TransactionAttribute>();

        return attribute != null;
    }
}