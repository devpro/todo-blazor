using Bogus;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Microsoft.Playwright;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public class WalkthroughStep(ScenarioContext scenarioContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;

    private IPage Page => (IPage)_scenarioContext["Page"];

    private string ServerAddress => (string)_scenarioContext["BlazorServerAddress"];

    private HomePage? _homePage;

    private LoginPage? _loginPage;

    private RegisterPage? _registerPage;

    private RegisterConfirmPage? _registerConfirmPage;

    private readonly Faker _faker = new();

    [Given(@"I navigate to the home page")]
    public async Task GivenNavigateToHome()
    {
        _homePage = new HomePage(Page);
        await _homePage.NavigateToAsync(ServerAddress);
    }

    [Given(@"the home page shows ""(.*)""")]
    public async Task GivenHomeShowsTitle(string expectedTitle)
    {
        await _homePage!.VerifyPageHeaderAsync(expectedTitle);
    }

    [Then("I see login error {string}")]
    public void ThenISeeLoginError(string message)
    {
        //
    }

    [When(@"I open the login page")]
    public async Task WhenOpenLogin()
    {
        _loginPage = await _homePage!.OpenLoginAsync();
        await _loginPage.VerifyPageHeaderAsync("Log in");
    }

    [When(@"I enter invalid credentials")]
    public async Task WhenEnterInvalidCredentials()
    {
        var email = _faker.Internet.Email();
        var password = _faker.Internet.Password(8) + "aA9!";
        await _loginPage!.EnterCredentialsAsync(email, password);
        await _loginPage.SubmitAndVerifyFailureAsync("Error: Invalid login attempt.");
    }

    [When(@"I open the register page")]
    public async Task WhenOpenRegisterFromLogin()
    {
        _registerPage = await _loginPage!.OpenRegisterAsync();
        await _registerPage.VerifyPageHeaderAsync("Register");
    }

    [When(@"I register with valid credentials")]
    public async Task WhenRegisterRandom()
    {
        var email = _faker.Internet.Email();
        var password = _faker.Internet.Password(8) + "aA9!";
        _scenarioContext["RegisteredEmail"] = email;
        _scenarioContext["RegisteredPassword"] = password;

        await _registerPage!.EnterCredentialsAsync(email, password, password);
        _registerConfirmPage = await _registerPage.SubmitAndVerifySuccessAsync();
    }

    [Then(@"I see register confirmation page")]
    public async Task ThenSeeRegisterConfirmation()
    {
        await _registerConfirmPage!.VerifyPageHeaderAsync("Register confirmation");
    }

    [When(@"I click the confirmation link")]
    public async Task WhenClickConfirmationLink()
    {
        await _registerConfirmPage!.ClickConfirmationLinkAsync();
    }

    [When(@"I login with the registered credentials")]
    public async Task WhenLoginRegistered()
    {
        _loginPage = await _registerConfirmPage!.OpenLoginAsync();
        var email = (string)_scenarioContext["RegisteredEmail"];
        var password = (string)_scenarioContext["RegisteredPassword"];
        await _loginPage.EnterCredentialsAsync(email, password);
        _homePage = await _loginPage.SubmitAndVerifySuccessAsync();
    }

    [Then(@"I am on the home page after successful login")]
    public async Task ThenOnHomeAfterLogin()
    {
        await _homePage!.VerifyPageHeaderAsync("Hello, world!");
    }

    [When(@"I click logout")]
    public async Task WhenClickLogout()
    {
        await _homePage!.ClickLogoutAsync();
    }

    [Then(@"I am back on the home page \(logged out state\)")]
    public async Task ThenBackOnHomeLoggedOut()
    {
        await _homePage!.VerifyPageHeaderAsync("Hello, world!");
    }
}
