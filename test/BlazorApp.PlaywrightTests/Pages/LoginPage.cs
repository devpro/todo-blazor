using AwesomeAssertions;
using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class LoginPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Log in";

    // locators

    private ILocator EmailInput => Page.GetByLabel("Email");

    private ILocator PasswordInput => Page.GetByLabel("Password");

    private ILocator LoginButton => Page.GetByRole(AriaRole.Button, new() { Name = "Log in" });

    private ILocator ErrorMessage => Page.Locator(".alert-danger");

    // actions

    public async Task EnterCredentialsAsync(string username, string password)
    {
        await EmailInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
    }

    public async Task SubmitAndVerifyFailureAsync(string message)
    {
        await LoginButton.ClickAsync();
        (await ErrorMessage.IsVisibleAsync()).Should().BeTrue();
        (await ErrorMessage.TextContentAsync()).Should().Be(message);
    }

    public async Task<HomePage> SubmitAndVerifySuccessAsync()
    {
        await LoginButton.ClickAsync();
        (await ErrorMessage.IsVisibleAsync()).Should().BeFalse();
        var homePage = new HomePage(Page);
        await homePage.WaitForReadyAsync();
        return homePage;
    }
}
