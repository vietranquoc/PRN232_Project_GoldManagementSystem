using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using BusinessObjects.ViewModels;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using System;
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
                Note = t.Note,
                Details = t.TransactionDetails?.Select(td => new TransactionDetailViewModel {
                    ProductName = td.Product?.Name ?? $"SP#{td.ProductId}",
                    Quantity = td.Quantity,
                    UnitPrice = td.UnitPrice,
                    TotalAmount = td.TotalAmount
                }).ToList() ?? new List<TransactionDetailViewModel>()
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

        public async Task<IEnumerable<TransactionViewModel>> GetRecentAsync(int count)
        {
            var transactions = await _repo.GetRecentAsync(count);
            return transactions.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                UserId = t.UserId,
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

        public async Task<object> GetStatisticsAsync()
        {
            var (totalRevenue, monthIncome, lastMonthIncome, yearAnalysis) = await _repo.GetStatisticsAsync();
            string monthCompare = "";
            if (lastMonthIncome > 0)
            {
                var percent = ((double)(monthIncome - lastMonthIncome) / (double)lastMonthIncome) * 100;
                monthCompare = (percent >= 0 ? "+" : "") + percent.ToString("0.#") + "% so với tháng trước";
            }
            return new
            {
                totalRevenue,
                monthIncome,
                monthCompare,
                yearAnalysis = yearAnalysis.Select(x => new { year = x.year, amount = x.amount })
            };
        }

        public async Task<bool> UpdateTransactionStatusAsync(int transactionId, string status)
        {
            return await _repo.UpdateStatusAsync(transactionId, status);
        }

        public async Task<IEnumerable<object>> GetDailyRevenueAsync(int days = 30)
        {
            var transactions = await _repo.GetAllAsync();
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-days + 1);
            
            var dailyRevenue = transactions
                .Where(t => t.TransactionDate.Date >= startDate && t.TransactionDate.Date <= endDate)
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new
                {
                    date = g.Key.ToString("yyyy-MM-dd"),
                    revenue = g.Sum(t => t.TotalAmount),
                    transactionCount = g.Count()
                })
                .OrderBy(x => x.date)
                .ToList();

            // Fill missing dates with zero revenue
            var result = new List<object>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var existing = dailyRevenue.FirstOrDefault(x => x.date == date.ToString("yyyy-MM-dd"));
                result.Add(new
                {
                    date = date.ToString("yyyy-MM-dd"),
                    revenue = existing?.revenue ?? 0,
                    transactionCount = existing?.transactionCount ?? 0
                });
            }

            return result;
        }
    }
}
