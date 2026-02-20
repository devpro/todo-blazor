using Devpro.TodoList.BlazorApp.PlaywrightTests.Aspects;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Hosting;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;

public class UserProfileSmokeTest(BlazorAppFactory factory) : SmokeTestBase(factory)
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
        await profilePage.OpenProfileSectionAsync();
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
}
