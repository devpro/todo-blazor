using Bogus;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests;

public class WalkthroughTest(BlazorAppFactory factory) : PageTest(), IClassFixture<BlazorAppFactory>
{
    private readonly Faker _faker = new();

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        Page.SetDefaultTimeout(10_000);
        Page.SetDefaultNavigationTimeout(20_000);
    }

    [Fact]
    public async Task Homepage_LoadsAndShowsTodoTitle()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        try
        {
            var homePage = new HomePage(Page);
            await homePage.NavigateToAsync(factory.ServerAddress);
            await homePage.VerifyPageHeaderAsync("Hello, world!");

            var loginPage = await homePage.OpenLoginAsync();
            await loginPage.VerifyPageHeaderAsync("Log in");
            await loginPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password);
            await loginPage.SubmitAndVerifyFailureAsync("Error: Invalid login attempt.");

            var registerPage = await loginPage.OpenRegisterAsync();
            await registerPage.VerifyPageHeaderAsync("Register");
            await registerPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password, userInfo.Password);
            var registerConfirmPage = await registerPage.SubmitAndVerifySuccessAsync();
            await registerConfirmPage.VerifyPageHeaderAsync("Register confirmation");
            await registerConfirmPage.ClickConfirmationLinkAsync();

            loginPage = await registerConfirmPage.OpenLoginAsync();
            await loginPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password);
            homePage = await loginPage.SubmitAndVerifySuccessAsync();

            var todoPage = await homePage.OpenTodoAsync();
            var task1 = _faker.Company.CatchPhrase();
            await todoPage.AddItemAsync(task1);
            await todoPage.ToggleDoneAsync(task1);
            await todoPage.EditAsync(task1, task1 + " oups");
            await todoPage.CancelEditAsync(task1 + " oups");
            await todoPage.DeleteAsync(task1 + " oups");

            await homePage.ClickLogoutFromAuthorizedAsync();
        }
        catch (Exception)
        {
            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = $"failure_{DateTime.Now:yyyyMMdd-HHmmss}.png",
                FullPage = true
            });
            throw;
        }
    }
}
