using Devpro.TodoList.BlazorApp.PlaywrightTests.Support;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Testing;
using Microsoft.Playwright;
using Reqnroll;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Hooks;

[Binding]
public class PlaywrightHooks
{
    private static IPlaywright? s_playwrightInstance;
    private static IBrowser? s_browser;
    private static BlazorAppFixture? s_appFixture;

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        s_playwrightInstance = await Playwright.CreateAsync();
        s_browser = await s_playwrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            SlowMo = 0
        });
        s_appFixture = new BlazorAppFixture();
    }

    [BeforeScenario]
    public static async Task BeforeScenario(ScenarioContext scenarioContext)
    {
        var context = await s_browser!.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new() { Width = 1920, Height = 1080 },
            IgnoreHTTPSErrors = true
        });

        await context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        var page = await context.NewPageAsync();
        page.SetDefaultTimeout(10000);
        page.SetDefaultNavigationTimeout(20000);

        scenarioContext[ScenarioContextKeys.Page] = page;
        scenarioContext[ScenarioContextKeys.Context] = context;
        scenarioContext[ScenarioContextKeys.BlazorServerAddress] = s_appFixture!.ServerAddress;
    }

    [AfterScenario]
    public static async Task AfterScenario(ScenarioContext scenarioContext)
    {
        if (!scenarioContext.TryGetValue(ScenarioContextKeys.Page, out var pageObj) || pageObj is not IPage page) return;
        if (!scenarioContext.TryGetValue(ScenarioContextKeys.Context, out var contextObj) || contextObj is not IBrowserContext context) return;

        var traceDir = Path.Combine(Directory.GetCurrentDirectory(), "traces");
        Directory.CreateDirectory(traceDir);

        if (scenarioContext.TestError != null)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = $"failure_{timestamp}.png", FullPage = true });
            await context.Tracing.StopAsync(new TracingStopOptions { Path = $"failure_{timestamp}.zip" });
        }
        else
        {
            await context.Tracing.StopAsync(new TracingStopOptions { Path = null });
        }

        await page.CloseAsync();
        await context.CloseAsync();
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (s_appFixture != null) await s_appFixture.DisposeAsync();
        await s_browser?.CloseAsync()!;
        s_playwrightInstance?.Dispose();
    }
}
