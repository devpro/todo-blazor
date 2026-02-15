using System.ComponentModel.DataAnnotations;

namespace Devpro.TodoList.BlazorApp.Configuration;

public class DatabaseSettings
{
    [Required(ErrorMessage = "Connection string is required")]
    public string ConnectionString { get; init; } = null!;

    [Required(ErrorMessage = "Database name is required")]
    public string DatabaseName { get; init; } = null!;
}
