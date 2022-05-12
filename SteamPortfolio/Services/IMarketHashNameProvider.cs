namespace SteamPortfolio.Services
{
    public interface IMarketHashNameProvider
    {
        public bool ValidateName(string marketHashName);
    }
}
