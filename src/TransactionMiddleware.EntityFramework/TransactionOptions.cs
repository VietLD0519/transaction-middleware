using System.Data;

namespace TransactionMiddleware.EntityFramework;

internal static class TransactionOptions
{
    public static IsolationLevel? IsolationLevel { get; set; }

    public static Type DbContextType { get; set; } = null!;
}