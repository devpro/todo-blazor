using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class ForgotPasswordPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Forgot your password?";

    // locators

    private ILocator EmailInput => Page.GetByLabel("Email");

    private ILocator ResetPasswordButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Reset password" });

    private ILocator AlertMessage => Page.GetByRole(AriaRole.Alert);

    // actions

    public async Task EnterEmailAsync(string email)
    {
        await EmailInput.FillAsync(email);
    }

    public async Task SubmitErrorAsync(string message)
    {
        await ResetPasswordButton.ClickAsync();
        await Assertions.Expect(AlertMessage).ToBeVisibleAsync();
        await Assertions.Expect(AlertMessage).ToHaveTextAsync(message);
    }

    public async Task<ForgotPasswordConfirmationPage> SubmitSuccessAsync()
    {
        await ResetPasswordButton.ClickAsync();
        var forgotPasswordConfirmationPage = new ForgotPasswordConfirmationPage(Page);
        await forgotPasswordConfirmationPage.WaitForReadyAsync();
        return forgotPasswordConfirmationPage;
    }
}
