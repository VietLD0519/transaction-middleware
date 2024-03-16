using Dapper;
using SampleWebApplication.Models;
using System.Data;
using TransactionMiddleware.Ado.Abstraction;

namespace SampleWebApplication.Persistence.Dapper;

public class TodoItemRepository(SqlConnectionProvider connectionProvider) : ITodoItemRepository
{
    private readonly IDbConnection _connection = connectionProvider.GetDbConnection;

    public Task<int> AddTodoItemAsync(TodoItem todoItem)
    {
        const string command = "INSERT INTO TodoItems (Title, Note, TodoListId) VALUES (@Title, @Note, @TodoListId)";

        var parameters = new DynamicParameters();
        parameters.Add("Title", todoItem.Title, DbType.String);
        parameters.Add("Note", todoItem.Note, DbType.String);
        parameters.Add("TodoListId", todoItem.TodoListId, DbType.Int32);

        return _connection.ExecuteAsync(command, parameters, connectionProvider.GetTransaction);
    }

    public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(int todoListId)
    {
        return (await _connection.ExecuteScalarAsync<IEnumerable<TodoItem>>("SELECT * FROM TodoItems WHERE TodoListId = @Id", new { Id = todoListId }))
                ?? Enumerable.Empty<TodoItem>();
    }
}