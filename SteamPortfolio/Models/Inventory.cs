using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SteamPortfolio.Models
{
    public class Inventory
    {
        private readonly List<Item> _items;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string Id { get; set; }

        [BsonElement("steam_id")]
        [JsonIgnore]
        public string SteamId64 { get; set; }

        [BsonElement("items")]
        public IEnumerable<Item> Items { get; set; }

        [BsonElement("spent")]
        public decimal TotalSpent { get; set; }

        [BsonElement("earned")]
        public decimal TotalEarned { get; set; }

        public bool AddItem(Item item)
        {
            _items.Add(item);
            return true;
        }
    }
}
