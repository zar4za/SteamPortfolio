namespace SteamPortfolio.Steam
{
    public interface IPriceProvider
    {
        public Task<Dictionary<string, decimal>> GetSteamPrices(IEnumerable<string> marketHashNames);
    }
}
