using System.Data;
using Microsoft.Extensions.DependencyInjection;
using TransactionMiddleware.Ado.Abstraction;
using TransactionMiddleware.OptionsBuilder;

namespace TransactionMiddleware.Ado;

/// <summary>
/// Provides extension methods for configuring transaction options within an ASP.NET Core application.
/// </summary>
public static class TransactionOptionsBuilderExtensions
{
    /// <param name="optionsBuilder">The current instance of the TransactionOptionsBuilder.</param>
    /// <param name="connection">The ADO.NET connection to use for transactions.</param>
    /// <param name="isolationLevel">The isolation level for transactions (default: ReadCommitted).</param>
    public static void UseAdo(this TransactionOptionsBuilder optionsBuilder, IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        TransactionOptions.IsolationLevel = isolationLevel;
        ((ITransactionOptionsBuilder)optionsBuilder).Services.AddScoped(_ => new SqlConnectionProvider(connection));
    }
}