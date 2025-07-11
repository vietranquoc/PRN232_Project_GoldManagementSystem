using BusinessObjects.DTOs;

namespace Services.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO?> GetRoleByIdAsync(int id);
        Task<RoleDTO> CreateRoleAsync(RoleDTO dto);
        Task<RoleDTO?> UpdateRoleAsync(int id, RoleDTO dto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
