using Devpro.TodoList.BlazorApp.PlaywrightTests.Testing;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;

public class LoginSmokeTest(BlazorAppFactory factory) : SmokeTestBase(factory)
{
    [Fact]
    public async Task Login_WithUnknownCredentials_IsRejected()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        try
        {
            var homePage = await OpenHomePage();
            await homePage.VerifyPageHeaderAsync("Hello, world!");

            var loginPage = await homePage.OpenLoginAsync();
            await homePage.VerifyPageHeaderAsync("Log in");
            await loginPage.EnterCredentialsAsync(userInfo.Email, userInfo.Password);
            await loginPage.SubmitAndVerifyFailureAsync("Error: Invalid login attempt.");
        }
        catch
        {
            await TakeScreenshot();
            throw;
        }
    }

    [Fact]
    public async Task Register_WithValidCredentials_Succeeds()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        try
        {
            var homePage = await OpenHomePage();

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
        catch
        {
            await TakeScreenshot();
            throw;
        }
    }
}
