using SampleWebApplication.Models;

namespace SampleWebApplication.Persistence;

public interface ITodoListRepository
{
    Task<int> AddAsync(TodoList todoList);

    Task<IEnumerable<TodoList>> GetListsAsync();

    Task<TodoList?> GetByIdAsync(int id);
}