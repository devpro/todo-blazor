using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class RegisterConfirmPage(IPage page) : PageBase(page)
{
    private ILocator ConfirmationLink => Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" });

    public async Task ClickConfirmationLinkAsync() => await ConfirmationLink.ClickAsync();
}
