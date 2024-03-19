using System.ComponentModel.DataAnnotations;

namespace SampleWebApplication.Models;

public record struct TodoListRequest(
    [Required][MaxLength(128)] string Title,
    [Required] IEnumerable<TodoItemRequest> Items);
//{
//    public TodoList ToTodoListModel => new()
//    {
//        Title = Title,
//        Items = Items.Select(i => new TodoItem { Title = i.Title, Note = i.Note }).ToList()
//    };
//};

public record struct TodoItemRequest(
    [Required][MaxLength(128)] string Title,
    [Required][MaxLength(128)] string Note
);