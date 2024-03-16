using SampleWebApplication.Models;

namespace SampleWebApplication.Persistence;

public interface ITodoListRepository
{
    Task<int> AddTodoListAsync(TodoList todoList);

    Task<IEnumerable<TodoList>> GetTodoListsAsync();
}