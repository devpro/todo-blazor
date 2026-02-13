using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Support;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public class AuthenticationSteps(ScenarioContext scenarioContext)
    : StepBase(scenarioContext)
{
    [When("I open the login page")]
    public async Task WhenOpenLogin()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var loginPage = await currentPage.OpenLoginAsync();
        await loginPage.VerifyPageHeaderAsync("Log in");
        SetCurrentPage(loginPage);
    }

    [When("I enter invalid credentials")]
    public async Task WhenEnterInvalidCredentials()
    {
        var loginPage = GetCurrentPage<LoginPage>();
        await loginPage.EnterCredentialsAsync(GenerateEmail(), GeneratePassword());
        
    }

    [Then("I see login error {string}")]
    public async Task ThenISeeLoginError(string message)
    {
        var loginPage = GetCurrentPage<LoginPage>();
        await loginPage.SubmitAndVerifyFailureAsync(message);
    }

    [When("I login with the registered credentials")]
    public async Task WhenLoginRegistered()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var loginPage = await currentPage.OpenLoginAsync();
        await loginPage.EnterCredentialsAsync((string)_scenarioContext[ScenarioContextKeys.RegisteredEmail],
            (string)_scenarioContext[ScenarioContextKeys.RegisteredPassword]);
        SetCurrentPage(loginPage);
    }

    [Then("I am on the home page after successful login")]
    public async Task ThenIAmOnTheHomePageAfterSuccessfulLogin()
    {
        var loginPage = GetCurrentPage<LoginPage>();
        var homePage = await loginPage.SubmitAndVerifySuccessAsync();
        SetCurrentPage(homePage);
    }


    [When("I click logout")]
    public async Task WhenClickLogout()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var homePage = await currentPage.ClickLogoutAsync();
        SetCurrentPage(homePage);
    }

    [Then("I am back on the home page \\(logged out state)")]
    public void ThenIAmBackOnTheHomePageLoggedOutState()
    {
        var homePage = GetCurrentPage<HomePage>();
        SetCurrentPage(homePage);
    }
}
