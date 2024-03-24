using Microsoft.AspNetCore.Mvc;
using SampleWebApplication.Models;
using SampleWebApplication.Persistence;
using TransactionMiddleware;

namespace SampleWebApplication.Controllers;

[Route("api/todo-list")]
[ApiController]
public class TodoListsController : ControllerBase
{
    [HttpGet("list")]
    public async Task<IEnumerable<TodoList>> Get([FromServices] ITodoListRepository repository)
    {
        return await repository.GetListsAsync();
    }

    [HttpGet("{id}")]
    public async Task<TodoList?> Get([FromServices] ITodoListRepository repository, int id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpGet("{id}/items")]
    public async Task<IEnumerable<TodoItem>> GetItems([FromServices] ITodoItemRepository repository, int id)
    {
        return await repository.GetTodoItemsAsync(id);
    }

    [HttpPost]
    [Transaction]
    public async Task<IActionResult> Post(
        [FromServices] ITodoListRepository todoListRepository,
        [FromServices] ITodoItemRepository todoItemRepository,
        [FromBody] TodoListRequest request
    )
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var todoListId = await todoListRepository.AddAsync(new TodoList { Title = request.Title });

        foreach (var item in request.Items)
        {
            await todoItemRepository.AddTodoItemAsync(new TodoItem { TodoListId = todoListId, Title = item.Title, Note = item.Note });
        }

        return CreatedAtAction(nameof(Get), new { id = todoListId });
    }

    [HttpPost("exception")]
    [Transaction]
    public async Task<IActionResult> PostWithException(
        [FromServices] ITodoListRepository todoListRepository,
        [FromServices] ITodoItemRepository todoItemRepository,
        [FromBody] TodoListRequest request
    )
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var todoListId = await todoListRepository.AddAsync(new TodoList { Title = request.Title });

        foreach (var item in request.Items)
        {
            await todoItemRepository.AddTodoItemAsync(new TodoItem { TodoListId = todoListId, Title = item.Title, Note = item.Note });
        }

        throw new InvalidOperationException("For testing purpose");
    }
}