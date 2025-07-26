using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects.DTOs;
using Services.Services.Interfaces;
using API.Utils;
namespace API.Controllers
{
    [Route("signin-google")]
    [ApiController]
    public class GoogleLoginController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public GoogleLoginController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string returnUrl = "/")
        {
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authResult.Succeeded)
                return BadRequest("Google login failed");

            var claims = authResult.Principal.Identities.FirstOrDefault()?.Claims;

            if (claims == null) return BadRequest("No user credential found!");

            var fullname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var username = email?.Split('@')[0];

            RegisterDTO dto = new RegisterDTO
            {
                FullName = fullname,
                Username = username,
                Email = email,
                Password = PasswordGeneratorUtils.Generate()
            };

            await _accountService.RegisterAsync(dto);

            return Ok();
        }
    }
}
    