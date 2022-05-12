using Microsoft.AspNetCore.Mvc;
using SteamPortfolio.Models;
using SteamPortfolio.Services;

namespace SteamPortfolio.Controllers
{
    [ApiController]
    [Route("api/v1/inventory")]
    public class InventoryController : ControllerBase
    {
        private const string NameIdentifierSchema = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string SteamOpenIdUri = "https://steamcommunity.com/openid/id/";

        private IInventoryRepository _inventoryRepository;
        private IMarketHashNameProvider _marketHashNameProvider;

        private string SteamId64 => User.Claims.First(x => x.Type == NameIdentifierSchema).Value.Replace(SteamOpenIdUri, string.Empty);

        public InventoryController(IInventoryRepository inventoryRepository, IMarketHashNameProvider marketHashNameProvider)
        {
            _inventoryRepository = inventoryRepository;
            _marketHashNameProvider = marketHashNameProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryAsync()
        {
            if (User?.Identity?.IsAuthenticated == false)
                return Unauthorized();

            var inventory = await _inventoryRepository.GetInventoryAsync(SteamId64);

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

            var success = await _inventoryRepository.AddItemAsync(SteamId64, item);

            if (success)
                return Ok();

            return BadRequest();
        }

        [HttpPost("update-item")]
        public async Task<IActionResult> UpdateItemAsync([FromBody] Item item)
        {
            if (User?.Identity?.IsAuthenticated == false)
                return Unauthorized();

            var success = await _inventoryRepository.UpdateItemAsync(SteamId64, item);

            if (success)
                return Ok();

            return BadRequest();
        }

        [HttpPost("remove-item")]
        public async Task<IActionResult> RemoveItemAsync([FromBody] Item item)
        {
            if (User?.Identity?.IsAuthenticated == false)
                return Unauthorized();

            var success = await _inventoryRepository.RemoveItemAsync(SteamId64, item);

            if (success)
                return Ok();

            return BadRequest();
        }
    }
}
