using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class RegisterConfirmPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Register confirmation";

    // locators

    private ILocator ConfirmationLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Click here to confirm your" });

    // actions

    public async Task ClickConfirmationLinkAsync() => await ConfirmationLink.ClickAsync();
}
