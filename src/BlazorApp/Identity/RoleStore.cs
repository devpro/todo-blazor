// hack: overrides https://github.com/dotnet/dotnet/blob/main/src/aspnetcore/src/Identity/EntityFrameworkCore/src/RoleStore.cs to make it work with MongoDB EF Provider

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Devpro.TodoList.BlazorApp.Identity;

public class RoleStore<TRole>(ApplicationDbContext context, IdentityErrorDescriber? describer = null)
    : RoleStore<TRole, DbContext, ObjectId>(context, describer)
    where TRole : IdentityRole<ObjectId>
{
}

public class RoleStore<TRole, TContext>(TContext context, IdentityErrorDescriber? describer = null)
    : RoleStore<TRole, TContext, string>(context, describer)
    where TRole : IdentityRole<string>
    where TContext : DbContext
{
}

public class RoleStore<TRole, TContext, TKey>(TContext context, IdentityErrorDescriber? describer = null)
    : RoleStore<TRole, TContext, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>(context, describer),
    IQueryableRoleStore<TRole>,
    IRoleClaimStore<TRole>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
}

public class RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>(TContext context, IdentityErrorDescriber? describer = null) :
    Microsoft.AspNetCore.Identity.EntityFrameworkCore.RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>(context, describer)
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
    where TUserRole : IdentityUserRole<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
{
}
