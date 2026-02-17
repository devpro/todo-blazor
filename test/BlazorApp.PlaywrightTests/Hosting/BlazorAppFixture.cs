namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Hosting;

public class BlazorAppFixture : IAsyncDisposable
{
    private BlazorAppFactory Factory { get; }

    public string ServerAddress => Factory.ServerAddress;

    public BlazorAppFixture()
    {
        Factory = new BlazorAppFactory();
        Factory.CreateDefaultClient();
    }

    public async ValueTask DisposeAsync()
    {
        await Factory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
