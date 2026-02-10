using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class RegisterPage(IPage page) : PageBase(page)
{
    private ILocator EmailInput => Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" });

    private ILocator PasswordInput => Page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true });

    private ILocator PasswordConfirmationInput => Page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" });

    private ILocator RegisterButton => Page.GetByRole(AriaRole.Button, new() { Name = "Register" });

    public async Task EnterCredentialsAsync(string username, string password, string passwordConfirmation)
    {
        await EmailInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await PasswordConfirmationInput.FillAsync(passwordConfirmation);
    }

    public async Task SubmitRegisterAsync() => await RegisterButton.ClickAsync();

    public async Task<RegisterConfirmPage> SubmitRegisterAndCheckSuccessAsync()
    {
        await SubmitRegisterAsync();
        // TODO
        return new RegisterConfirmPage(Page);
    }
}
