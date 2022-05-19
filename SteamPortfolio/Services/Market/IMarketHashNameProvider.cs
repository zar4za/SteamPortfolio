namespace SteamPortfolio.Services.Market
{
    public interface IMarketHashNameProvider
    {
        public bool ValidateName(string marketHashName);
    }
}
