// hack: overrides https://github.com/dotnet/dotnet/blob/main/src/aspnetcore/src/Identity/EntityFrameworkCore/src/UserStore.cs to make it work with MongoDB EF Provider

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace BlazorApp.Identity;

public class UserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null)
        : UserStore<ApplicationUser>(context, describer)
{
}

public class UserStore<TUser>(DbContext context, IdentityErrorDescriber? describer = null)
    : UserStore<TUser, IdentityRole<ObjectId>, DbContext, ObjectId>(context, describer)
    where TUser : ApplicationUser, new()
{
}

public class UserStore<TUser, TRole, TContext>(TContext context, IdentityErrorDescriber? describer = null)
    : UserStore<TUser, TRole, TContext, ObjectId>(context, describer)
    where TUser : ApplicationUser
    where TRole : IdentityRole<ObjectId>
    where TContext : DbContext
{
}

public class UserStore<TUser, TRole, TContext, TKey>(TContext context, IdentityErrorDescriber? describer = null)
    : UserStore<TUser, TRole, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>(context, describer)
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
{
}

public class UserStore<TUser, TRole, TContext, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>(TContext context, IdentityErrorDescriber? describer = null) :
    Microsoft.AspNetCore.Identity.EntityFrameworkCore.UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>(context, describer)
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserRole : IdentityUserRole<TKey>, new()
    where TUserLogin : IdentityUserLogin<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
{
    protected DbSet<TUser> UsersSet { get { return Context.Set<TUser>(); } }
    protected DbSet<TRole> Roles { get { return Context.Set<TRole>(); } }
    private DbSet<TUserClaim> UserClaims { get { return Context.Set<TUserClaim>(); } }
    protected DbSet<TUserRole> UserRoles { get { return Context.Set<TUserRole>(); } }

    public override Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var id = ObjectId.Parse(userId); // the base method calls ConvertIdFromString(userId) which doesn't work with MongoDB ObjectId
        return UsersSet.FindAsync([id], cancellationToken).AsTask();
    }

    public override IQueryable<TUser> Users
    {
        get { return UsersSet; }
    }

    public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        // join doesn't work with MongoDB EF Provider
        var roleIds = await UserRoles
            .Where(ur => ur.UserId.Equals(user.Id))
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);
        return await Roles
            .Where(r => roleIds.Contains(r.Id))
            .Select(r => r.Name!)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return UserClaims.Any() ?
            await UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync(cancellationToken) :
            [];
    }
}
