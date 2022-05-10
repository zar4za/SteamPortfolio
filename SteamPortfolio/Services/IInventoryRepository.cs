using SteamPortfolio.Models;

namespace SteamPortfolio.Services
{
    public interface IInventoryRepository
    {
        public Task<bool> AddItemAsync(string steamId64, Item item);
        public Task<bool> RemoveItemAsync(string steamId64, Item item);
        public Task<bool> UpdateItemAsync(string steamId64, Item item);
        public Task<Inventory> CreateInventoryAsync(string steamId64);
        public Task<Inventory> GetInventoryAsync(string steamId64);
    }
}
