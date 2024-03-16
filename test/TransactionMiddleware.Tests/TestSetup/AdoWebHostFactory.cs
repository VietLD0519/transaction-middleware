//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//namespace TransactionMiddleware.Tests.TestSetup;

//internal class AdoWebHostFactory
//{
//    public void CreateMsSqlWebHost()
//    {
//        var x = new WebHostBuilder().;
//    }

//    private void RegisterDependencies(IServiceCollection services, test)
//    {
//    }
//}

//public class IntegrationTestWebAppFactory : WebApplicationFactory<>,
//        IAsyncLifetime
//{
//    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
//        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
//        .WithPassword("Strong_password_123!")
//        .Build();

//    protected override void ConfigureWebHost(IWebHostBuilder builder)
//    {
//        builder.ConfigureTestServices(services =>
//        {
//            var descriptorType =
//                typeof(DbContextOptions<ApplicationDbContext>);

//            var descriptor = services
//                .SingleOrDefault(s => s.ServiceType == descriptorType);

//            if (descriptor is not null)
//            {
//                services.Remove(descriptor);
//            }

//            services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(_dbContainer.GetConnectionString()));
//        });

//        builder.
//    }

//    public Task InitializeAsync()
//    {
//        return _dbContainer.StartAsync();
//    }

//    public new Task DisposeAsync()
//    {
//        return _dbContainer.StopAsync();
//    }
//}