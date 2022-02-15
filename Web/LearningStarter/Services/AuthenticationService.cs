using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace LearningStarter.Services
{
    public interface IAuthenticationService
    {
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

        public bool Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return false if user not found
            if (user == null) return false;

            // authentication successful so generate cookie and signin
            SignInUser(user);

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

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == "Id").Value);

            return _context.Users.SingleOrDefault(x => x.Id == userId);
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
}