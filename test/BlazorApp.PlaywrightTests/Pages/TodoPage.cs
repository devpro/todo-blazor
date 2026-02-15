using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class TodoPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Todo";

    // locators

    private ILocator NewInput => Page.GetByTestId("new-input");

    private ILocator AddButton => Page.GetByTestId("add-button");

    private ILocator LoadingSpinner => Page.GetByTestId("loading-message");

    private ILocator Rows => Page.Locator("ul.list-group > li");

    private ILocator GetRow(string title) => Rows.Filter(new() { HasText = title });

    private ILocator GetDoneCheckbox(string title) => GetRow(title).GetByRole(AriaRole.Checkbox);

    private ILocator GetEditButton(string title) => GetRow(title).GetByRole(AriaRole.Button, new() { Name = "Edit" });

    private ILocator GetDeleteButton(string title) => GetRow(title).GetByRole(AriaRole.Button, new() { Name = "Delete" });

    private ILocator GetEditInput(string title) => GetRow(title).GetByRole(AriaRole.Textbox);

    private ILocator GetSaveButton(string title) => GetRow(title).GetByRole(AriaRole.Button, new() { Name = "Save" });

    private ILocator GetCancelButton(string title) => GetRow(title).GetByRole(AriaRole.Button, new() { Name = "Cancel" });

    private ILocator DeleteModal => Page.GetByTestId("delete-confirmation-modal");

    private ILocator ConfirmDeleteButton => Page.GetByTestId("confirm-delete");

    public ILocator CancelDeleteButton => Page.GetByTestId("cancel-delete");

    // actions

    public override async Task WaitForReadyAsync()
    {
        await Assertions.Expect(LoadingSpinner).ToBeHiddenAsync();

        await base.WaitForReadyAsync();
    }

    public async Task AddItemAsync(string todo, bool pressEnter = false)
    {
        await Assertions.Expect(NewInput).ToBeEnabledAsync();
        await NewInput.ClickAsync();
        await NewInput.FillAsync(todo);

        await Assertions.Expect(NewInput).ToHaveValueAsync(todo);

        if (pressEnter)
        {
            await NewInput.PressAsync("Enter");
        }
        else
        {
            await Assertions.Expect(AddButton).ToBeEnabledAsync();
            await AddButton.ClickAsync();
        }

        await Assertions.Expect(GetRow(todo)).ToBeVisibleAsync();
    }

    public async Task ToggleDoneAsync(string title)
    {
        await GetDoneCheckbox(title).CheckAsync();
    }

    public async Task EditAsync(string title, string newTitle)
    {
        await Assertions.Expect(GetRow(title)).ToBeVisibleAsync();
        await Assertions.Expect(GetRow(title)).ToHaveCountAsync(1);
        var dataTestId = await GetRow(title).GetAttributeAsync("data-testid") ?? throw new InvalidDataException($"data-testid is undefined for {title}");
        var row = Page.GetByTestId(dataTestId);
        await row.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
        await Assertions.Expect(row.GetByRole(AriaRole.Textbox)).ToBeVisibleAsync();
        await row.GetByRole(AriaRole.Textbox).FillAsync(newTitle);
        await row.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await Assertions.Expect(row).ToContainTextAsync(newTitle);
    }

    public async Task CancelEditAsync(string title)
    {
        var dataTestId = await GetRow(title).GetAttributeAsync("data-testid") ?? throw new InvalidDataException($"data-testid is undefined for {title}");
        var row = Page.GetByTestId(dataTestId);;
        await row.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
        await Assertions.Expect(row.GetByRole(AriaRole.Textbox)).ToBeVisibleAsync();
        await row.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
        await Assertions.Expect(row.GetByRole(AriaRole.Textbox)).ToBeHiddenAsync();
    }

    public async Task DeleteAsync(string title)
    {
        await GetDeleteButton(title).ClickAsync();
        await Assertions.Expect(DeleteModal).ToBeVisibleAsync();
        await ConfirmDeleteButton.ClickAsync();
        await Assertions.Expect(DeleteModal).ToBeHiddenAsync();
        await Assertions.Expect(GetRow(title)).ToBeHiddenAsync();
    }

    public async Task<bool> HasTodoAsync(string title)
    {
        return await GetRow(title).IsVisibleAsync();
    }
}
