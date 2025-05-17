
// RecallApp.Infrastructure/Repositories/IRecallOrderRepository.cs
using RecallApp.Core.Models;

namespace RecallApp.Infrastructure.Repositories
{
    public interface IRecallOrderRepository
    {
        Task<RecallOrder> CreateAsync(RecallOrder recallOrder);
        Task<RecallOrder?> GetByIdAsync(string id);
        Task<IEnumerable<RecallOrder>> GetByCustomerIdAsync(string customerId);
        Task UpdateAsync(RecallOrder recallOrder);
    }
}