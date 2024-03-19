using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleWebApplication;
using SampleWebApplication.Persistence.EntityFramework;
using Testcontainers.MsSql;

namespace TransactionMiddleware.Tests.TestSetup;

public class TestWebApplicationFactory : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .Build();

    public MsSqlContainer SqlDbContainer => _dbContainer;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(TodoDbContext));

            var connectionString = _dbContainer.GetConnectionString();
            services.AddDbContext<TodoDbContext>(options =>
                options.UseSqlServer(connectionString));
        });

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            //configBuilder.AddInMemoryCollection(
            //    new Dictionary<string, string>
            //    {
            //        ["Sec:Key"] = $""
            //    });
        });
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync() => await _dbContainer.StopAsync();
}