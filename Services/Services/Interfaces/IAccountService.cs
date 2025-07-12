﻿using BusinessObjects.DTOs;

namespace Services.Services.Interfaces
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDTO dto);
        Task<TokenViewModel> LoginAsync(LoginDTO dto);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName);
        Task CreateEmployeeAsync(CreateEmployeeDTO dto);
        Task UpdateProfileAsync(int userId, UpdateProfileDTO dto);
        Task ForgotPasswordAsync(ForgotPasswordDTO dto);
        Task LogoutAsync(int userId);
    }
}
