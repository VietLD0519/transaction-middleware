using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SampleWebApplication.Models;
using SampleWebApplication.Persistence.EntityFramework;
using System.Net;

namespace TransactionMiddleware.Tests;

public class TestBase2<TStartUp>(TestWebApplication<TStartUp> factory) : IClassFixture<TestWebApplication<TStartUp>>
    where TStartUp : class
{
    public async Task CreateTodo_WhenNoExceptionThrown_TransactionCommit()
    {
        // Arrange
        var title = "Test1";
        var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

        TodoListRequest request = new(title, new[] { new TodoItemRequest { Title = "Item 1", Note = "Some note" } });

        // Act
        var response = await factory.HttpClient.PostAsync("api/todo-list", new JsonContent(request));
        var todo = await dbContext.TodoLists.Include(td => td.Items).AsNoTracking().FirstOrDefaultAsync(td => td.Title == title);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(todo);
        Assert.Single(todo.Items);
    }

    public async Task CreateTodo_WhenExceptionThrown_TransactionRollback()
    {
        // Arrange
        var title = "Test2";
        var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

        TodoListRequest request = new(title, new[] { new TodoItemRequest { Title = "Item 1", Note = "Some note" } });

        // Act
        var response = await factory.HttpClient.PostAsync("api/todo-list/exception", new JsonContent(request));
        var todo = await dbContext.TodoLists.Include(td => td.Items).AsNoTracking().FirstOrDefaultAsync(td => td.Title == title);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Null(todo);
    }
}