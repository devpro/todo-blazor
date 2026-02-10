using Devpro.TodoList.BlazorApp.Components.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace Devpro.TodoList.BlazorApp.PlaywrightTests;

public class BypassIdentityRedirectManager(NavigationManager navigationManager, IHttpContextAccessor httpContextAccessor)
    : IdentityRedirectManager(navigationManager)
{
    public override void RedirectTo(string? uri)
    {
        uri ??= "";

        var context = httpContextAccessor.HttpContext;
        if (context != null)
        {
            context.Response.Redirect(uri, permanent: false);
        }
        else
        {
            NavigationManager.NavigateTo(uri);
        }
    }
}
