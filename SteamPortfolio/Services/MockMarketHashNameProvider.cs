namespace SteamPortfolio.Services
{
    public class MockMarketHashNameProvider : IMarketHashNameProvider
    {
        public bool ValidateName(string marketHashName)
        {
            return true;
        }
    }
}
