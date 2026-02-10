using AwesomeAssertions;
using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class LoginPage(IPage page) : PageBase(page)
{
    private ILocator EmailInput => Page.GetByLabel("Email");

    private ILocator PasswordInput => Page.GetByLabel("Password");

    private ILocator LoginButton => Page.GetByRole(AriaRole.Button, new() { Name = "Log in" });

    private ILocator ErrorMessage => Page.Locator(".alert-danger");

    public async Task EnterCredentialsAsync(string username, string password)
    {
        await EmailInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
    }

    public async Task SubmitLoginAsync() => await LoginButton.ClickAsync();

    public async Task<HomePage> SubmitLoginAndCheckSuccessAsync()
    {
        await SubmitLoginAsync();
        (await ErrorMessage.IsVisibleAsync()).Should().BeFalse();
        return new HomePage(Page);
    }

    public async Task AssertErrorVisibleAsync() => (await ErrorMessage.IsVisibleAsync()).Should().BeTrue();

    public async Task AssertErrorTextAsync(string message) => (await ErrorMessage.TextContentAsync()).Should().Be(message);
}
