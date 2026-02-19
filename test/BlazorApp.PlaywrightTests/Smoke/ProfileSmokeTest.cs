using Devpro.TodoList.BlazorApp.PlaywrightTests.Aspects;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Hosting;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;

public class ProfileSmokeTest(BlazorAppFactory factory) : SmokeTestBase(factory)
{
    [Fact]
    [ScreenshotOnFailure]
    public async Task DeleteProfile_Succeeds()
    {
        var userInfo = new { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8) + "aA9!" };

        var homePage = await RegisterLoginUserAsync(userInfo.Email, userInfo.Password);

        var profilePage = await homePage.OpenUserProfileAsync();
        await profilePage.OpenPersonalDataAsync();
        await profilePage.ClickAndConfirmDeletionAsync(userInfo.Password);
    }
}
