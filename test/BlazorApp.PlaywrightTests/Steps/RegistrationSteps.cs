using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Support;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public class RegistrationSteps(ScenarioContext scenarioContext)
    : StepBase(scenarioContext)
{
    [When("I register with valid credentials")]
    public async Task RegisterWithValidCredentials()
    {
        var registerPage = GetCurrentPage<RegisterPage>();
        var email = GenerateEmail();
        var password = GeneratePassword();
        _scenarioContext[ScenarioContextKeys.RegisteredEmail] = email;
        _scenarioContext[ScenarioContextKeys.RegisteredPassword] = password;
        await registerPage.EnterCredentialsAsync(email, password, password);
        var registerConfirmPage = await registerPage.SubmitAndVerifySuccessAsync();
        SetCurrentPage(registerConfirmPage);
    }

    [Given("I register with valid credentials")]
    public async Task RegisterAndConfirmWithValidCredentials()
    {
        await RegisterWithValidCredentials();
        await SeeRegisterConfirmation();
        await ClickConfirmationLink();
    }

    [Then("I see register confirmation page")]
    public async Task SeeRegisterConfirmation()
    {
        var registerConfirmPage = GetCurrentPage<RegisterConfirmPage>();
        await registerConfirmPage.VerifyPageHeaderAsync("Register confirmation");
    }

    [When("I click the confirmation link")]
    public async Task ClickConfirmationLink()
    {
        var registerConfirmPage = GetCurrentPage<RegisterConfirmPage>();
        await registerConfirmPage.ClickConfirmationLinkAsync();
    }
}
