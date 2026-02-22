using Microsoft.Playwright;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;

public class UserProfilePage(IPage page) : PageBase(page)
{
    // base

    protected override string WebPageTitle => "Profile";

    // locators

    private ILocator AlertMessage => Page.GetByRole(AriaRole.Alert);

    private ILocator ProfileSectionLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Profile" });

    private ILocator ProfileSectionHeader => Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Profile", Exact = true });

    private ILocator UsernameField => Page.GetByLabel("Username");

    private ILocator PhoneNumberField => Page.GetByLabel("Phone number");

    private ILocator SaveButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" });

    private ILocator EmailSectionLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Email" });

    private ILocator EmailSectionHeader => Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Manage email" });

    private ILocator EmailField => Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Email", Exact = true });

    private ILocator NewEmailField => Page.GetByLabel("New email");

    private ILocator ChangeEmailButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Change email" });

    private ILocator PasswordSectionLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Password" });

    private ILocator PasswordSectionHeader => Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Change password" });

    private ILocator OldPasswordField => Page.GetByLabel("Old password");

    private ILocator NewPasswordField => Page.GetByLabel("New password");

    private ILocator ConfirmPasswordField => Page.GetByLabel("Confirm password");

    private ILocator UpdatePasswordButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Update password" });

    private ILocator PersonalDataSectionLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Personal data" });

    private ILocator PersonalDataSectionHeader => Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Personal Data", Exact = true });

    private ILocator DeleteLink => Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Delete" });

    private ILocator DeletePersonalDataHeader => Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Delete Personal Data" });

    private ILocator PasswordField => Page.GetByLabel("Password");

    private ILocator DeleteConfirmationButton => Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Delete data and close my account" });

    // actions

    public override async Task WaitForReadyAsync()
    {
        await base.WaitForReadyAsync();
        await Assertions.Expect(ProfileSectionHeader).ToBeVisibleAsync();
    }

    public async Task UpdateProfileAsync(string username, string phoneNumber)
    {
        await Assertions.Expect(UsernameField).ToHaveValueAsync(username);
        await Assertions.Expect(PhoneNumberField).ToBeEnabledAsync();
        await PhoneNumberField.ClickAsync();
        await PhoneNumberField.FillAsync(phoneNumber);
        await SaveButton.ClickAsync();
        await Assertions.Expect(AlertMessage).ToBeVisibleAsync();
        await Assertions.Expect(AlertMessage).ToContainTextAsync("Your profile has been updated");
    }

    public async Task OpenEmailSectionAsync()
    {
        await EmailSectionLink.ClickAsync();
        await Assertions.Expect(EmailSectionHeader).ToBeVisibleAsync();
    }

    public async Task UpdateEmailAsync(string oldEmail, string newEmail)
    {
        await Assertions.Expect(EmailField).ToHaveValueAsync(oldEmail);
        await Assertions.Expect(NewEmailField).ToBeEnabledAsync();
        await NewEmailField.FillAsync(newEmail);
        await ChangeEmailButton.ClickAsync();
        await Assertions.Expect(AlertMessage).ToBeVisibleAsync();
        await Assertions.Expect(AlertMessage).ToContainTextAsync(oldEmail != newEmail
            ? "Confirmation link to change email sent. Please check your email."
            : "Your email is unchanged.");
    }

    public async Task OpenPasswordSectionAsync()
    {
        await PasswordSectionLink.ClickAsync();
        await Assertions.Expect(PasswordSectionHeader).ToBeVisibleAsync();
    }

    public async Task UpdatePasswordAsync(string oldPassword, string newPassword)
    {
        await OldPasswordField.FillAsync(oldPassword);
        await NewPasswordField.FillAsync(newPassword);
        await ConfirmPasswordField.FillAsync(newPassword);
        await UpdatePasswordButton.ClickAsync();
        await Assertions.Expect(AlertMessage).ToBeVisibleAsync();
        await Assertions.Expect(AlertMessage).ToContainTextAsync("Your password has been changed");
    }

    public async Task OpenPersonalDataSectionAsync()
    {
        await PersonalDataSectionLink.ClickAsync();
        await Assertions.Expect(PersonalDataSectionHeader).ToBeVisibleAsync();
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
