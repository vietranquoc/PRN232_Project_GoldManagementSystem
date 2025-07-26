using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                await _accountService.RegisterAsync(dto);
                return Ok("User registered successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var token = await _accountService.LoginAsync(dto);
                return Ok(new { Token = token });
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all user accounts.
        /// Only accessible to Manager role.
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet("all")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _accountService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve users: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all customer accounts.
        /// Only accessible to Manager role.
        /// </summary>
        /// <returns>List of customer users</returns>
        [HttpGet("customers")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCustomerUsers()
        {
            try
            {
                var users = await _accountService.GetUsersByRoleAsync("Customer");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve customer users: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all employee accounts.
        /// Only accessible to Manager role.
        /// </summary>
        /// <returns>List of employee users</returns>
        [HttpGet("employees")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetEmployeeUsers()
        {
            try
            {
                var users = await _accountService.GetUsersByRoleAsync("Employee");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve employee users: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all manager accounts.
        /// Only accessible to Manager role.
        /// </summary>
        /// <returns>List of manager users</returns>
        [HttpGet("managers")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetManagerUsers()
        {
            try
            {
                var users = await _accountService.GetUsersByRoleAsync("Manager"); 
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve admin users: {ex.Message}");
            }
        }

        [HttpPost("create-employee")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _accountService.CreateEmployeeAsync(dto);
                return Ok("Employee account created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                await _accountService.UpdateProfileAsync(userId, dto);
                return Ok("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _accountService.ForgotPasswordAsync(dto);
                return Ok("Password reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Logout user by invalidating the current token.
        /// </summary>
        /// <returns>Logout confirmation message</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Lấy user ID từ token
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                // Có thể thêm logic để blacklist token hoặc cập nhật trạng thái user
                // Hiện tại chỉ trả về thông báo thành công
                await _accountService.LogoutAsync(userId);
                
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Logout failed: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var profile = await _accountService.GetProfileAsync(userId);
            return Ok(profile);
        }
    }
}
