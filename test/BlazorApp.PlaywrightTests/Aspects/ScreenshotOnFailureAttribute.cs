using Devpro.TodoList.BlazorApp.PlaywrightTests.Smoke;
using Metalama.Framework.Aspects;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Aspects;

[CompileTime]
public class ScreenshotOnFailureAttribute : OverrideMethodAspect
{
    public override dynamic? OverrideMethod()
    {
        throw new NotSupportedException("Sync override not implemented; use async methods.");
    }

    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        try
        {
            return await meta.ProceedAsync();
        }
        catch
        {
            var test = (SmokeTestBase)meta.This;
            await test.TakeScreenshotAsync();
            throw;
        }
    }
}
