// source: https://github.com/dotnet/dotnet/blob/main/src/aspnetcore/src/Identity/EntityFrameworkCore/src/RoleStore.cs

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace BlazorApp.Identity;

/// <summary>
/// Creates a new instance of a persistence store for roles.
/// </summary>
/// <typeparam name="TRole">The type of the class representing a role</typeparam>
public class RoleStore<TRole> : RoleStore<TRole, DbContext, ObjectId>
    where TRole : IdentityRole<ObjectId>
{
    /// <summary>
    /// Constructs a new instance of <see cref="RoleStore{TRole}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public RoleStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

/// <summary>
/// Creates a new instance of a persistence store for roles.
/// </summary>
/// <typeparam name="TRole">The type of the class representing a role.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
public class RoleStore<TRole, TContext> : RoleStore<TRole, TContext, string>
    where TRole : IdentityRole<string>
    where TContext : DbContext
{
    /// <summary>
    /// Constructs a new instance of <see cref="RoleStore{TRole, TContext}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public RoleStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

/// <summary>
/// Creates a new instance of a persistence store for roles.
/// </summary>
/// <typeparam name="TRole">The type of the class representing a role.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
public class RoleStore<TRole, TContext, TKey> : RoleStore<TRole, TContext, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>,
    IQueryableRoleStore<TRole>,
    IRoleClaimStore<TRole>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    /// <summary>
    /// Constructs a new instance of <see cref="RoleStore{TRole, TContext, TKey}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public RoleStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

public class RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>(TContext context, IdentityErrorDescriber? describer = null) :
    Microsoft.AspNetCore.Identity.EntityFrameworkCore.RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>(context, describer)
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
    where TUserRole : IdentityUserRole<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
{
    protected DbSet<TRoleClaim> RoleClaims { get { return Context.Set<TRoleClaim>(); } }

    public override async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);

        //return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType!, c.ClaimValue!)).ToListAsync(cancellationToken);
        return (await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).ToListAsync()).Select(c => new Claim(c.ClaimType!, c.ClaimValue!)).ToList();
    }

    /// <summary>
    /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role">The role to add the claim to.</param>
    /// <param name="claim">The claim to add to the role.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        ArgumentNullException.ThrowIfNull(claim);

        RoleClaims.Add(CreateRoleClaim(role, claim));
        return Task.FromResult(false);
    }

    /// <summary>
    /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role">The role to remove the claim from.</param>
    /// <param name="claim">The claim to remove from the role.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        ArgumentNullException.ThrowIfNull(claim);
        var claims = await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
        foreach (var c in claims)
        {
            RoleClaims.Remove(c);
        }
    }
}
