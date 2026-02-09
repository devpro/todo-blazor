using AwesomeAssertions;
using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class LoginPage(IPage page) : PageBase(page)
{
    private ILocator UsernameInput => Page.GetByLabel("Email");

    private ILocator PasswordInput => Page.GetByLabel("Password");

    private ILocator LoginButton => Page.GetByRole(AriaRole.Button, new() { Name = "Log in" });

    private ILocator ErrorMessage => Page.Locator(".alert-danger");

    public async Task EnterCredentialsAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
    }

    public async Task SubmitLoginAsync() => await LoginButton.ClickAsync();

    public async Task AssertErrorVisibleAsync() => (await ErrorMessage.IsVisibleAsync()).Should().BeTrue();
}
