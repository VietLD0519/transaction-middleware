using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SampleWebApplication.Persistence;
using SampleWebApplication.Persistence.EntityFramework;
using TransactionMiddleware;
using TransactionMiddleware.Ado;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace SampleWebApplication;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));

        services.AddTransactionMiddleware(options =>
        {
            options.UseAdo(new SqlConnection(Configuration.GetConnectionString("Default")));
        });

        services.AddTransient<ITodoListRepository, Persistence.Dapper.TodoListRepository>();
        services.AddTransient<ITodoItemRepository, Persistence.Dapper.TodoItemRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApplicationLifetime appLiftime)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

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