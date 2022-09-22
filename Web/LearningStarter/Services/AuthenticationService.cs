using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace LearningStarter.Services;
public interface IAuthenticationService
{
    ClaimsPrincipal RequestingUser { get; }
    bool Login(string username, string password);

    void Logout();

    bool IsUserLoggedIn();

    User GetLoggedInUser();
}

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(
        DataContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public ClaimsPrincipal RequestingUser
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;

            return !httpContext.User.Identity.IsAuthenticated
                ? null
                : httpContext.User;
        }
    }
    

    public bool Login(string username, string password)
    {
        
        return true;
    }

    public void Logout()
    {
        _httpContextAccessor.HttpContext.SignOutAsync().Wait();
    }

    public bool IsUserLoggedIn()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public User GetLoggedInUser()
    {
        if (!IsUserLoggedIn())
            return null;

        var id = RequestingUser.FindFirstValue(JwtClaimTypes.Subject).SafeParseInt();

        return id == null
            ? null 
            : _context.Users.SingleOrDefault(x => x.Id == id.Value);
    }

    // helper methods
    private async void SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties { IssuedUtc = DateTimeOffset.UtcNow };
        authProperties.ExpiresUtc = authProperties.IssuedUtc.Value.AddDays(1);

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}
