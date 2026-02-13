using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class TodoPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Todo";

    // locators

    private ILocator NewInput => Page.GetByRole(AriaRole.Textbox, new() { Name = "Something todo" });

    private ILocator AddButton => Page.GetByRole(AriaRole.Button, new () { Name = "Add todo" });

    // actions

    public async Task AddItem(string todo)
    {
        await NewInput.ClickAsync();
        await NewInput.FillAsync(todo);
        await NewInput.ClickAsync();
        await NewInput.PressAsync("Enter");
        await AddButton.ClickAsync();
    }
}
