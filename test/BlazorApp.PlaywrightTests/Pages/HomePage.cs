using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class HomePage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Home";

    // actions

    public async Task NavigateToAsync(string baseAddress)
    {
        await Page.GotoAsync(baseAddress);
        await WaitForReadyAsync();
    }
}
