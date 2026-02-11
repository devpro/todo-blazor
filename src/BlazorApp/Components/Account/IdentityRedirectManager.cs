using Devpro.TodoList.BlazorApp.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Devpro.TodoList.BlazorApp.Components.Account
{
    public class IdentityRedirectManager(NavigationManager navigationManager)
    {
        protected NavigationManager NavigationManager { get; private set; } = navigationManager;

        public const string StatusCookieName = "Identity.StatusMessage";

        private static readonly CookieBuilder s_statusCookieBuilder = new()
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            IsEssential = true,
            MaxAge = TimeSpan.FromSeconds(5),
        };

        public virtual void RedirectTo(string? uri)
        {
            uri ??= "";

            // prevents open redirects
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = NavigationManager.ToBaseRelativePath(uri);
            }

            NavigationManager.NavigateTo(uri);
        }

        public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
        {
            var uriWithoutQuery = NavigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
            var newUri = NavigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
            RedirectTo(newUri);
        }

        public void RedirectToWithStatus(string uri, string message, HttpContext context)
        {
            context.Response.Cookies.Append(StatusCookieName, message, s_statusCookieBuilder.Build(context));
            RedirectTo(uri);
        }

        private string CurrentPath => NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path);

        public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

        public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
            => RedirectToWithStatus(CurrentPath, message, context);

        public void RedirectToInvalidUser(UserManager<ApplicationUser> userManager, HttpContext context)
            => RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
    }
}
