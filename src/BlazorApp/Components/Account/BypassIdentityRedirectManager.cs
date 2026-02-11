using Microsoft.AspNetCore.Components;

namespace Devpro.TodoList.BlazorApp.Components.Account;

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
