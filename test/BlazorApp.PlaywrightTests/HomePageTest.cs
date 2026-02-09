using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Microsoft.Playwright.Xunit.v3;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests;

public class HomePageTest(RealKestrelFactory factory) : PageTest, IClassFixture<RealKestrelFactory>
{
    private readonly RealKestrelFactory _factory = factory;

    [Fact]
    public async Task Homepage_LoadsAndShowsTodoTitle()
    {
        var homePage = new HomePage(Page);
        await homePage.NavigateAsync(_factory.ServerAddress);
        var loginPage = await homePage.OpenLoginAsync();
        await loginPage.EnterCredentialsAsync("toto@titi.com", "tutu");
        await loginPage.SubmitLoginAsync();
        await loginPage.AssertErrorVisibleAsync();

        //await Page.ClickAsync("button#add-todo");
        //await Expect(Page.Locator("input#new-todo")).ToBeVisibleAsync();
    }
}
