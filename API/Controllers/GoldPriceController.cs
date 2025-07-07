using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using BusinessObjects.DTOs;

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

        [HttpGet("lastest")]
        //[Authorize]
        public async Task<IActionResult> GetLatestGoldPrices()
        {
            try
            {
                var result = await _goldPriceService.GetLatestGoldPricesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve gold prices: {ex.Message}");
            }
        }

        [HttpPost("manual")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddManualGoldPrice([FromBody] ManualGoldPriceDTO dto)
        {
            try
            {
                await _goldPriceService.AddManualGoldPriceAsync(dto);
                return Ok("Gold price added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to add gold price: {ex.Message}");
            }
        }
    }
}
