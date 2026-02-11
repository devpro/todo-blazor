using Devpro.Common.Mvc.Testing;
using Devpro.TodoList.BlazorApp.Components.Account;
using Devpro.TodoList.BlazorApp.PlaywrightTests.Testing.Account;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests.Testing
{
    public class BlazorAppFactory : RealKestrelFactory<Program>
    {
        protected override void ConfigureServices(IHostBuilder builder)
        {
            base.ConfigureServices(builder);

            // switches to a redirect manager to fix issue with successful login redirect
            builder.ConfigureServices(services =>
            {
                var originalDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IdentityRedirectManager));
                if (originalDescriptor != null)
                {
                    services.Remove(originalDescriptor);
                }

                services.AddScoped<IdentityRedirectManager, BypassIdentityRedirectManager>();
            });
        }
    }
}
