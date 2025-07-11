using Services.Services.Interfaces;
using BusinessObjects.EntityModel;
using Repositories.Infrastructure.Interfaces;
using BusinessObjects.DTOs;

namespace Services.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            });
        }

        public async Task<RoleDTO?> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return null;
            return new RoleDTO 
            { 
                Id = role.Id, 
                Name = role.Name, 
                Description = role.Description 
            };
        }

        public async Task<RoleDTO> CreateRoleAsync(RoleDTO dto)
        {
            var role = new Role 
            { 
                Name = dto.Name, 
                Description = dto.Description 
            };
            _roleRepository.Insert(role);
            await _roleRepository.SaveChangesAsync();
            dto.Id = role.Id;
            return dto;
        }

        public async Task<RoleDTO?> UpdateRoleAsync(int id, RoleDTO dto)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return null;
            role.Name = dto.Name;
            role.Description = dto.Description;
            _roleRepository.Update(role);
            await _roleRepository.SaveChangesAsync();
            return new RoleDTO 
            { 
                Id = role.Id, 
                Name = role.Name, 
                Description = role.Description 
            };
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return false;
            _roleRepository.Remove(role);
            await _roleRepository.SaveChangesAsync();
            return true;
        }
    }
}
