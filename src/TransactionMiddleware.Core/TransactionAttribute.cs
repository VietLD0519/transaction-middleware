namespace TransactionMiddleware;

/// <summary>
/// Represents an attribute associated with a transaction.
/// In ASP.NET Core, apply this attribute to controller actions to automatically initiate a transaction.
/// </summary>
/// <remarks>
/// This class inherits from the base <see cref="Attribute"/> class and provides additional functionality specific to transaction-related attributes.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class TransactionAttribute : Attribute
{
}