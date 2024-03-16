using Microsoft.Extensions.DependencyInjection;

namespace TransactionMiddleware;

/// <summary>
/// Transaction middleware options builder
/// </summary>
public interface ITransactionOptionsBuilder
{
    /// <summary>
    /// Gets the services collection.
    /// </summary>
    /// <value>The services.</value>
    IServiceCollection Services { get; }
}