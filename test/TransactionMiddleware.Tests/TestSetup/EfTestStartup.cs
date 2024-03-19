using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleWebApplication;
using SampleWebApplication.Persistence;
using SampleWebApplication.Persistence.EntityFramework;
using TransactionMiddleware.Ado.Abstraction;
using TransactionMiddleware.EntityFramework;

public class EfTestStartup(IConfiguration configuration) : Startup(configuration)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));

        services.AddScoped((_) => new SqlConnectionProvider(new SqlConnection(Configuration.GetConnectionString("Default"))));
        services.AddTransient<ITodoListRepository, TodoListRepository>();
        services.AddTransient<ITodoItemRepository, TodoItemRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApplicationLifetime appLiftime)
    {
        app.UseRouting();

        app.UseAuthorization();

        app.UseDbTransaction();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        appLiftime.ApplicationStarted.Register(() =>
        {
            var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            dbContext.Database.EnsureCreated();
        });
    }
}