using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class UserProfilePage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Profile";

    // locators

    private ILocator ProfileMenuLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Profile" });

    private ILocator UsernameField => Page.GetByLabel("Username");

    private ILocator PhoneNumberField => Page.GetByLabel("Phone number");

    private ILocator SaveButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" });

    private ILocator EmailMenuLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Email" });

    private ILocator EmailField => Page.GetByLabel("Email");

    private ILocator NewEmailField => Page.GetByLabel("New email");

    private ILocator ChangeEmailButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Change email" });

    private ILocator PasswordMenuLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Password" });

    private ILocator OldPasswordField => Page.GetByLabel("Old password");

    private ILocator NewPasswordField => Page.GetByLabel("New password");

    private ILocator ConfirmPasswordField => Page.GetByLabel("Confirm password");

    private ILocator UpdatePasswordButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Update password" });

    private ILocator PersonalDataMenuLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Personal data" });

    private ILocator PersonalDataHeader => Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Personal Data", Exact =  true });

    private ILocator DeleteLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Delete" });

    private ILocator DeletePersonalDataHeader => Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Delete Personal Data" });

    private ILocator PasswordField => Page.GetByLabel("Password");

    private ILocator DeleteConfirmationButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Delete data and close my account" });

    // Manage your account

    // actions

    // TODO

    public async Task OpenPersonalDataAsync()
    {
        await PersonalDataMenuLink.ClickAsync();
        await Assertions.Expect(PersonalDataHeader).ToBeVisibleAsync();
    }

    public async Task<LoginPage> ClickAndConfirmDeletionAsync(string password)
    {
        await DeleteLink.ClickAsync();
        await Assertions.Expect(DeletePersonalDataHeader).ToBeVisibleAsync();
        await PasswordField.FillAsync(password);
        await DeleteConfirmationButton.ClickAsync();
        var loginPage = new LoginPage(Page);
        await loginPage.WaitForReadyAsync();
        return loginPage;
    }
}
