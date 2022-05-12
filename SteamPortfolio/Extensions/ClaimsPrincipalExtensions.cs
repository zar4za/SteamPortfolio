using System.Security.Claims;

namespace SteamPortfolio.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        private const string NameIdentifierSchema = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string SteamOpenIdUri = "https://steamcommunity.com/openid/id/";

        public static string? GetSteamId64(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == NameIdentifierSchema)?.Value.Replace(SteamOpenIdUri, string.Empty);
        }
    }
}
