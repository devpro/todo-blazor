using Withywoods.Configuration;

namespace Devpro.TodoList.BlazorApp.Configuration;

public class DatabaseSettings(IConfiguration configuration)
{
    public string ConnectionString { get; } = configuration.TryGetSection<string>("DatabaseSettings:ConnectionString");

    public string DatabaseName { get; } = configuration.TryGetSection<string>("DatabaseSettings:DatabaseName");
}
