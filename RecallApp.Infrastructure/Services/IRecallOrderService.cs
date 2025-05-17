using RecallApp.Core.Models;

namespace RecallApp.Core.Services
{
    public interface IRecallOrderService
    {
        Task<RecallOrder> CreateRecallOrderAsync(RecallOrder recallOrder);
        Task<RecallOrder?> GetRecallOrderAsync(string id);
        Task<IEnumerable<RecallOrder>> GetCustomerRecallOrdersAsync(string customerId);
    }
}