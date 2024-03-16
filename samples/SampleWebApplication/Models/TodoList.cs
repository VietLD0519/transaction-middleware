﻿namespace SampleWebApplication.Models;

public class TodoList
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public IList<TodoItem> Items { get; set; } = new List<TodoItem>();
}