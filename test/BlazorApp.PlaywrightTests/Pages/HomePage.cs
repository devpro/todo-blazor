using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class HomePage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Home";

    // actions

    public async Task NavigateToAsync(string baseAdress)
    {
        await Page.GotoAsync(baseAdress);
        await WaitForReadyAsync();
    }
}
