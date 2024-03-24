using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleWebApplication;
using SampleWebApplication.Persistence.EntityFramework;
using Testcontainers.MsSql;
using TransactionMiddleware.Ado;
using TransactionMiddleware.Ado.Abstraction;
using TransactionMiddleware.EntityFramework;

namespace TransactionMiddleware.Tests;

public class TestWebApplication<TStartup> : IAsyncLifetime
where TStartup : class
{
    private readonly string _connectionString;
    private readonly MsSqlContainer _dbContainer;
    private readonly TestServer _testServer;

    public TestWebApplication()
    {
        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithCleanUp(true)
            .Build();

        _dbContainer.StartAsync().GetAwaiter().GetResult();

        _connectionString = _dbContainer.GetConnectionString();

        var builder = new WebHostBuilder().UseStartup<TStartup>();
        builder.ConfigureServices(ConfigureTestServices);
        builder.ConfigureAppConfiguration(ConfigureAppConfiguration);

        _testServer = new TestServer(builder);
        HttpClient = _testServer.CreateClient();
        Services = _testServer.Services;
    }

    public HttpClient HttpClient { get; }

    public IServiceProvider Services { get; }

    private void ConfigureTestServices(IServiceCollection services)
    {
        RemoveService(typeof(TodoDbContext));
        RemoveService(typeof(SqlConnectionProvider));

        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(_connectionString));

        if (typeof(TStartup) == typeof(AdoStartup))
        {
            services.AddTransactionMiddleware(options =>
            {
                options.UseAdo(new SqlConnection(_connectionString));
            });
        }
        else
        {
            services.AddTransactionMiddleware(options =>
            {
                options.UseEntityFramework<TodoDbContext>();
            });
        }

        void RemoveService(Type serviceType)
        {
            var type = services.SingleOrDefault(d => d.ServiceType == serviceType);
            if (type != null)
            {
                services.Remove(type);
            }
        }
    }

    private void ConfigureAppConfiguration(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string>
            {
                ["ConnectionStrings:Default"] = _connectionString
            }!);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        _testServer.Dispose();
    }
}