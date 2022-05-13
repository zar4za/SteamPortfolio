using SteamPortfolio.Services;
using System.Text.Json.Nodes;

namespace SteamPortfolio.Steam
{
    public class SkinportParser : IPriceProvider, IMarketHashNameProvider
    {
        private const int UpdateTimeSeconds = 1200;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, decimal> _prices;
        private DateTime _updateTime;

        public SkinportParser(IHttpClientFactory factory)
        {
            _prices = new Dictionary<string, decimal>();
            _httpClient = factory.CreateClient();
            UpdatePrices().Wait();
            _updateTime = DateTime.MinValue;
        }

        public async Task<Dictionary<string, decimal>> GetSteamPrices(IEnumerable<string> marketHashNames)
        {
            if (DateTime.UtcNow - _updateTime > TimeSpan.FromSeconds(UpdateTimeSeconds))
                await UpdatePrices();

            var prices = new Dictionary<string, decimal>();

            foreach (var name in marketHashNames)
            {
                prices.Add(name, _prices[name]);
            }

            return prices;
        }

        public bool ValidateName(string marketHashName) => _prices.ContainsKey(marketHashName);

        private async Task UpdatePrices()
        {
            foreach (var price in await GetSkinportPrices())
            {
                _prices[price!["market_hash_name"]!.GetValue<string>()] = price["suggested_price"]?.GetValue<decimal>() ?? decimal.Zero;
            }

            _updateTime = DateTime.UtcNow;
        }

        private async Task<JsonArray> GetSkinportPrices()
        {
            var url = "https://api.skinport.com/v1/items?app_id=730&currency=RUB";
            var response = await _httpClient.GetAsync(url);
            var json = JsonNode.Parse(await response.Content.ReadAsStreamAsync());
            return json!.AsArray();
        }
    }
}
