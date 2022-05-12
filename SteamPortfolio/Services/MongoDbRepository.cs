using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SteamPortfolio.Models;

namespace SteamPortfolio.Services
{
    public class MongoDbRepository : IInventoryRepository
    {
        private readonly IMongoCollection<Inventory> _inventoryCollection;

        public MongoDbRepository(IOptions<MongoDbRepositorySettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            _inventoryCollection = mongoDatabase.GetCollection<Inventory>(bookStoreDatabaseSettings.Value.UserCollectionName);
        }

        public async Task<bool> AddItemAsync(string steamId64, Item item)
        {
            var filter = Builders<Inventory>.Filter.Eq(Inventory.SteamId64PropertyName, steamId64);
            var update = Builders<Inventory>.Update.AddToSet(Inventory.ItemsPropertyName, item);
            return await TryUpdateOneAsync(filter, update);
        }

        public async Task<Inventory> CreateInventoryAsync(string steamId64)
        {
            var inventory = new Inventory()
            {
                SteamId64 = steamId64,
                Items = new List<Item>()
            };
            await _inventoryCollection.InsertOneAsync(inventory);
            return inventory;
        }

        public async Task<Inventory> GetInventoryAsync(string steamId64)
        {
            var inventoryCursor = await _inventoryCollection.FindAsync(i => i.SteamId64 == steamId64);
            var inventory = await inventoryCursor.FirstOrDefaultAsync();
            return inventory;
        }

        public async Task<bool> RemoveItemAsync(string steamId64, Item item)
        {
            var filter = Builders<Inventory>.Filter.Eq(Inventory.SteamId64PropertyName, steamId64);
            var update = Builders<Inventory>.Update.Pull(Inventory.ItemsPropertyName, item);
            return await TryUpdateOneAsync(filter, update);
        }

        public async Task<bool> UpdateItemAsync(string steamId64, Item item)
        {
            var filter = Builders<Inventory>.Filter.And(new[] {
                Builders<Inventory>.Filter.Eq(Inventory.SteamId64PropertyName, steamId64),
                Builders<Inventory>.Filter.Eq($"{Inventory.ItemsPropertyName}.{Item.MarketHashNamePropertyName}", item.MarketHashName)
            });
            var update = Builders<Inventory>.Update.Set($"{Inventory.ItemsPropertyName}.$", item);
            return await TryUpdateOneAsync(filter, update);
        }

        private async Task<bool> TryUpdateOneAsync(FilterDefinition<Inventory> filter, UpdateDefinition<Inventory> update)
        {
            var result = await _inventoryCollection.UpdateOneAsync(filter, update);

            if (result.IsModifiedCountAvailable)
                return result.ModifiedCount != 0;

            return false;
        }
    }
}
