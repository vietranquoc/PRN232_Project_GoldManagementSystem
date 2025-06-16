using BusinessObjects.DTOs;
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
    }
}
