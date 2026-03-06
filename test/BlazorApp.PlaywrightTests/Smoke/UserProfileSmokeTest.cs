using System.Threading.Tasks;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Aspects;
using Withywoods.AspNetCore.Mvc.Testing;
using Xunit;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;

public class UserProfileSmokeTest(KestrelWebAppFactory<Program> factory)
    : SmokeTestBase(factory)
{
    [Fact]
    [ScreenshotOnFailure]
    public async Task UpdateProfile_Succeeds()
    {
        var userInfo = new
        {
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password(8) + "aA9!",
            PhoneNumber = _faker.Phone.PhoneNumber()
        };

        var homePage = await RegisterLoginUserAsync(userInfo.Email, userInfo.Password);

        var profilePage = await homePage.OpenUserProfileAsync();
        await profilePage.UpdateProfileAsync(userInfo.Email, userInfo.PhoneNumber);

        await DeleteUserAsync(profilePage, userInfo.Password);
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task UpdateEmail_Succeeds()
    {
        var userInfo = new
        {
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password(8) + "aA9!"
        };

        var homePage = await RegisterLoginUserAsync(userInfo.Email, userInfo.Password);

        var profilePage = await homePage.OpenUserProfileAsync();
        await profilePage.OpenEmailSectionAsync();
        await profilePage.UpdateEmailAsync(userInfo.Email, userInfo.Email);
        await profilePage.UpdateEmailAsync(userInfo.Email, userInfo.Email + "o");

        await DeleteUserAsync(profilePage, userInfo.Password);
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task UpdatePassword_Succeeds()
    {
        var userInfo = new
        {
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password(8) + "aA9!"
        };

        var homePage = await RegisterLoginUserAsync(userInfo.Email, userInfo.Password);

        var profilePage = await homePage.OpenUserProfileAsync();
        await profilePage.OpenPasswordSectionAsync();
        await profilePage.UpdatePasswordAsync(userInfo.Password, userInfo.Password + "o");

        await DeleteUserAsync(profilePage, userInfo.Password + "o");
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task DeleteProfile_Succeeds()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await RegisterLoginUserAsync(userInfo.Email, userInfo.Password);

        var profilePage = await homePage.OpenUserProfileAsync();
        await profilePage.OpenPersonalDataSectionAsync();
        await profilePage.ClickAndConfirmDeletionAsync(userInfo.Password);
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task ResendEmailConfirmation_Fails()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await RegisterUserAsync(userInfo.Email, userInfo.Password);

        var loginPage = await homePage.OpenLoginAsync();
        var resendEmailConfirmationPage = await loginPage.OpenResendEmailConfirmationPage();
        await resendEmailConfirmationPage.SubmitAsync("The Email field is required.");
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task ResendEmailConfirmation_Succeeds()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await RegisterUserAsync(userInfo.Email, userInfo.Password);

        var loginPage = await homePage.OpenLoginAsync();
        var resendEmailConfirmationPage = await loginPage.OpenResendEmailConfirmationPage();
        await resendEmailConfirmationPage.EnterEmailAsync(userInfo.Email);
        await resendEmailConfirmationPage.SubmitAsync("Verification email sent. Please check your email.");
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task ForgotPassword_Fails()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await RegisterUserAsync(userInfo.Email, userInfo.Password);

        var loginPage = await homePage.OpenLoginAsync();
        var forgotPasswordPage = await loginPage.OpenForgotPasswordPage();
        await forgotPasswordPage.SubmitErrorAsync("The Email field is required.");
    }

    [Fact]
    [ScreenshotOnFailure]
    public async Task ForgotPassword_Succeeds()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await RegisterUserAsync(userInfo.Email, userInfo.Password);

        var loginPage = await homePage.OpenLoginAsync();
        var forgotPasswordPage = await loginPage.OpenForgotPasswordPage();
        await forgotPasswordPage.EnterEmailAsync(userInfo.Email);
        var forgotPasswordConfirmationPage = await forgotPasswordPage.SubmitSuccessAsync();
        await forgotPasswordConfirmationPage.CheckStatusAsync("Please check your email to reset your password.");
    }
}
