using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class RegisterPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Register";

    // locators

    private ILocator EmailInput => Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Email" });

    private ILocator PasswordInput => Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Password", Exact = true });

    private ILocator PasswordConfirmationInput => Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Confirm Password" });

    private ILocator RegisterButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Register" });

    // actions

    public async Task EnterCredentialsAsync(string username, string password, string passwordConfirmation)
    {
        await EmailInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await PasswordConfirmationInput.FillAsync(passwordConfirmation);
    }

    // TODO: implement SubmitAndVerifyFailureAsync

    public async Task<RegisterConfirmPage> SubmitAndVerifySuccessAsync()
    {
        await RegisterButton.ClickAsync();
        var registerConfirmPage = new RegisterConfirmPage(Page);
        await registerConfirmPage.WaitForReadyAsync();
        return registerConfirmPage;
    }
}
