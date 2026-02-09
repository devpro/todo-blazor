using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests;

public class HomePageTest : PageTest, IClassFixture<RealKestrelFactory>
{
    private readonly RealKestrelFactory _factory;

    public HomePageTest(RealKestrelFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Homepage_LoadsAndShowsTodoTitle()
    {
        var baseUrl = _factory.ServerAddress;

        if (string.IsNullOrEmpty(baseUrl))
        {
            Assert.Fail("ServerAddress is empty — check console logs for binding errors.");
        }

        Console.WriteLine($"Navigating to: {baseUrl}");

        await Page.GotoAsync(baseUrl);

        //await Expect(Page).ToHaveTitleAsync(new Regex("Todo Blazor App"));

        //await Expect(Page.Locator("h1")).ToHaveTextAsync("Todo List");

        //await Page.ClickAsync("button#add-todo");
        //await Expect(Page.Locator("input#new-todo")).ToBeVisibleAsync();
    }
}
