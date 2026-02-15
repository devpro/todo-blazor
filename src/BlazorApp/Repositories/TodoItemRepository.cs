using Devpro.TodoList.BlazorApp.Models;
using MongoDB.Driver;

namespace Devpro.TodoList.BlazorApp.Repositories;

public class TodoItemRepository : RepositoryBase
{
    private readonly IMongoCollection<TodoItem> _todoItems;

    public TodoItemRepository(IMongoDatabase mongoDatabase, ILogger<TodoItemRepository> logger)
        : base(mongoDatabase, logger)
    {
        _todoItems = GetCollection<TodoItem>();
    }

    protected override string CollectionName => "todo_item";

    public async Task<List<TodoItem>> FindAsync(string userId)
    {
        return await _todoItems.Find(t => t.UserId == userId).ToListAsync();
    }

    public async Task<TodoItem> FindByIdAsync(string userId, string id)
    {
        return await _todoItems.Find(t => t.UserId == userId && t.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(TodoItem todo)
    {
        await _todoItems.InsertOneAsync(todo);
    }

    public async Task UpdateAsync(TodoItem todo)
    {
        await _todoItems.ReplaceOneAsync(t => t.Id == todo.Id, todo);
    }

    public async Task DeleteAsync(string id)
    {
        await _todoItems.DeleteOneAsync(t => t.Id == id);
    }
}
