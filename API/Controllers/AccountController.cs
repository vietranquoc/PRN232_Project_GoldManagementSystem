﻿using BusinessObjects.DTOs;
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
    }
}
