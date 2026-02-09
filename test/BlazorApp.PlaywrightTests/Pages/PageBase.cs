using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class PageBase(IPage page)
{
    protected IPage Page { get; private set; } = page;
}
