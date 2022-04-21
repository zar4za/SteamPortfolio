using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SteamPortfolio.Controllers
{
    public class AuthController : ControllerBase
    {
        private const string OpenIdProvider = "Steam";

        [HttpGet("/signin")]
        public IActionResult SignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/identity",
            };

            return Challenge(properties, OpenIdProvider);
        }

        [HttpGet("/logout")]
        public IActionResult LogOut()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/identity",
            };

            return SignOut(properties, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
