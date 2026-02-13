using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public class HomeSteps(ScenarioContext scenarioContext)
    : StepBase(scenarioContext)
{
    [Given(@"I navigate to the home page")]
    public async Task GivenNavigateToHomePage()
    {
        var homePage = new HomePage(Page);
        await homePage.NavigateToAsync(ServerAddress);
        SetCurrentPage(homePage);
    }

    [Given(@"the home page shows ""(.*)""")]
    public async Task GivenHomeShowsTitle(string expectedTitle = "Hello, world!")
    {
        var homePage = GetCurrentPage<HomePage>();
        await homePage.VerifyPageHeaderAsync(expectedTitle);
    }
}
