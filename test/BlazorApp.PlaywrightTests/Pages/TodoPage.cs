using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class TodoPage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Todo";

    // locators

    private ILocator NewInput => Page.GetByTestId("new-todo-input");

    private ILocator AddButton => Page.GetByTestId("add-button");

    private ILocator LoadingSpinner => Page.GetByTestId("loading-message");

    public ILocator TodoRows => Page.GetByTestId("todo-row");

    public ILocator GetTodoRow(string title) => TodoRows.Filter(new() { HasText = title });

    public ILocator GetDoneCheckbox(string title) => GetTodoRow(title).GetByTestId("done-checkbox");

    public ILocator GetEditButton(string title) => GetTodoRow(title).GetByTestId("edit-button");

    public ILocator GetDeleteButton(string title) => GetTodoRow(title).GetByTestId("delete-button");

    public ILocator GetEditInput(string title) => GetTodoRow(title).Locator("input[type='text']");

    public ILocator GetSaveButton(string title) => GetTodoRow(title).GetByTestId("save-button");

    public ILocator GetCancelButton(string title) => GetTodoRow(title).GetByTestId("cancel-button");

    public ILocator DeleteModal => Page.GetByTestId("delete-confirmation-modal");

    public ILocator ConfirmDeleteButton => DeleteModal.GetByRole(AriaRole.Button, new() { Name = "Delete" });

    public ILocator CancelDeleteButton => DeleteModal.GetByRole(AriaRole.Button, new() { Name = "Cancel" });

    // actions

    public async override Task WaitForReadyAsync()
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
            await Assertions.Expect(AddButton).ToBeEnabledAsync(new() { Timeout = 100 });
            await AddButton.ClickAsync();
        }

        await Assertions.Expect(GetTodoRow(todo)).ToBeVisibleAsync();
    }

    public async Task ToggleDoneAsync(string title)
    {
        await GetDoneCheckbox(title).CheckAsync();
    }

    public async Task EditTodoAsync(string title, string newTitle)
    {
        await GetEditButton(title).ClickAsync();
        await GetEditInput(title).FillAsync(newTitle);
        await GetSaveButton(title).ClickAsync();
        await Assertions.Expect(GetTodoRow(newTitle)).ToBeVisibleAsync();
    }

    public async Task DeleteTodoAsync(string title)
    {
        await GetDeleteButton(title).ClickAsync();
        await ConfirmDeleteButton.ClickAsync();
        await Assertions.Expect(GetTodoRow(title)).ToBeHiddenAsync(new() { Timeout = 10000 });
    }

    public async Task CancelEditAsync(string title)
    {
        await GetEditButton(title).ClickAsync();
        await GetCancelButton(title).ClickAsync();
        await Assertions.Expect(GetEditInput(title)).ToBeHiddenAsync();
    }

    public async Task<bool> HasTodoAsync(string title)
    {
        return await GetTodoRow(title).IsVisibleAsync();
    }
}
