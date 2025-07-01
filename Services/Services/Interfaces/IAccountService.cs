using BusinessObjects.DTOs;

namespace Services.Services.Interfaces
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDTO dto);
        Task<string> LoginAsync(LoginDTO dto);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName);
    }
}
