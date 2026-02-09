using AwesomeAssertions;
using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class HomePage(IPage page) : PageBase(page)
{
    private ILocator LoginLink => Page.GetByRole(AriaRole.Link, new() { Name = "Login" });

    public async Task NavigateAsync(string baseAdress)
    {
        await Page.GotoAsync(baseAdress);
        await Assertions.Expect(Page).ToHaveTitleAsync("Home");
        await Assertions.Expect(Page.Locator("h1")).ToHaveTextAsync("Hello, world!");
    }

    public async Task<LoginPage> OpenLoginAsync()
    {
        await LoginLink.ClickAsync();
        return new LoginPage(Page);
    }
}
