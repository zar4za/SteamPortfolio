using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SteamPortfolio.Models
{
    public class Item
    {
        [BsonElement("amount")]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [BsonElement("total")]
        [JsonPropertyName("total")]
        public decimal TotalSpent { get; set; }

        [BsonElement("market_hash_name")]
        [JsonPropertyName("market_hash_name")]
        public string MarketHashName { get; set; } = null!;
    }
}
