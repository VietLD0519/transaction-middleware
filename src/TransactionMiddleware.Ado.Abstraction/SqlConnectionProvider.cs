using System.Data;

namespace TransactionMiddleware.Ado.Abstraction;

/// <inheritdoc/>
public class SqlConnectionProvider : ISqlConnectionProvider
{
    private IDbTransaction _transaction = null!;
    private readonly IDbConnection _connection;

    public SqlConnectionProvider(IDbConnection connection)
    {
        _connection = connection;
    }

    public IDbConnection GetDbConnection => _connection;

    public IDbTransaction GetTransaction => _transaction;

    public IDbTransaction CreateTransaction(IsolationLevel isolationLevel)
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }

        _transaction = _connection.BeginTransaction(isolationLevel);

        return _transaction;
    }
}