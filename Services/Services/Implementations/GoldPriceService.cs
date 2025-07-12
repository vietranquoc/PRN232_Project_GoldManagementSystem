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
    public class GoldPriceService : IGoldPriceService
    {
        private readonly IGoldPriceRepository _repo;
        public GoldPriceService(IGoldPriceRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GoldPriceViewModel>> GetAllAsync()
        {
            var prices = await _repo.GetAllAsync();
            return prices.Select(p => new GoldPriceViewModel
            {
                Id = p.Id,
                GoldTypeId = p.GoldTypeId,
                BuyPrice = p.BuyPrice,
                SellPrice = p.SellPrice,
                RecordedAt = p.RecordedAt,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            });
        }

        public async Task<GoldPriceViewModel> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return new GoldPriceViewModel
            {
                Id = p.Id,
                GoldTypeId = p.GoldTypeId,
                BuyPrice = p.BuyPrice,
                SellPrice = p.SellPrice,
                RecordedAt = p.RecordedAt,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            };
        }

        public async Task<bool> CreateAsync(CreateGoldPriceDTO dto)
        {
            var entity = new GoldPrice
            {
                GoldTypeId = dto.GoldTypeId,
                BuyPrice = dto.BuyPrice,
                SellPrice = dto.SellPrice,
                RecordedAt = dto.RecordedAt
            };
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<GoldPriceViewModel> UpdateAsync(UpdateGoldPriceDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return null;
            entity.GoldTypeId = dto.GoldTypeId;
            entity.BuyPrice = dto.BuyPrice;
            entity.SellPrice = dto.SellPrice;
            entity.RecordedAt = dto.RecordedAt;
            entity.IsActive = dto.IsActive;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new GoldPriceViewModel
            {
                Id = entity.Id,
                GoldTypeId = entity.GoldTypeId,
                BuyPrice = entity.BuyPrice,
                SellPrice = entity.SellPrice,
                RecordedAt = entity.RecordedAt,
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

        public async Task<GoldPriceViewModel> GetLatestByGoldTypeIdAsync(int goldTypeId)
        {
            var prices = await _repo.GetAllAsync();
            var latest = prices
                .Where(p => p.GoldTypeId == goldTypeId)
                .OrderByDescending(p => p.RecordedAt)
                .FirstOrDefault();
            if (latest == null) return null;
            return new GoldPriceViewModel
            {
                Id = latest.Id,
                GoldTypeId = latest.GoldTypeId,
                BuyPrice = latest.BuyPrice,
                SellPrice = latest.SellPrice,
                RecordedAt = latest.RecordedAt,
                IsActive = latest.IsActive,
                CreatedDate = latest.CreatedDate,
                UpdatedDate = latest.UpdatedDate
            };
        }
    }
}
