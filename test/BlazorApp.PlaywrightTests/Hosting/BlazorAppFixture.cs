using System;
using System.Threading.Tasks;
using Withywoods.AspNetCore.Mvc.Testing;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Hosting;

public class BlazorAppFixture : IAsyncDisposable
{
    private KestrelWebAppFactory<Program> Factory { get; }

    public string ServerAddress => Factory.ServerAddress;

    public BlazorAppFixture()
    {
        Factory = new KestrelWebAppFactory<Program>();
        Factory.CreateDefaultClient();
    }

    public async ValueTask DisposeAsync()
    {
        await Factory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
