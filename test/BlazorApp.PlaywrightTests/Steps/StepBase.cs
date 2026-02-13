using Bogus;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Support;
using Microsoft.Playwright;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps
{
    public class StepBase(ScenarioContext scenarioContext)
    {
        protected readonly Faker _faker = new();

        protected readonly ScenarioContext _scenarioContext = scenarioContext;

        protected IPage Page => (IPage)_scenarioContext[ScenarioContextKeys.Page];

        protected string ServerAddress => (string)_scenarioContext[ScenarioContextKeys.BlazorServerAddress];

        protected void SetCurrentPage<T>(T page)
            where T : PageBase
        {
            _scenarioContext[ScenarioContextKeys.CurrentPage] = page;
        }

        protected T GetCurrentPage<T>()
            where T : PageBase
        {
            if (!_scenarioContext.TryGetValue<T>(ScenarioContextKeys.CurrentPage, out var page))
            {
                throw new Exception($"Not on ${typeof(T)} page");
            }

            return page;
        }

        protected string GenerateEmail()
        {
            return _faker.Internet.Email();
        }

        protected string GeneratePassword()
        {
            return _faker.Internet.Password(8) + "aA9!";
        }
    }
}
