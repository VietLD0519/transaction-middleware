using System.ComponentModel.DataAnnotations;

namespace SampleWebApplication.Models;

public readonly struct TodoListRequest([Required][MaxLength(128)] string title)
{
    public TodoList ToTodoListModel => new() { Title = title };
};