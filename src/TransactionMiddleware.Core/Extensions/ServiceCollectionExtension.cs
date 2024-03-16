using Microsoft.Extensions.DependencyInjection;
using TransactionMiddleware.OptionsBuilder;

namespace TransactionMiddleware;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddTransactionMiddleware(this IServiceCollection services, Action<TransactionOptionsBuilder> optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        var builder = new TransactionOptionsBuilder(services);
        optionsBuilder.Invoke(builder);

        return services;
    }
}