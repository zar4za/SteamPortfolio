using Microsoft.AspNetCore.Mvc;
using SteamPortfolio.Models;
using SteamPortfolio.Extensions;
using SteamPortfolio.Services.Market;
using SteamPortfolio.Services.InventoryRepository;

namespace SteamPortfolio.Controllers
{
    [ApiController]
    [Route("api/v1/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMarketHashNameProvider _marketHashNameProvider;
        private readonly IPriceProvider _priceProvider;

        public InventoryController(IInventoryRepository inventoryRepository,
            IMarketHashNameProvider marketHashNameProvider,
            IPriceProvider priceProvider)
        {
            _inventoryRepository = inventoryRepository;
            _marketHashNameProvider = marketHashNameProvider;
            _priceProvider = priceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryAsync()
        {
            if (User?.Identity?.IsAuthenticated == false)
                return Unauthorized();

            var inventory = await _inventoryRepository.GetInventoryAsync(User!.GetSteamId64()!);

            inventory.Prices = await _priceProvider.GetSteamPrices(inventory.Items.Select(x => x.MarketHashName));

            if (inventory != null)
                return Ok(inventory);
            
            return BadRequest();
        }

        [HttpGet("steam-inventory")]
        public async Task<Inventory> GetSteamInventoryAsync()
        {
            //TODO
            return new Inventory();
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemAsync([FromBody] Item item)
        {
            if (User?.Identity?.IsAuthenticated == false)
                return Unauthorized();
            if (_marketHashNameProvider.ValidateName(item.MarketHashName) == false)
                return BadRequest();

            var success = await _inventoryRepository.AddItemAsync(User!.GetSteamId64()!, item);

            if (success)
                return Ok();

            return BadRequest();
        }

        [HttpPost("update-item")]
        public async Task<IActionResult> UpdateItemAsync([FromBody] Item item)
        {
            if (User?.Identity?.IsAuthenticated == false)
                return Unauthorized();

            var success = await _inventoryRepository.UpdateItemAsync(User!.GetSteamId64()!, item);

            if (success)
                return Ok();

            return BadRequest();
        }

        [HttpPost("remove-item")]
        public async Task<IActionResult> RemoveItemAsync([FromBody] Item item)
        {
            if (User?.Identity?.IsAuthenticated == false)
                return Unauthorized();

            var success = await _inventoryRepository.RemoveItemAsync(User!.GetSteamId64()!, item);

            if (success)
                return Ok();

            return BadRequest();
        }
    }
}
