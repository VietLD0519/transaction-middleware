using Microsoft.EntityFrameworkCore;
using SampleWebApplication.Models;

namespace SampleWebApplication.Persistence.EntityFramework;

public class TodoItemRepository(TodoDbContext context) : ITodoItemRepository
{
    public async Task<int> AddTodoItemAsync(TodoItem todoItem)
    {
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        return todoItem.Id;
    }

    public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(int todoListId)
    {
        return await context.TodoItems.Where(t => t.TodoListId == todoListId).ToListAsync();
    }
}