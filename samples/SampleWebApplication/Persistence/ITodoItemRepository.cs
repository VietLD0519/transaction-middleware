using SampleWebApplication.Models;

namespace SampleWebApplication.Persistence;

public interface ITodoItemRepository
{
    Task<int> AddTodoItemAsync(TodoItem todoItem);

    Task<IEnumerable<TodoItem>> GetTodoItemsAsync(int todoListId);
}