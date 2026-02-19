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

    private ILocator GetRowWithText(string title) => Page.Locator("ul.list-group > li")
        .Filter(new LocatorFilterOptions { HasText = title });

    private ILocator GetEditButton(string testId) => Page.GetByTestId(testId)
        .GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Edit" });

    private ILocator GetTextbox(string testId) => Page.GetByTestId(testId)
        .GetByRole(AriaRole.Textbox);

    private ILocator GetSaveButton(string testId) => Page.GetByTestId(testId)
        .GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Save" });

    private ILocator GetCancelButton(string testId) => Page.GetByTestId(testId)
        .GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Cancel" });

    private ILocator GetCheckbox(string testId) => Page.GetByTestId(testId)
        .GetByRole(AriaRole.Checkbox);

    private ILocator GetDeleteButton(string testId) => Page.GetByTestId(testId).GetByRole(AriaRole.Button,
        new LocatorGetByRoleOptions { Name = "Delete" });

    private ILocator DeleteModal => Page.GetByTestId("delete-confirmation-modal");

    private ILocator ConfirmDeleteButton => DeleteModal.GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Delete" });

    public ILocator CancelDeleteButton => DeleteModal.GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Cancel" });

    // actions

    public override async Task WaitForReadyAsync()
    {
        await Assertions.Expect(LoadingSpinner).ToBeHiddenAsync();
        await base.WaitForReadyAsync();
    }

    public async Task AddItemAsync(string title, bool pressEnter = false)
    {
        await Assertions.Expect(NewInput).ToBeEnabledAsync();
        await NewInput.FillAsync(title);

        if (pressEnter)
        {
            await NewInput.PressAsync("Enter");
        }
        else
        {
            await Assertions.Expect(AddButton).ToBeEnabledAsync();
            await AddButton.ClickAsync();
        }

        await Assertions.Expect(GetRowWithText(title)).ToBeVisibleAsync();
    }

    public async Task ToggleDoneAsync(string title)
    {
        var rowTestId = await GetRowTestId(title);
        await GetCheckbox(rowTestId).CheckAsync();
    }

    public async Task SaveEditAsync(string title, string newTitle)
    {
        var rowTestId = await GetRowTestId(title);
        await GetEditButton(rowTestId).ClickAsync();
        await Assertions.Expect(GetTextbox(rowTestId)).ToBeVisibleAsync();
        await GetTextbox(rowTestId).FillAsync(newTitle);
        await GetSaveButton(rowTestId).ClickAsync();

        await Assertions.Expect(Page.GetByTestId(rowTestId)).ToContainTextAsync(newTitle);
    }

    public async Task CancelEditAsync(string title)
    {
        var rowTestId = await GetRowTestId(title);
        await GetEditButton(rowTestId).ClickAsync();
        await Assertions.Expect(GetTextbox(rowTestId)).ToBeVisibleAsync();
        await GetCancelButton(rowTestId).ClickAsync();
        await Assertions.Expect(GetTextbox(rowTestId)).ToBeHiddenAsync();
    }

    public async Task DeleteAsync(string title)
    {
        var rowTestId = await GetRowTestId(title);
        await GetDeleteButton(rowTestId).ClickAsync();

        await Assertions.Expect(DeleteModal).ToBeVisibleAsync();
        await ConfirmDeleteButton.ClickAsync();
        await Assertions.Expect(DeleteModal).ToBeHiddenAsync();
        await Assertions.Expect(GetRowWithText(title)).ToBeHiddenAsync();
    }

    public async Task<bool> HasTodoAsync(string title)
    {
        return await GetRowWithText(title).IsVisibleAsync();
    }

    private async Task<string> GetRowTestId(string title)
    {
        var row = GetRowWithText(title);
        await Assertions.Expect(row).ToBeVisibleAsync();
        return await row.GetAttributeAsync("data-testid") ??
               throw new InvalidDataException($"data test id is undefined for {title}");
    }
}
