using Bogus;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;

public abstract class SmokeTestBase(BlazorAppFactory factory) : PageTest(), IClassFixture<BlazorAppFactory>
{
    protected readonly Faker _faker = new();

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        Page.SetDefaultTimeout(10_000);
        Page.SetDefaultNavigationTimeout(20_000);
    }

    protected async Task<HomePage> OpenHomePage()
    {
        var homePage = new HomePage(Page);
        await homePage.NavigateToAsync(factory.ServerAddress);
        return homePage;
    }

    protected async Task TakeScreenshot()
    {
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = $"failure_{DateTime.Now:yyyyMMdd-HHmmss}.png",
            FullPage = true
        });
    }

    protected async Task<HomePage> RegisterLoginUser(string username, string password)
    {
        var homePage = await OpenHomePage();
        var registerPage = await homePage.OpenRegisterAsync();
        await registerPage.EnterCredentialsAsync(username, password, password);
        var registerConfirmPage = await registerPage.SubmitAndVerifySuccessAsync();
        await registerConfirmPage.ClickConfirmationLinkAsync();
        var loginPage = await registerConfirmPage.OpenLoginAsync();
        await loginPage.EnterCredentialsAsync(username, password);
        return await loginPage.SubmitAndVerifySuccessAsync();
    }
}
