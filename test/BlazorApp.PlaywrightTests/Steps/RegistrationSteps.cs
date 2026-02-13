using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Support;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public class RegistrationSteps(ScenarioContext scenarioContext)
    : StepBase(scenarioContext)
{
    [When(@"I open the register page")]
    public async Task WhenOpenRegisterPage()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var registerPage = await currentPage.OpenRegisterAsync();
        await registerPage.VerifyPageHeaderAsync("Register");
        SetCurrentPage(registerPage);
    }

    [When(@"I register with valid credentials")]
    public async Task WhenRegisterWithValidCredentials()
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

    [Then(@"I see register confirmation page")]
    public async Task ThenSeeRegisterConfirmation()
    {
        var registerConfirmPage = GetCurrentPage<RegisterConfirmPage>();
        await registerConfirmPage.VerifyPageHeaderAsync("Register confirmation");
    }

    [When(@"I click the confirmation link")]
    public async Task WhenClickConfirmationLink()
    {
        var registerConfirmPage = GetCurrentPage<RegisterConfirmPage>();
        await registerConfirmPage.ClickConfirmationLinkAsync();
    }
}
