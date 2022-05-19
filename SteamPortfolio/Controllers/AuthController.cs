using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SteamPortfolio.Extensions;
using SteamPortfolio.Services.InventoryRepository;

namespace SteamPortfolio.Controllers
{
    public class AuthController : ControllerBase
    {
        private const string OpenIdProvider = "Steam";
        private const string DefaultRedirectUri = "/";

        private event Action<string> UserSignedIn;

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
                RedirectUri = redirectUri ?? DefaultRedirectUri,
            };

            return SignOut(properties);
        }

        [HttpGet("/signin")]
        public IActionResult SignIn(string? redirectUri)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri ?? DefaultRedirectUri,
                IsPersistent = true
            };
            
            var challengeResult = Challenge(properties, OpenIdProvider);
            UserSignedIn?.Invoke(User!.GetSteamId64()!);
            return Challenge(properties, OpenIdProvider);
        }
    }
}
