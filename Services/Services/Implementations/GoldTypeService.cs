using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using BusinessObjects.ViewModels;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.Implementations
{
    public class GoldTypeService : IGoldTypeService
    {
        private readonly IGoldTypeRepository _repo;
        public GoldTypeService(IGoldTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GoldTypeViewModel>> GetAllAsync()
        {
            var goldTypes = await _repo.GetAllAsync();
            return goldTypes.Select(g => new GoldTypeViewModel
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Karat = g.Karat,
                PriceType = g.PriceType,
                IsActive = g.IsActive,
                CreatedDate = g.CreatedDate,
                UpdatedDate = g.UpdatedDate
            });
        }

        public async Task<GoldTypeViewModel> GetByIdAsync(int id)
        {
            var g = await _repo.GetByIdAsync(id);
            if (g == null) return null;
            return new GoldTypeViewModel
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Karat = g.Karat,
                PriceType = g.PriceType,
                IsActive = g.IsActive,
                CreatedDate = g.CreatedDate,
                UpdatedDate = g.UpdatedDate
            };
        }

        public async Task<bool> CreateAsync(CreateGoldTypeDTO dto)
        {
            var entity = new GoldType
            {
                Name = dto.Name,
                Description = dto.Description,
                Karat = dto.Karat,
                PriceType = dto.PriceType
            };
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<GoldTypeViewModel> UpdateAsync(UpdateGoldTypeDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return null;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Karat = dto.Karat;
            entity.PriceType = dto.PriceType;
            entity.IsActive = dto.IsActive;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new GoldTypeViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Karat = entity.Karat,
                PriceType = entity.PriceType,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
} 