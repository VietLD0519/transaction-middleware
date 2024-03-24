using SampleWebApplication;

namespace TransactionMiddleware.Tests;

public class AdoTransactionMiddlewareTest(TestWebApplication<AdoStartup> factory) : TestBase2<AdoStartup>(factory)
{
    [Fact]
    public async Task AdoCreateTodo_WhenNoExceptionThrown_TransactionCommit() =>
        await CreateTodo_WhenExceptionThrown_TransactionRollback();

    [Fact]
    public async Task AdoCreateTodo_WhenExceptionThrown_TransactionRollback() =>
        await CreateTodo_WhenExceptionThrown_TransactionRollback();
}

public class EfTransactionMiddlewareTest(TestWebApplication<EfStartup> factory) : TestBase2<EfStartup>(factory)
{
    [Fact]
    public async Task AdoCreateTodo_WhenNoExceptionThrown_TransactionCommit() =>
        await CreateTodo_WhenNoExceptionThrown_TransactionCommit();

    [Fact]
    public async Task AdoCreateTodo_WhenExceptionThrown_TransactionRollback() =>
        await CreateTodo_WhenExceptionThrown_TransactionRollback();
}