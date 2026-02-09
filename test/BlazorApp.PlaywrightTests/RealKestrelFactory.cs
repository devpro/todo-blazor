using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;  // For GetRequiredService
using Microsoft.Extensions.Hosting;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests;

public class RealKestrelFactory : WebApplicationFactory<Program>
{
    private IHost? _host;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Build the TestServer host first (for compatibility)
        var testHost = builder.Build();

        // Switch to real Kestrel for network binding
        builder.ConfigureWebHost(webHostBuilder =>
        {
            webHostBuilder.UseKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 0);  // Random free HTTP port
            });

            // Disable HTTPS to avoid redirection failures (common in Blazor)
            webHostBuilder.UseSetting("https_port", "0");
        });

        // Build and start the real Kestrel host
        _host = builder.Build();
        _host.Start();  // Force binding now

        // Capture the bound address
        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;

        ClientOptions.BaseAddress = addresses?.Select(x => new Uri(x)).Last()
            ?? throw new InvalidOperationException("No bound address found after Kestrel startup. Check for port conflicts or HTTPS-only config.");

        // Start the TestServer host for factory internals
        testHost.Start();

        // Return the TestServer host (factory expects it)
        return testHost;
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
            // Force factory bootstrap
            using var _ = CreateDefaultClient();
        }
    }

    protected override void Dispose(bool disposing)
    {
        _host?.Dispose();
        base.Dispose(disposing);
    }
}
