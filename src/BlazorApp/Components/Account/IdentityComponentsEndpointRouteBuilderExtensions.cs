using System.Security.Claims;
using Devpro.TodoList.BlazorApp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Devpro.TodoList.BlazorApp.Components.Account
{
    internal static class IdentityComponentsEndpointRouteBuilderExtensions
    {
        // maps endpoints required by Razor components in /Components/Account/Pages
        public static void MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var accountGroup = endpoints.MapGroup("/Account");

            accountGroup.MapPost("/Logout", async (
                ClaimsPrincipal user,
                [FromServices] SignInManager<ApplicationUser> signInManager,
                [FromForm] string returnUrl) =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });
        }
    }
}
