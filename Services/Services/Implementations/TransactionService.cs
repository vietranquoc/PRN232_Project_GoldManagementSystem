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
                UnitPrice = t.UnitPrice,
                TotalAmount = t.TotalAmount,
                TransactionDate = t.TransactionDate,
                Status = t.Status,
                IsActive = t.IsActive,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate,
                ReceiverName = t.ReceiverName,
                ReceiverPhone = t.ReceiverPhone,
                ReceiverEmail = t.ReceiverEmail,
                Province = t.Province,
                District = t.District,
                Address = t.Address,
                Note = t.Note
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
                UnitPrice = t.UnitPrice,
                TotalAmount = t.TotalAmount,
                TransactionDate = t.TransactionDate,
                Status = t.Status,
                IsActive = t.IsActive,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate,
                ReceiverName = t.ReceiverName,
                ReceiverPhone = t.ReceiverPhone,
                ReceiverEmail = t.ReceiverEmail,
                Province = t.Province,
                District = t.District,
                Address = t.Address,
                Note = t.Note
            };
        }

        public async Task<bool> CreateAsync(CreateTransactionDTO dto)
        {
            if (dto.DeliveryMethod == "pickup") 
            {
                dto.Province = "Nhận tại cửa hàng";
                dto.District = "";
                dto.Address = "";
            } 
            if (string.IsNullOrWhiteSpace(dto.Province) || 
                string.IsNullOrWhiteSpace(dto.District) || 
                string.IsNullOrWhiteSpace(dto.Address))
            {
                throw new Exception("Vui lòng nhập đầy đủ địa chỉ giao hàng.");
            }

            decimal shippingFee = 0;
            if (dto.ShippingMethod == "shipping2")
            {
                shippingFee = 100000;
            }

            var entity = new Transaction
            {
                UserId = dto.UserId,
                GoldTypeId = dto.GoldTypeId,
                UnitPrice = dto.UnitPrice,
                TotalAmount = dto.UnitPrice + shippingFee,
                TransactionDate = dto.TransactionDate,
                Status = dto.Status,
                ReceiverName = dto.ReceiverName,
                ReceiverPhone = dto.ReceiverPhone,
                ReceiverEmail = dto.ReceiverEmail,
                Province = dto.Province,
                District = dto.District,
                Address = dto.Address,
                Note = dto.Note
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
            entity.UnitPrice = dto.UnitPrice;
            entity.TotalAmount = dto.TotalAmount;
            entity.TransactionDate = dto.TransactionDate;
            entity.Status = dto.Status;
            entity.IsActive = dto.IsActive;
            entity.ReceiverName = dto.ReceiverName;
            entity.ReceiverPhone = dto.ReceiverPhone;
            entity.ReceiverEmail = dto.ReceiverEmail;
            entity.Province = dto.Province;
            entity.District = dto.District;
            entity.Address = dto.Address;
            entity.Note = dto.Note;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new TransactionViewModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                GoldTypeId = entity.GoldTypeId,
                UnitPrice = entity.UnitPrice,
                TotalAmount = entity.TotalAmount,
                TransactionDate = entity.TransactionDate,
                Status = entity.Status,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                ReceiverName = entity.ReceiverName,
                ReceiverPhone = entity.ReceiverPhone,
                ReceiverEmail = entity.ReceiverEmail,
                Province = entity.Province,
                District = entity.District,
                Address = entity.Address,
                Note = entity.Note
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
                UnitPrice = t.UnitPrice,
                TotalAmount = t.TotalAmount,
                TransactionDate = t.TransactionDate,
                Status = t.Status,
                IsActive = t.IsActive,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate,
                ReceiverName = t.ReceiverName,
                ReceiverPhone = t.ReceiverPhone,
                ReceiverEmail = t.ReceiverEmail,
                Province = t.Province,
                District = t.District,
                Address = t.Address,
                Note = t.Note
            });
        }
    }
}
