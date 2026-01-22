using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace BlazorApp.Data;

public class ApplicationUser : IdentityUser<ObjectId>
{
}
