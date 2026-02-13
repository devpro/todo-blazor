using Devpro.TodoList.BlazorApp.PlaywrightTests.Pages;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Steps;

[Binding]
public class TodoSteps(ScenarioContext scenarioContext)
    : StepBase(scenarioContext)
{
    [When("I add todo {string}")]
    public async Task AddTodo(string todo)
    {
        var todoPage = GetCurrentPage<TodoPage>();
        await todoPage.AddItem(todo);
    }

    [Then("the todo list should contain {string}")]
    public void TodoListShouldContain(string todo)
    {
        // TODO
    }
}
