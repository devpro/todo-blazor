using Devpro.TodoList.BlazorApp.IntegrationTests.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Devpro.TodoList.BlazorApp.IntegrationTests.Resources;

[Trait("Category", "IntegrationTests")]
public class HealthCheckResourceTest(WebApplicationFactory<Program> factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    [Trait("Mode", "Readonly")]
    public async Task HealthCheckResource_Get_ReturnsOk()
    {
        // Arrange
        var client = CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        await response.CheckResponseAndGetContent(System.Net.HttpStatusCode.OK, "text/plain", "Healthy");
    }
}
