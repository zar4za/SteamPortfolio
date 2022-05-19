using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SteamPortfolio.Models;

namespace SteamPortfolio.Services.InventoryRepository
{
    public class MongoDbRepository : IInventoryRepository
    {
        private readonly IMongoCollection<Inventory> _inventoryCollection;

        public MongoDbRepository(IOptions<MongoDbRepositorySettings> MongoDbDatabaseSettings)
        {
            var mongoClient = new MongoClient(MongoDbDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(MongoDbDatabaseSettings.Value.DatabaseName);
            _inventoryCollection = mongoDatabase.GetCollection<Inventory>(MongoDbDatabaseSettings.Value.UserCollectionName);
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

        public async Task<Inventory?> GetInventoryAsync(string steamId64)
        {
            var cursor = await FindInventoryAsync(steamId64);
            var inventory = await cursor.FirstOrDefaultAsync();
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

        public async Task<bool> ContainsAsync(string steamId64)
        {
            var cursor = await FindInventoryAsync(steamId64);
            return cursor.Any();
        }

        private async Task<IAsyncCursor<Inventory>?> FindInventoryAsync(string steamId64)
        {
            return await _inventoryCollection.FindAsync(i => i.SteamId64 == steamId64);
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
