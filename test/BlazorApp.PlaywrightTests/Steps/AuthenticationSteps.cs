using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Support;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public class AuthenticationSteps(ScenarioContext scenarioContext)
    : StepBase(scenarioContext)
{
    [When("I enter invalid credentials")]
    public async Task EnterInvalidCredentials()
    {
        var loginPage = GetCurrentPage<LoginPage>();
        await loginPage.EnterCredentialsAsync(GenerateEmail(), GeneratePassword());
        
    }

    [Then("I see login error {string}")]
    public async Task SeeLoginError(string message)
    {
        var loginPage = GetCurrentPage<LoginPage>();
        await loginPage.SubmitAndVerifyFailureAsync(message);
    }

    [When("I login with the registered credentials")]
    public async Task LoginRegistered()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var loginPage = await currentPage.OpenLoginAsync();
        await loginPage.EnterCredentialsAsync((string)_scenarioContext[ScenarioContextKeys.RegisteredEmail],
            (string)_scenarioContext[ScenarioContextKeys.RegisteredPassword]);
        SetCurrentPage(loginPage);
    }

    [Then("I am on the home page after successful login")]
    public async Task OnTheHomePageAfterSuccessfulLogin()
    {
        var loginPage = GetCurrentPage<LoginPage>();
        var homePage = await loginPage.SubmitAndVerifySuccessAsync();
        SetCurrentPage(homePage);
    }

    [Given("I am logged in")]
    public async Task AmLoggedIn()
    {
        await LoginRegistered();
        await OnTheHomePageAfterSuccessfulLogin();
    }

    [When("I click logout")]
    public async Task ClickLogout()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var homePage = await currentPage.ClickLogoutAsync();
        SetCurrentPage(homePage);
    }

    [Then("I am back on the home page \\(logged out state)")]
    public void BackOnTheHomePageLoggedOutState()
    {
        var homePage = GetCurrentPage<HomePage>();
        SetCurrentPage(homePage);
    }
}
