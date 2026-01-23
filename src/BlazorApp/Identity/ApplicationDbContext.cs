using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace BlazorApp.Identity;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<ObjectId>, ObjectId>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .ToCollection("users");

        builder.Entity<ApplicationUser>(b =>
        {
            b.Property(u => u.Id).HasElementName("_id");
            b.Property(u => u.UserName).HasElementName("username");
            b.Property(u => u.NormalizedUserName).HasElementName("normalized_username");
            b.Property(u => u.Email).HasElementName("emailaddress");
            b.Property(u => u.NormalizedEmail).HasElementName("normalized_emailaddress");
            b.Property(u => u.EmailConfirmed).HasElementName("is_emailaddress_confirmed");
            b.Property(u => u.PasswordHash).HasElementName("passwordhash");
            b.Property(u => u.SecurityStamp).HasElementName("securitystamp");
            b.Property(u => u.ConcurrencyStamp).HasElementName("concurrencystamp");
            b.Property(u => u.PhoneNumber).HasElementName("phonenumber");
            b.Property(u => u.PhoneNumberConfirmed).HasElementName("is_phonenumber_confirmed");
            b.Property(u => u.TwoFactorEnabled).HasElementName("is_twofactor_enabled");
            b.Property(u => u.LockoutEnd).HasElementName("lockout_end");
            b.Property(u => u.LockoutEnabled).HasElementName("is_lockout_enabled");
            b.Property(u => u.AccessFailedCount).HasElementName("accessfailedcount");
        });
    }
}
