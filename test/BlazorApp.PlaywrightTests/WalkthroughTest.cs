using Bogus;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests;

public class WalkthroughTest(RealKestrelFactory factory) : PageTest(), IClassFixture<RealKestrelFactory>
{
    private readonly RealKestrelFactory _factory = factory;
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
            await homePage.NavigateAsync(_factory.ServerAddress);
            await homePage.AssertTitleAndMainHeaderAsync("Home", "Hello, world!");

            var loginPage = await homePage.OpenLoginAsync();
            await loginPage.AssertTitleAndMainHeaderAsync("Log in", "Log in");
            await loginPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password);
            await loginPage.SubmitLoginAsync();
            await loginPage.AssertErrorVisibleAsync();
            await loginPage.AssertErrorTextAsync("Error: Invalid login attempt.");

            var registerPage = await loginPage.OpenRegisterAsync();
            await registerPage.AssertTitleAndMainHeaderAsync("Register", "Register");
            await registerPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password, userInfo.Password);
            var registerConfirmPage = await registerPage.SubmitRegisterAndCheckSuccessAsync();
            await registerConfirmPage.AssertTitleAndMainHeaderAsync("Register confirmation", "Register confirmation");
            await registerConfirmPage.ClickConfirmationLinkAsync();

            loginPage = await registerConfirmPage.OpenLoginAsync();
            await loginPage.AssertTitleAndMainHeaderAsync("Log in", "Log in");
            await loginPage.SubmitLoginAsync();
            await loginPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password);
            homePage = await loginPage.SubmitLoginAndCheckSuccessAsync();
            await homePage.AssertTitleAndMainHeaderAsync("Home", "Hello, world!");
            await homePage.ClickLogoutAsync();
            await homePage.AssertTitleAndMainHeaderAsync("Home", "Hello, world!");
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
