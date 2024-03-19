using Dapper;
using SampleWebApplication.Models;
using System.Data;
using TransactionMiddleware.Ado.Abstraction;

namespace SampleWebApplication.Persistence.Dapper;

public class TodoListRepository(SqlConnectionProvider connectionProvider) : ITodoListRepository
{
    private readonly IDbConnection _connection = connectionProvider.GetDbConnection;

    public async Task<int> AddAsync(TodoList todoList)
    {
        const string command = "INSERT INTO TodoLists (Title) " +
                               "OUTPUT Inserted.Id " +
                               "VALUES (@Title)";

        var parameters = new DynamicParameters();
        parameters.Add("Title", todoList.Title, DbType.String);

        var id = await _connection.ExecuteScalarAsync<int>(command, parameters, connectionProvider.GetTransaction);

        return id;
    }

    public async Task<IEnumerable<TodoList>> GetListsAsync()
    {
        return (await _connection.ExecuteScalarAsync<IEnumerable<TodoList>>("SELECT * FROM TodoLists")) ?? Enumerable.Empty<TodoList>();
    }

    public async Task<TodoList?> GetByIdAsync(int id)
    {
        const string command = "SELECT tl.Id, tl.Title, ti.Id, ti.TodoListId, ti.Title, od.Note " +
                               "FROM TodoLists tl " +
                               "INNER JOIN TodoItems ti ON tl.Id = ti.TodoListId " +
                               "WHERE tl.Id = @Id";

        var todoLists = await _connection.QueryAsync<TodoList, TodoItem, TodoList>(command, (todoList, todoItem) =>
        {
            todoList.Items.Add(todoItem);
            return todoList;
        },
            param: new { Id = id },
            splitOn: "tl.Id"
        );

        return todoLists?.FirstOrDefault();
    }
}