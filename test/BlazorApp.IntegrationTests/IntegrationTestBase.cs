using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Devpro.TodoList.BlazorApp.IntegrationTests;

/// <summary>
/// Ref. https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests
/// </summary>
public abstract class IntegrationTestBase(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    protected HttpClient CreateClient(Action<IWebHostBuilder>? builderConfiguration = null)
    {
        return (builderConfiguration == null) ? factory.CreateClient()
            : factory.WithWebHostBuilder(builderConfiguration).CreateClient();
    }
}
