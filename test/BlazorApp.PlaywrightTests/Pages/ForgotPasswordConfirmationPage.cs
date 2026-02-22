using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class ForgotPasswordConfirmationPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Forgot password confirmation";

    // locators

    private ILocator AlertMessage => Page.GetByRole(AriaRole.Alert);

    // actions

    public async Task CheckStatusAsync(string message)
    {
        await Assertions.Expect(AlertMessage).ToBeVisibleAsync();
        await Assertions.Expect(AlertMessage).ToHaveTextAsync(message);
    }
}
