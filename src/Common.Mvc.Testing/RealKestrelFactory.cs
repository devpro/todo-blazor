using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Devpro.Common.Mvc.Testing;

/// <summary>
/// Workaround to use Playwright with a Blazor Server application.
/// </summary>
/// <typeparam name="TEntryPoint"></typeparam>
/// <remarks>
/// https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Testing/src/WebApplicationFactory.cs
/// </remarks>
public class RealKestrelFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
{
    private IHost? _host;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // builds the TestServer host first (for compatibility)
        var testHost = builder.Build();

        // switches to real Kestrel for network binding (listen to a random free HTTP port)
        builder.ConfigureWebHost(webHostBuilder =>
        {
            webHostBuilder.UseKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 0);
            });

            // disables HTTPS to avoid redirection failures
            webHostBuilder.UseSetting("https_port", "0");
        });

        ConfigureServices(builder);

        // builds and starts the real Kestrel host (force binding)
        _host = builder.Build();
        _host.Start();

        // captures the bound address
        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;

        ClientOptions.BaseAddress = addresses?.Select(x => new Uri(x)).Last()
            ?? throw new InvalidOperationException("No bound address found after Kestrel startup. Check for port conflicts or HTTPS-only config.");

        // starts the TestServer host for factory internals
        testHost.Start();

        // returns the TestServer host (factory expects it)
        return testHost;
    }

    protected virtual void ConfigureServices(IHostBuilder builder)
    {
    }

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    private void EnsureServer()
    {
        if (_host == null)
        {
            // forces factory bootstrap
            using var _ = CreateDefaultClient();
        }
    }

    protected override void Dispose(bool disposing)
    {
        _host?.Dispose();
        base.Dispose(disposing);
    }
}
