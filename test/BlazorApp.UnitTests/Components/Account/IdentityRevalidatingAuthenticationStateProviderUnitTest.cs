using System.Security.Claims;
using Devpro.TodoList.BlazorApp.Components.Account;
using Devpro.TodoList.BlazorApp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;

namespace BlazorApp.UnitTests.Components.Account;

public class IdentityRevalidatingAuthenticationStateProviderUnitTest
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly TestIdentityRevalidatingProvider _provider;

    public IdentityRevalidatingAuthenticationStateProviderUnitTest()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);

        var optionsMock = new Mock<IOptions<IdentityOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new IdentityOptions
        {
            ClaimsIdentity = { SecurityStampClaimType = "AspNet.Identity.SecurityStamp" }
        });

        _provider = new TestIdentityRevalidatingProvider(
            Mock.Of<ILoggerFactory>(),
            Mock.Of<IServiceScopeFactory>(),
            optionsMock.Object);
    }

    [Fact]
    public async Task ValidateSecurityStampAsync_UserNotFound_ReturnsFalse()
    {
        var objectId = new ObjectId();
        var principal = CreatePrincipalWithStamp(objectId.ToString(), "stamp-abc");
        _userManagerMock.Setup(m => m.GetUserAsync(principal)).ReturnsAsync((ApplicationUser)null!);

        var result = await _provider.ExposedValidateSecurityStampAsync(_userManagerMock.Object, principal);

        Assert.False(result);
    }

    [Fact]
    public async Task ValidateSecurityStampAsync_NoSecurityStampSupport_ReturnsTrue()
    {
        var objectId = new ObjectId();
        var principal = CreatePrincipalWithStamp(objectId.ToString(), "stamp-abc");
        var user = new ApplicationUser { Id = objectId };

        _userManagerMock.Setup(m => m.GetUserAsync(principal)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.SupportsUserSecurityStamp).Returns(false);

        var result = await _provider.ExposedValidateSecurityStampAsync(_userManagerMock.Object, principal);

        Assert.True(result);
    }

    [Fact]
    public async Task ValidateSecurityStampAsync_StampsMatch_ReturnsTrue()
    {
        var objectId = new ObjectId();
        var principal = CreatePrincipalWithStamp(objectId.ToString(), "stamp-abc");
        var user = new ApplicationUser { Id = objectId };

        _userManagerMock.Setup(m => m.GetUserAsync(principal)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.SupportsUserSecurityStamp).Returns(true);
        _userManagerMock.Setup(m => m.GetSecurityStampAsync(user)).ReturnsAsync("stamp-abc");

        var result = await _provider.ExposedValidateSecurityStampAsync(_userManagerMock.Object, principal);

        Assert.True(result);
    }

    [Fact]
    public async Task ValidateSecurityStampAsync_StampsDifferent_ReturnsFalse()
    {
        var objectId = new ObjectId();
        var principal = CreatePrincipalWithStamp(objectId.ToString(), "stamp-abc");
        var user = new ApplicationUser { Id = objectId };

        _userManagerMock.Setup(m => m.GetUserAsync(principal)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.SupportsUserSecurityStamp).Returns(true);
        _userManagerMock.Setup(m => m.GetSecurityStampAsync(user)).ReturnsAsync("new-stamp");

        var result = await _provider.ExposedValidateSecurityStampAsync(_userManagerMock.Object, principal);

        Assert.False(result);
    }

    [Fact]
    public void RevalidationInterval_Is30Minutes()
    {
        Assert.Equal(TimeSpan.FromMinutes(30), _provider.ExposedRevalidationInterval);
    }

    // Helper
    private static ClaimsPrincipal CreatePrincipalWithStamp(string userId, string stamp)
    {
        var identity = new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim("AspNet.Identity.SecurityStamp", stamp)
        ], "TestAuth");

        return new ClaimsPrincipal(identity);
    }
}

// exposes protected members for testing
internal class TestIdentityRevalidatingProvider(
    ILoggerFactory loggerFactory,
    IServiceScopeFactory scopeFactory,
    IOptions<IdentityOptions> options)
    : IdentityRevalidatingAuthenticationStateProvider(loggerFactory, scopeFactory, options)
{

    public TimeSpan ExposedRevalidationInterval => RevalidationInterval;

    public Task<bool> ExposedValidateSecurityStampAsync(UserManager<ApplicationUser> um, ClaimsPrincipal p)
        => ValidateSecurityStampAsync(um, p);
}
