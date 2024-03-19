using Microsoft.Extensions.DependencyInjection;
using SampleWebApplication.Models;
using SampleWebApplication.Persistence.EntityFramework;
using System.Text;
using System.Text.Json;
using TransactionMiddleware.Tests.TestSetup;

namespace TransactionMiddleware.Tests;

public class AdoMiddlewareTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    private TestWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

        TodoListRequest request = new("Test", null);
        var body = JsonSerializer.Serialize(request);
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("api/todo-list", new StringContent(body));

        // Assert
        response.EnsureSuccessStatusCode();
    }
}

public class JsonContent(string content) : StringContent(content, Encoding.UTF8);