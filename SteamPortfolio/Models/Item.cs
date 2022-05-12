using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SteamPortfolio.Models
{
    public class Item
    {
        public const string AmountPropertyName = "amount";
        public const string TotalSpentPropertyName = "total";
        public const string MarketHashNamePropertyName = "market_hash_name";

        [BsonElement(AmountPropertyName)]
        [JsonPropertyName(AmountPropertyName)]
        public int Amount { get; set; }

        [BsonElement(TotalSpentPropertyName)]
        [JsonPropertyName(TotalSpentPropertyName)]
        public decimal TotalSpent { get; set; }

        [BsonElement(MarketHashNamePropertyName)]
        [JsonPropertyName(MarketHashNamePropertyName)]
        public string MarketHashName { get; set; } = null!;
    }
}
