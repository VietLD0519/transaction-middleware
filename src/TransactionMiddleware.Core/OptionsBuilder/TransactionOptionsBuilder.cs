using Microsoft.Extensions.DependencyInjection;

namespace TransactionMiddleware.OptionsBuilder;

/// <summary>
/// Transaction middleware options builder class, used during app services registration.
/// Implements <see cref="ITransactionOptionsBuilder"/>.
/// </summary>
public class TransactionOptionsBuilder : ITransactionOptionsBuilder
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// It creates an instance of <see cref="ITransactionOptionsBuilder"/>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public TransactionOptionsBuilder(IServiceCollection services)
    {
        _services = services;
    }

    IServiceCollection ITransactionOptionsBuilder.Services => _services;
}