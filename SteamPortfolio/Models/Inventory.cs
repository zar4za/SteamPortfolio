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

        private readonly List<Item> _items;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string Id { get; set; }

        [BsonElement(SteamId64PropertyName)]
        [JsonIgnore]
        public string SteamId64 { get; set; }

        [BsonElement(ItemsPropertyName)]
        public IEnumerable<Item> Items { get; set; }

        [BsonElement(TotalSpentPropertyName)]
        public decimal TotalSpent { get; set; }

        [BsonElement(TotalEarnedPropertyName)]
        public decimal TotalEarned { get; set; }

        public bool AddItem(Item item)
        {
            _items.Add(item);
            return true;
        }
    }
}
