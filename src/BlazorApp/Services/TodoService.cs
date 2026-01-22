using BlazorApp.Models;
using MongoDB.Driver;

namespace BlazorApp.Services;

public class TodoService
{
    private readonly IMongoCollection<TodoItem> _todos;
    public TodoService(IMongoClient client)
    {
        var db = client.GetDatabase("TodoDb");
        _todos = db.GetCollection<TodoItem>("Todos");
    }

    public async Task<List<TodoItem>> GetTodosForUserAsync(string userId)
    {
        return await _todos.Find(t => t.UserId == userId).ToListAsync();
    }

    public async Task AddTodoAsync(TodoItem todo)
    {
        await _todos.InsertOneAsync(todo);
    }

    public async Task UpdateTodoAsync(TodoItem todo)
    {
        await _todos.ReplaceOneAsync(t => t.Id == todo.Id, todo);
    }

    public async Task DeleteTodoAsync(string id)
    {
        await _todos.DeleteOneAsync(t => t.Id == id);
    }
}
