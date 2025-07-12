﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.DTOs;
using BusinessObjects.ViewModels;

namespace Services.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionViewModel>> GetAllAsync();
        Task<TransactionViewModel> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateTransactionDTO dto);
        Task<TransactionViewModel> UpdateAsync(UpdateTransactionDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TransactionViewModel>> GetByUserIdAsync(int userId);
    }
}
