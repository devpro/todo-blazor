using System.Text.RegularExpressions;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public partial class HomeSteps(ScenarioContext scenarioContext)
    : StepBase(scenarioContext)
{
    [Given("I navigate to the home page")]
    public async Task GivenNavigateToHomePage()
    {
        var homePage = new HomePage(Page);
        await homePage.NavigateToAsync(ServerAddress);
        SetCurrentPage(homePage);
    }

    [Given("the home page shows \"(.*)\"")]
    public async Task GivenHomeShowsTitle(string expectedTitle = "Hello, world!")
    {
        var homePage = GetCurrentPage<HomePage>();
        await homePage.VerifyPageHeaderAsync(expectedTitle);
    }


    [Given("I am a new user")]
    public async Task GivenAmNewUser()
    {
        await GivenNavigateToHomePage();
        await WhenOpenRegisterPage();
    }

    [When("I open the login page")]
    public async Task WhenOpenLogin()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var loginPage = await currentPage.OpenLoginAsync();
        await loginPage.VerifyPageHeaderAsync("Log in");
        SetCurrentPage(loginPage);
    }

    [When("I open the register page")]
    public async Task WhenOpenRegisterPage()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var registerPage = await currentPage.OpenRegisterAsync();
        await registerPage.VerifyPageHeaderAsync("Register");
        SetCurrentPage(registerPage);
    }

    [When("I open the todo page")]
    public async Task OpenTheTodoPage()
    {
        var currentPage = GetCurrentPage<PageBase>();
        var todoPage = await currentPage.OpenTodoAsync();
        await todoPage.VerifyPageHeaderAsync(TodoTitleRegex());
        SetCurrentPage(todoPage);
    }

    [GeneratedRegex("Todo", RegexOptions.None)]
    private static partial Regex TodoTitleRegex();
}
