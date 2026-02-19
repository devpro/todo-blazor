using Devpro.TodoList.BlazorApp.PlaywrightTests.Aspects;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Hosting;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;

public class LoginSmokeTest(BlazorAppFactory factory) : SmokeTestBase(factory)
{
    [Fact]
    [ScreenshotOnFailure]
    public async Task Login_WithUnknownCredentials_Fails()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await OpenHomePageAsync();
        await homePage.VerifyPageHeaderAsync("Hello, world!");

        var loginPage = await homePage.OpenLoginAsync();
        await homePage.VerifyPageHeaderAsync("Log in");
        await loginPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password);
        await loginPage.SubmitAndVerifyFailureAsync("Error: Invalid login attempt.");
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task LoginAfterRegister_WithValidCredentials_Succeeds()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await OpenHomePageAsync();

        var registerPage = await homePage.OpenRegisterAsync();
        await registerPage.VerifyPageHeaderAsync("Register");
        await registerPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password, userInfo.Password);
        var registerConfirmPage = await registerPage.SubmitAndVerifySuccessAsync();
        await registerConfirmPage.VerifyPageHeaderAsync("Register confirmation");
        await registerConfirmPage.ClickConfirmationLinkAsync();

        var loginPage = await registerConfirmPage.OpenLoginAsync();
        await loginPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password);
        homePage = await loginPage.SubmitAndVerifySuccessAsync();

        await homePage.ClickLogoutAsync();
    }
}
