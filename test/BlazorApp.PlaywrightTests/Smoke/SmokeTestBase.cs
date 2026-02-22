using Bogus;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Hosting;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
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

    public async Task TakeScreenshotAsync()
    {
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = $"failure_{DateTime.Now:yyyyMMdd-HHmmss}.png",
            FullPage = true
        });
    }

    protected async Task<HomePage> OpenHomePageAsync()
    {
        var homePage = new HomePage(Page);
        await homePage.NavigateToAsync(factory.ServerAddress);
        return homePage;
    }

    protected async Task<HomePage> RegisterLoginUserAsync(string username, string password)
    {
        var homePage = await OpenHomePageAsync();
        var registerPage = await homePage.OpenRegisterAsync();
        await registerPage.EnterCredentialsAsync(username, password, password);
        var registerConfirmPage = await registerPage.SubmitAndVerifySuccessAsync();
        await registerConfirmPage.ClickConfirmationLinkAsync();
        var loginPage = await registerConfirmPage.OpenLoginAsync();
        await loginPage.EnterCredentialsAsync(username, password);
        return await loginPage.SubmitAndVerifySuccessAsync();
    }

    protected static async Task<LoginPage> DeleteUserAsync(PageBase page, string password)
    {
        var profilePage = await page.OpenUserProfileAsync();
        await profilePage.OpenPersonalDataSectionAsync();
        return await profilePage.ClickAndConfirmDeletionAsync(password);
    }
}
