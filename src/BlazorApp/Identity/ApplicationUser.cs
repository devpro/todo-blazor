using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace BlazorApp.Identity;

public class ApplicationUser : IdentityUser<ObjectId>
{
}
