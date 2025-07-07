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
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = 1,
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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Username = u.Username,
                Phone = u.PhoneNumber,
                Address = u.Address,
                Email = u.Email,
                RoleName = u.Role.Name,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate,
                CreatedBy = u.CreatedBy
            });
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _userRepository.GetUsersByRoleAsync(roleName);
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Username = u.Username,
                Phone = u.PhoneNumber,
                Address = u.Address,
                Email = u.Email,
                RoleName = u.Role.Name,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate,
                CreatedBy = u.CreatedBy
            });
        }

        public async Task CreateEmployeeAsync(CreateEmployeeDTO dto)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new ArgumentException("Username already exists.");

            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = 2,
                PhoneNumber = dto.Phone,
                Address = dto.Address,
                Email = dto.Email
            };

            _userRepository.Insert(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(int userId, UpdateProfileDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.Phone;
            user.Address = dto.Address;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDTO dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || user.Email?.ToLower() != dto.Email.ToLower())
                throw new Exception("User or email not found.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
