using Microsoft.EntityFrameworkCore;
using SampleWebApplication.Models;

namespace SampleWebApplication.Persistence.EntityFramework;

public class TodoListRepository(TodoDbContext context) : ITodoListRepository
{
    public async Task<int> AddTodoListAsync(TodoList todoList)
    {
        context.Add(todoList);
        await context.SaveChangesAsync();

        return todoList.Id;
    }

    public async Task<IEnumerable<TodoList>> GetTodoListsAsync()
    {
        return await context.TodoLists.ToListAsync();
    }
}