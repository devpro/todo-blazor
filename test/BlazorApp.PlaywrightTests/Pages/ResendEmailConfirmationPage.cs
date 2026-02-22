using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class ResendEmailConfirmationPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Resend email confirmation";

    // locators

    private ILocator EmailInput => Page.GetByLabel("Email");

    private ILocator ResendButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Resend" });

    private ILocator AlertMessage => Page.GetByRole(AriaRole.Alert);

    // actions

    public async Task EnterEmailAsync(string email)
    {
        await EmailInput.FillAsync(email);
    }

    public async Task SubmitAsync(string message)
    {
        await ResendButton.ClickAsync();
        await Assertions.Expect(AlertMessage).ToBeVisibleAsync();
        await Assertions.Expect(AlertMessage).ToHaveTextAsync(message);
    }
}
