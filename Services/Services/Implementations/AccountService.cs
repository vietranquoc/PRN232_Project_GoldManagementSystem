using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;
        private readonly ILogger<AccountService> _logger;
        public AccountService(IUserRepository userRepository, IConfiguration configuration, ILogger<AccountService> logger)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:SecretKey is not configured.");
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            try
            {
                ValidateRegisterDTO(dto);

                var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration attempt with existing username: {Username}", dto.Username);
                    throw new ArgumentException("Username already exists.");
                }

                var user = new User
                {
                    FullName = dto.FullName,
                    Username = dto.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    RoleId = 1,
                    PhoneNumber = dto.Phone,
                    Address = dto.Address,
                    Email = dto.Email,
                    CreatedDate = DateTime.UtcNow
                };

                _userRepository.Insert(user);
                await _userRepository.SaveChangesAsync();
                _logger.LogInformation("User registered successfully: {Username}", dto.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for username: {Username}", dto.Username);
                throw;
            }
        }

        public async Task<TokenViewModel> LoginAsync(LoginDTO dto)
        {
            try
            {
                ValidateLoginDTO(dto);

                var user = await _userRepository.GetByUsernameAsync(dto.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                {
                    _logger.LogWarning("Failed login attempt for username: {Username}", dto.Username);
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

                var token = GenerateJwtToken(user);

                var userProfile = new UserProfileViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = user.FullName,
                    RoleName = user.Role.Name,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Address = user.Address
                };

                _logger.LogInformation("User logged in successfully: {Username}", user.Username);
                return new TokenViewModel
                {
                    Code = token,
                    Expiration = DateTime.UtcNow.AddHours(1),
                    LifeTime = 3600, // 1 hour in seconds
                    UserProfile = userProfile
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", dto.Username);
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                _logger.LogInformation("Retrieved {Count} users", users.Count());
                return users.Select(MapToUserDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    _logger.LogWarning("GetUsersByRoleAsync called with invalid role name");
                    throw new ArgumentException("Role name cannot be empty.", nameof(roleName));
                }

                var users = await _userRepository.GetUsersByRoleAsync(roleName.Trim());
                _logger.LogInformation("Retrieved {Count} users for role: {RoleName}", users.Count(), roleName);
                return users.Select(MapToUserDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users for role: {RoleName}", roleName);
                throw;
            }
        }

        public async Task CreateEmployeeAsync(CreateEmployeeDTO dto)
        {
            try
            {
                ValidateCreateEmployeeDTO(dto);

                var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Create employee attempt with existing username: {Username}", dto.Username);
                    throw new ArgumentException("Username already exists.");
                }

                var user = new User
                {
                    FullName = dto.FullName,
                    Username = dto.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    RoleId = 2,
                    PhoneNumber = dto.Phone,
                    Address = dto.Address,
                    Email = dto.Email,
                };

                _userRepository.Insert(user);
                await _userRepository.SaveChangesAsync();
                _logger.LogInformation("Employee created successfully: {Username}", dto.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee: {Username}", dto.Username);
                throw;
            }
        }

        public async Task UpdateProfileAsync(int userId, UpdateProfileDTO dto)
        {
            try
            {
                ValidateUpdateProfileDTO(dto);

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Update profile attempt for non-existent user ID: {UserId}", userId);
                    throw new ArgumentException("User not found.", nameof(userId));
                }

                user.FullName = dto.FullName;
                user.Email = dto.Email;
                user.PhoneNumber = dto.Phone;
                user.Address = dto.Address;

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                _logger.LogInformation("Profile updated successfully for user ID: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user ID: {UserId}", userId);
                throw;
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDTO dto)
        {
            try
            {
                ValidateForgotPasswordDTO(dto);

                var user = await _userRepository.GetByUsernameAsync(dto.Username);
                if (user == null || user.Email != dto.Email)
                {
                    _logger.LogWarning("Forgot password attempt for invalid username or email: {Username}", dto.Username);
                    throw new ArgumentException("Invalid username or email.");
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                _logger.LogInformation("Password reset successfully for username: {Username}", dto.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for username: {Username}", dto.Username);
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(nameof(user.Id), user.Id.ToString()),
                new Claim(nameof(user.FullName), user.FullName),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "Unknown"),
                new Claim(nameof(user.Username), user.Username ?? ""),
                new Claim(nameof(user.Email), user.Email ?? ""),
                new Claim(nameof(user.PhoneNumber), user.PhoneNumber ?? ""),
                new Claim(nameof(user.Address), user.Address ?? "")
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

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("Generated JWT for user {Username} with expiration {Expiration}",
                user.FullName, token.ValidTo);
            return tokenString;
        }

        private UserDTO MapToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Phone = user.PhoneNumber,
                Address = user.Address,
                Email = user.Email,
                RoleName = user.Role?.Name,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                CreatedBy = user.CreatedBy
            };
        }

        private void ValidateRegisterDTO(RegisterDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Username)) throw new ArgumentException("Username is required.", nameof(dto.Username));
            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Password is required.", nameof(dto.Password));
            if (dto.Password != dto.ConfirmPassword) throw new ArgumentException("Password and ConfirmPassword do not match.");
            if (string.IsNullOrWhiteSpace(dto.FullName)) throw new ArgumentException("Full name is required.", nameof(dto.FullName));
            if (string.IsNullOrWhiteSpace(dto.Email) || !new EmailAddressAttribute().IsValid(dto.Email))
                throw new ArgumentException("Invalid email format.", nameof(dto.Email));
        }

        private void ValidateLoginDTO(LoginDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Username)) throw new ArgumentException("Username is required.", nameof(dto.Username));
            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Password is required.", nameof(dto.Password));
        }

        private void ValidateCreateEmployeeDTO(CreateEmployeeDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Username)) throw new ArgumentException("Username is required.", nameof(dto.Username));
            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Password is required.", nameof(dto.Password));
            if (dto.Password.Length < 6) throw new ArgumentException("Password must be at least 6 characters.", nameof(dto.Password));
            if (string.IsNullOrWhiteSpace(dto.ConfirmPassword)) throw new ArgumentException("ConfirmPassword is required.", nameof(dto.ConfirmPassword));
            if (dto.Password != dto.ConfirmPassword) throw new ArgumentException("Password and ConfirmPassword do not match.");
            if (string.IsNullOrWhiteSpace(dto.FullName)) throw new ArgumentException("Full name is required.", nameof(dto.FullName));
            if (string.IsNullOrWhiteSpace(dto.Email) || !new EmailAddressAttribute().IsValid(dto.Email))
                throw new ArgumentException("Invalid email format.", nameof(dto.Email));
        }

        private void ValidateUpdateProfileDTO(UpdateProfileDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.FullName)) throw new ArgumentException("Full name is required.", nameof(dto.FullName));
            if (string.IsNullOrWhiteSpace(dto.Email) || !new EmailAddressAttribute().IsValid(dto.Email))
                throw new ArgumentException("Invalid email format.", nameof(dto.Email));
        }

        private void ValidateForgotPasswordDTO(ForgotPasswordDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Username)) throw new ArgumentException("Username is required.", nameof(dto.Username));
            if (string.IsNullOrWhiteSpace(dto.Email) || !new EmailAddressAttribute().IsValid(dto.Email))
                throw new ArgumentException("Invalid email format.", nameof(dto.Email));
            if (string.IsNullOrWhiteSpace(dto.NewPassword)) throw new ArgumentException("New password is required.", nameof(dto.NewPassword));
        }
    }
}
