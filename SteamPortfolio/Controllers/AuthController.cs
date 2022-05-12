using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SteamPortfolio.Services;

namespace SteamPortfolio.Controllers
{
    public class AuthController : ControllerBase
    {
        private const string NameIdentifierSchema = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string SteamOpenIdUri = "https://steamcommunity.com/openid/id/";
        private const string OpenIdProvider = "Steam";
        private const string DefaultRedirectUri = "/";

        private event Action<string> UserSignedIn;

        private string SteamId64 => User.Claims.First(x => x.Type == NameIdentifierSchema).Value.Replace(SteamOpenIdUri, string.Empty);

        public AuthController(IInventoryRepository inventoryRepository)
        {
            UserSignedIn += async (steamId64) =>
            {
                var contains = await inventoryRepository.ContainsAsync(steamId64);

                if (contains == false)
                    await inventoryRepository.CreateInventoryAsync(steamId64);
            };
        }


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
                RedirectUri = redirectUri ?? DefaultRedirectUri,
                IsPersistent = true,
                
            };

            var challengeResult = Challenge(properties, OpenIdProvider);
            UserSignedIn?.Invoke(SteamId64);
            return Challenge(properties, OpenIdProvider);
        }
    }
}
