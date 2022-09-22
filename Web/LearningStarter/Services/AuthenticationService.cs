using System.Linq;
using System.Security.Claims;
using IdentityModel;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Http;

namespace LearningStarter.Services;

public interface IAuthenticationService
{
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
    
    public User GetLoggedInUser()
    {
        if (!IsUserLoggedIn())
            return null;

        var id = RequestingUser.FindFirstValue(JwtClaimTypes.Subject).SafeParseInt();

        return id == null
            ? null 
            : _context.Users.SingleOrDefault(x => x.Id == id.Value);
    }
    
    private ClaimsPrincipal RequestingUser
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var identity = httpContext?.User.Identity;

            if (identity == null)
            {
                return null;
            }

            return !identity.IsAuthenticated
                ? null
                : httpContext.User;
        }
    }

    private bool IsUserLoggedIn()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
