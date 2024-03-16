using Microsoft.EntityFrameworkCore;
using SampleWebApplication.Models;

namespace SampleWebApplication.Persistence.EntityFramework;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoList>().Property(t => t.Title).HasMaxLength(128).IsRequired();
        modelBuilder.Entity<TodoList>().HasMany(tl => tl.Items).WithOne().HasForeignKey(ti => ti.TodoListId);

        base.OnModelCreating(modelBuilder);
    }
}