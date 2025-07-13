using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using BusinessObjects.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllProductsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetProductByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
        {
            try
            {
                var productId = await _service.CreateProductAsync(dto);
                if (productId <= 0) return BadRequest("Không thể tạo sản phẩm");
                return Ok(productId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi server khi tạo sản phẩm");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID không khớp");
            
            try
            {
                var updated = await _service.UpdateProductAsync(dto);
                if (updated == null) return NotFound("Không tìm thấy sản phẩm");
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi server khi cập nhật sản phẩm");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteProductAsync(id);
            if (!deleted) return NotFound();
            return Ok(deleted);
        }
    }
} 