namespace SteamPortfolio.Services.Market
{
    public class MockMarketHashNameProvider : IMarketHashNameProvider
    {
        public bool ValidateName(string marketHashName)
        {
            return true;
        }
    }
}
