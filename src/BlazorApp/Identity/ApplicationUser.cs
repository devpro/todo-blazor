using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Devpro.TodoList.BlazorApp.Identity;

public class ApplicationUser : IdentityUser<ObjectId>
{
}
