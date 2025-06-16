using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;
        public AccountService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:SecretKey is not configured.");
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new ArgumentException("Password and ConfirmPassword do not match.");
            }

            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                throw new ArgumentException($"Username already exists.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Hash the password
                RoleId = dto.RoleId,
                PhoneNumber = dto.Phone,
                Address = dto.Address,
                Email = dto.Email
            };

            _userRepository.Insert(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "GoldManagementApi",
                audience: "GoldManagementApi",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
