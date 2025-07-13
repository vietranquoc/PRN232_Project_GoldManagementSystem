using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoldPriceController : ControllerBase
    {
        private readonly IGoldPriceService _service;
        public GoldPriceController(IGoldPriceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server khi lấy danh sách giá vàng: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound("Không tìm thấy giá vàng");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server khi lấy thông tin giá vàng: " + ex.Message);
            }
        }

        [HttpGet("latest/{goldTypeId}")]
        public async Task<IActionResult> GetLatestByGoldTypeId(int goldTypeId)
        {
            try
            {
                var item = await _service.GetLatestByGoldTypeIdAsync(goldTypeId);
                if (item == null) return NotFound("Không tìm thấy giá vàng cho loại vàng này");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server khi lấy giá vàng mới nhất: " + ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Create([FromBody] CreateGoldPriceDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result) return BadRequest();
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGoldPriceDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            var updated = await _service.UpdateAsync(dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok(deleted);
        }
    }
}
