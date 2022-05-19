using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SteamPortfolio.Models
{
    public class Inventory
    {
        public const string SteamId64PropertyName = "steam_id";
        public const string ItemsPropertyName = "items";
        public const string TotalSpentPropertyName = "spent";
        public const string TotalEarnedPropertyName = "earned";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string Id { get; set; } = null!;

        [BsonElement(SteamId64PropertyName)]
        [JsonIgnore]
        public string SteamId64 { get; set; } = null!;

        [BsonElement(ItemsPropertyName)]
        public IEnumerable<Item> Items { get; set; } = null!;

        [BsonElement(TotalSpentPropertyName)]
        [JsonPropertyName(TotalSpentPropertyName)]
        public decimal TotalSpent { get; set; }

        [BsonElement(TotalEarnedPropertyName)]
        [JsonPropertyName(TotalEarnedPropertyName)]
        public decimal TotalEarned { get; set; }

        [BsonIgnore]
        public Dictionary<string, decimal> Prices { get; set; } = null!;
    }
}
