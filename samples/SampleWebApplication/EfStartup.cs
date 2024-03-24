using Microsoft.EntityFrameworkCore;
using SampleWebApplication.Persistence;
using SampleWebApplication.Persistence.EntityFramework;
using TransactionMiddleware;
using TransactionMiddleware.EntityFramework;

namespace SampleWebApplication;

public class EfStartup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddTransactionMiddleware(options =>
        {
            options.UseEntityFramework<TodoDbContext>();
        });

        services.AddTransient<ITodoListRepository, TodoListRepository>();
        services.AddTransient<ITodoItemRepository, TodoItemRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostApplicationLifetime appLiftime)
    {
        app.UseApiExceptionHandling();

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