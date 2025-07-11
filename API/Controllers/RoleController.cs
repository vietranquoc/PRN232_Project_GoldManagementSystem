using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Lấy tất cả role
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        // Lấy role theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        // Tạo mới role
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDTO dto)
        {
            var created = await _roleService.CreateRoleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Cập nhật role
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDTO dto)
        {
            var updated = await _roleService.UpdateRoleAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // Xóa role
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roleService.DeleteRoleAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
