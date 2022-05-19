namespace SteamPortfolio.Services.Market
{
    public interface IPriceProvider
    {
        public Task<Dictionary<string, decimal>> GetSteamPrices(IEnumerable<string> marketHashNames);
    }
}
