using LearningStarterServer.Common;
using LearningStarterServer.Entities;
using LearningStarterServer.Helpers;
using LearningStarterServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LearningStarterServer.Services
{
    public interface IAuthenticationService
    {
        AuthenticateResponse Login(AuthenticateRequest model);
        void Logout();
        bool IsUserLoggedIn();
    }

    public class AuthenticationService : IAuthenticationService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User {Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test"}
        };

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AuthenticateResponse Login(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            SignInUser(user);

            return new AuthenticateResponse(user);
        }

        public async void Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

        public bool IsUserLoggedIn()
        {
            return GetLoggedInUser() != null;
        }

        public User GetLoggedInUser()
        {
            var hasUser = _httpContextAccessor.HttpContext.Items.ContainsKey(Constants.HttpContext.Items.User);

            if (!hasUser)
            {
                return null;
            }

            return (User) _httpContextAccessor.HttpContext.Items[Constants.HttpContext.Items.User];
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private async void SignInUser(User user)
        {
            var claims = new List<Claim>();

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}