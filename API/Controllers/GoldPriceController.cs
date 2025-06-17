using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoldPriceController : ControllerBase
    {
        private readonly IGoldPriceService _goldPriceService;

        public GoldPriceController(IGoldPriceService goldPriceService)
        {
            _goldPriceService = goldPriceService;
        }

        /// <summary>
        /// Updates gold prices by fetching data from BTMC API.
        /// Only accessible to Manager role.
        /// </summary>
        /// <returns>Confirmation message</returns>
        [HttpPost("update")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateGoldPrices()
        {
            try
            {
                await _goldPriceService.UpdateGoldPricesAsync();
                return Ok("Gold prices updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update gold prices: {ex.Message}");
            }
        }

        [HttpGet("lastest")]
        [Authorize]
        public async Task<IActionResult> GetlastestGoldPrices()
        {
            try
            {
                var goldPrices = await _goldPriceService.GetLatestGoldPricesAsync();
                var result = goldPrices.Select(gp => new
                {
                    GoldTypeId = gp.GoldTypeId,
                    GoldTypeName = gp.GoldType.Name,
                    BuyPrice = gp.BuyPrice,
                    SellPrice = gp.SellPrice,
                    RecordedAt = gp.RecordedAt,
                    CreatedBy = gp.CreatedBy,
                    CreatedDate = gp.CreatedDate
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve gold prices: {ex.Message}");
            }
        }    
    }
}
