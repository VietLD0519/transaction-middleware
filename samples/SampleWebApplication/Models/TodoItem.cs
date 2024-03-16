namespace SampleWebApplication.Models;

public class TodoItem
{
    public int Id { get; set; }

    public int TodoListId { get; set; }

    public string Title { get; set; } = null!;

    public string? Note { get; set; } = null;
}