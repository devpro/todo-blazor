using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class PageBase(IPage page)
{
    protected IPage Page { get; private set; } = page;

    private ILocator MainHeader => Page.Locator("h1");

    private ILocator LoginLink => Page.GetByRole(AriaRole.Link, new() { Name = "Login" });

    private ILocator RegisterLink => Page.GetByRole(AriaRole.Link, new() { Name = "Register" });

    private ILocator LogoutLink => Page.GetByRole(AriaRole.Button, new() { Name = "Logout" });

    public async Task AssertTitleAndMainHeaderAsync(string title, string header)
    {
        await Assertions.Expect(Page).ToHaveTitleAsync(title);
        await Assertions.Expect(MainHeader).ToHaveTextAsync(header);
    }

    public async Task NavigateAsync(string baseAdress)
    {
        await Page.GotoAsync(baseAdress);
    }

    public async Task<LoginPage> OpenLoginAsync()
    {
        await LoginLink.ClickAsync();
        return new LoginPage(Page);
    }

    public async Task<RegisterPage> OpenRegisterAsync()
    {
        await RegisterLink.ClickAsync();
        return new RegisterPage(Page);
    }

    public async Task<HomePage> ClickLogoutAsync()
    {
        await LogoutLink.ClickAsync();
        return new HomePage(Page);
    }
}
