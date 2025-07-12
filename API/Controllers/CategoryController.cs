using BusinessObjects.EntityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using BusinessObjects.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllCategories());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetCategoryById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
        {
            var result = await _service.CreateNewCategory(dto);
            if (!result) return BadRequest();
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateCategoryDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            var updated = await _service.UpdateCategory(dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteCategory(id);
            if (!deleted) return NotFound();
            return Ok(deleted);
        }
    }
} 