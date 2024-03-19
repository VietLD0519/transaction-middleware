using Microsoft.EntityFrameworkCore;
using SampleWebApplication.Models;

namespace SampleWebApplication.Persistence.EntityFramework;

public class TodoListRepository(TodoDbContext context) : ITodoListRepository
{
    public async Task<int> AddAsync(TodoList todoList)
    {
        context.Add(todoList);
        await context.SaveChangesAsync();

        return todoList.Id;
    }

    public async Task<IEnumerable<TodoList>> GetListsAsync()
    {
        return await context.TodoLists.AsNoTracking().ToListAsync();
    }

    public async Task<TodoList?> GetByIdAsync(int id)
    {
        return await context.TodoLists
            .Include(tl => tl.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(tl => tl.Id == id);
    }
}