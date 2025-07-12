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
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        public TransactionService(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllAsync()
        {
            var transactions = await _repo.GetAllAsync();
            return transactions.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                UserId = t.UserId,
                GoldTypeId = t.GoldTypeId,
                TransactionType = t.TransactionType,
                Weight = t.Weight,
                UnitPrice = t.UnitPrice,
                TotalAmount = t.TotalAmount,
                TransactionDate = t.TransactionDate,
                Status = t.Status,
                IsActive = t.IsActive,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate
            });
        }

        public async Task<TransactionViewModel> GetByIdAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return null;
            return new TransactionViewModel
            {
                Id = t.Id,
                UserId = t.UserId,
                GoldTypeId = t.GoldTypeId,
                TransactionType = t.TransactionType,
                Weight = t.Weight,
                UnitPrice = t.UnitPrice,
                TotalAmount = t.TotalAmount,
                TransactionDate = t.TransactionDate,
                Status = t.Status,
                IsActive = t.IsActive,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate
            };
        }

        public async Task<bool> CreateAsync(CreateTransactionDTO dto)
        {
            var entity = new Transaction
            {
                UserId = dto.UserId,
                GoldTypeId = dto.GoldTypeId,
                TransactionType = dto.TransactionType,
                Weight = dto.Weight,
                UnitPrice = dto.UnitPrice,
                TotalAmount = dto.Weight * dto.UnitPrice,
                TransactionDate = dto.TransactionDate,
                Status = dto.Status
            };
            _repo.Insert(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionViewModel> UpdateAsync(UpdateTransactionDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return null;
            entity.UserId = dto.UserId;
            entity.GoldTypeId = dto.GoldTypeId;
            entity.TransactionType = dto.TransactionType;
            entity.Weight = dto.Weight;
            entity.UnitPrice = dto.UnitPrice;
            entity.TotalAmount = dto.TotalAmount;
            entity.TransactionDate = dto.TransactionDate;
            entity.Status = dto.Status;
            entity.IsActive = dto.IsActive;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new TransactionViewModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                GoldTypeId = entity.GoldTypeId,
                TransactionType = entity.TransactionType,
                Weight = entity.Weight,
                UnitPrice = entity.UnitPrice,
                TotalAmount = entity.TotalAmount,
                TransactionDate = entity.TransactionDate,
                Status = entity.Status,
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

        public async Task<IEnumerable<TransactionViewModel>> GetByUserIdAsync(int userId)
        {
            var transactions = await _repo.GetByUserIdAsync(userId);
            return transactions.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                UserId = t.UserId,
                GoldTypeId = t.GoldTypeId,
                TransactionType = t.TransactionType,
                Weight = t.Weight,
                UnitPrice = t.UnitPrice,
                TotalAmount = t.TotalAmount,
                TransactionDate = t.TransactionDate,
                Status = t.Status,
                IsActive = t.IsActive,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate
            });
        }
    }
}
