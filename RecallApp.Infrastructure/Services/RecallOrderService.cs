using RecallApp.Core.Models;
using RecallApp.Core.Services;
using RecallApp.Infrastructure.Repositories;

namespace RecallApp.API.Services
{
    public class RecallOrderService : IRecallOrderService
    {
        private readonly IRecallOrderRepository _repository;
        //private readonly ILogger<RecallOrderService> _logger;

        public RecallOrderService(
            IRecallOrderRepository repository)
           // ILogger<RecallOrderService> logger)
        {
            _repository = repository;
          //  _logger = logger;
        }

        public async Task<RecallOrder> CreateRecallOrderAsync(RecallOrder recallOrder)
        {
           // _logger.LogInformation("Creating recall order with {ItemCount} items", recallOrder.Items.Count);

            // Ensure we have a valid ID
            if (string.IsNullOrEmpty(recallOrder.Id))
            {
                recallOrder.Id = Guid.NewGuid().ToString();
            }

            // Set timestamps
            recallOrder.RequestDate = DateTime.UtcNow;
            recallOrder.LastModified = DateTime.UtcNow;

            // Save to Cosmos DB (this will trigger the decomposition Azure Function)
            await _repository.CreateAsync(recallOrder);

            return recallOrder;
        }

        public async Task<RecallOrder?> GetRecallOrderAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RecallOrder>> GetCustomerRecallOrdersAsync(string customerId)
        {
            return await _repository.GetByCustomerIdAsync(customerId);
        }
    }
}