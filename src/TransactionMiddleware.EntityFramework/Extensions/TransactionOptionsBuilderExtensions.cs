using System.Data;
using Microsoft.EntityFrameworkCore;
using TransactionMiddleware.OptionsBuilder;

namespace TransactionMiddleware.EntityFramework;

/// <summary>
/// Provides extension methods for configuring transaction options within an ASP.NET Core application.
/// </summary>
public static class TransactionOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the transaction options to use Entity Framework Core with the specified isolation level (if provided).
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext to use for transactions.</typeparam>
    /// <param name="optionsBuilder">The current instance of the TransactionOptionsBuilder.</param>
    /// <param name="isolationLevel">The optional isolation level for transactions. (default: null - inherits from the isolation level of the transaction is whatever
    /// isolation level the database provider considers its default setting.)
    /// </param>
    public static void UseEntityFramework<TDbContext>(this TransactionOptionsBuilder optionsBuilder, IsolationLevel? isolationLevel = null)
        where TDbContext : DbContext
    {
        TransactionOptions.IsolationLevel = isolationLevel;
        TransactionOptions.DbContextType = typeof(TDbContext);
    }
}