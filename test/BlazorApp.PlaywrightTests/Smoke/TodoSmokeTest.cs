using AwesomeAssertions;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Hosting;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;

public class TodoSmokeTest(BlazorAppFactory factory) : SmokeTestBase(factory)
{
    [Fact]
    public async Task ManageTodo_WithCreateUpdateDelete_Succeeds()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await RegisterLoginUserAsync(userInfo.Email, userInfo.Password);

        var todoPage = await homePage.OpenTodoAsync();
        var task1 = _faker.Company.CatchPhrase();
        await todoPage.AddItemAsync(task1);
        await todoPage.ToggleDoneAsync(task1);
        await todoPage.SaveEditAsync(task1, task1 + " bis");
        await todoPage.CancelEditAsync(task1 + " bis");
        (await todoPage.HasTodoAsync(task1 + " bis")).Should().BeTrue();
        await todoPage.DeleteAsync(task1 + " bis");

        await homePage.ClickLogoutFromAuthorizedAsync();
    }
}
