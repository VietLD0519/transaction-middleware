using System.Data;

namespace TransactionMiddleware.Ado;

internal static class TransactionOptions
{
    public static IsolationLevel IsolationLevel { get; set; }
}