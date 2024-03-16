using Microsoft.AspNetCore.Mvc;
using SampleWebApplication.Models;
using SampleWebApplication.Persistence;
using TransactionMiddleware;

namespace SampleWebApplication.Controllers;

[ApiController]
[Route("todo")]
public class TodoListsController : ControllerBase
{
    [HttpGet("list")]
    public async Task<IEnumerable<object>> Get([FromServices] ITodoListRepository repository)
    {
        return await repository.GetTodoListsAsync();
    }

    [HttpGet("{id}")]
    public async Task<IEnumerable<object>> Get(int id, [FromServices] ITodoListRepository repository)
    {
        return await repository.GetTodoListsAsync();
    }

    [Transaction]
    [HttpPost("todo-list")]
    public async Task<IActionResult> Post(
        [FromBody] TodoListRequest request,
        [FromServices] ITodoListRepository todoListRepository)
    {
        var id = await todoListRepository.AddTodoListAsync(request.ToTodoListModel);

        return Ok();
    }
}