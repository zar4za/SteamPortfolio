using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SteamPortfolio.Controllers
{
    public class AuthController : ControllerBase
    {
        private const string OpenIdProvider = "Steam";
        private const string DefaultRedirectUri = "/";

        [HttpGet("/logout")]
        public IActionResult LogOut(string? redirectUri)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri ?? DefaultRedirectUri
            };

            return SignOut(properties, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("/signin")]
        public IActionResult SignIn(string? redirectUri)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri ?? DefaultRedirectUri
            };

            return Challenge(properties, OpenIdProvider);
        }
    }
}
