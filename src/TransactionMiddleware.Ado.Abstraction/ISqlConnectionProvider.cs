using System.Data;

namespace TransactionMiddleware.Ado.Abstraction;

/// <summary>
/// Defines an interface for providing connections and transactions to a SQL database.
/// </summary>
public interface ISqlConnectionProvider
{
    /// <summary>
    /// Gets a connection to the SQL database.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IDbConnection"/> representing the connection to the database.
    /// </value>
    IDbConnection GetDbConnection { get; }

    /// <summary>
    /// Gets the currently active transaction, if any.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IDbTransaction"/> representing the active transaction, or null if no transaction is active.
    /// </value>
    IDbTransaction GetTransaction { get; }

    /// <summary>
    /// Creates a new transaction with the specified isolation level.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the new transaction.</param>
    /// <returns>
    /// An instance of <see cref="IDbTransaction"/> representing the newly created transaction.
    /// </returns>
    IDbTransaction CreateTransaction(IsolationLevel isolationLevel);
}