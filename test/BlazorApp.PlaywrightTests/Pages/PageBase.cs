using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public abstract class PageBase(IPage page)
{
    // base

    protected IPage Page { get; private set; } = page;

    protected abstract string WebPageTitle { get; }

    // locators

    private ILocator PageHeader => Page.Locator("h1");

    private ILocator LoginLink => Page.GetByRole(AriaRole.Link, new() { Name = "Login" });

    private ILocator RegisterLink => Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true });

    private ILocator LogoutLink => Page.GetByRole(AriaRole.Button, new() { Name = "Logout" });

    private ILocator TodoLink => Page.GetByRole(AriaRole.Link, new() { Name = "Todo" });

    // assertions

    public async Task WaitForReadyAsync()
    {
        await Assertions.Expect(Page).ToHaveTitleAsync(WebPageTitle);
        await Assertions.Expect(PageHeader).ToBeVisibleAsync();
    }

    public async Task VerifyPageHeaderAsync(string header)
    {
        await Assertions.Expect(PageHeader).ToHaveTextAsync(header);
    }

    public async Task VerifyPageHeaderAsync(Regex header)
    {
        await Assertions.Expect(PageHeader).ToHaveTextAsync(header);
    }

    // actions

    public async Task<LoginPage> OpenLoginAsync()
    {
        await LoginLink.ClickAsync();
        var loginPage = new LoginPage(Page);
        await loginPage.WaitForReadyAsync();
        return loginPage;
    }

    public async Task<RegisterPage> OpenRegisterAsync()
    {
        await RegisterLink.ClickAsync();
        var registerPage = new RegisterPage(Page);
        await registerPage.WaitForReadyAsync();
        return registerPage;
    }

    public async Task<HomePage> ClickLogoutAsync()
    {
        await LogoutLink.ClickAsync();
        var homePage = new HomePage(Page);
        await homePage.WaitForReadyAsync();
        return homePage;
    }

    public async Task<TodoPage> OpenTodoAsync()
    {
        await TodoLink.ClickAsync();
        var todoPage = new TodoPage(Page);
        await todoPage.WaitForReadyAsync();
        return todoPage;
    }
}
