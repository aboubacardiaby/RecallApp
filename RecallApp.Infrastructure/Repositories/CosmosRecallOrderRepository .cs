using Microsoft.Azure.Cosmos;
using RecallApp.Core.Models;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;

namespace RecallApp.Infrastructure.Repositories
{
    public class CosmosRecallOrderRepository : IRecallOrderRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;
       // private readonly ILogger<CosmosRecallOrderRepository> _logger;

        public CosmosRecallOrderRepository(
            Microsoft.Azure.Cosmos.Container container)
          //  ILogger<CosmosRecallOrderRepository> logger)
        {
            _container = container;
            //_logger = logger;
        }

        public async Task<RecallOrder> CreateAsync(RecallOrder recallOrder)
        {
            try
            {
                var response = await _container.CreateItemAsync(
                    recallOrder,
                    new PartitionKey(recallOrder.PartitionKey));

               // _logger.LogInformation("Created recall order {RecallOrderId} with request charge {RequestCharge} RUs",
                   // recallOrder.Id, response.RequestCharge);

                return response.Resource;
            }
            catch (CosmosException ex)
            {
              //  _logger.LogError(ex, "Error creating recall order {RecallOrderId}", recallOrder.Id);
                throw;
            }
        }

        public async Task<RecallOrder?> GetByIdAsync(string id)
        {
            try
            {
                // Since we don't know the partition key, we need to use a query
                var query = new QueryDefinition(
                    "SELECT * FROM c WHERE c.id = @id AND c.type = 'RecallOrder'")
                    .WithParameter("@id", id);

                var results = new List<RecallOrder>();
                var iterator = _container.GetItemQueryIterator<RecallOrder>(query);

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    results.AddRange(response);
                }

                return results.FirstOrDefault();
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
               // _logger.LogWarning("Recall order {RecallOrderId} not found", id);
                return null;
            }
            catch (CosmosException ex)
            {
              //  _logger.LogError(ex, "Error retrieving recall order {RecallOrderId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<RecallOrder>> GetByCustomerIdAsync(string customerId)
        {
            try
            {
                var query = new QueryDefinition(
                    "SELECT * FROM c WHERE c.customerId = @customerId AND c.type = 'RecallOrder' ORDER BY c.requestDate DESC")
                    .WithParameter("@customerId", customerId);

                var results = new List<RecallOrder>();
                var iterator = _container.GetItemQueryIterator<RecallOrder>(query);

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    results.AddRange(response);
                }

                return results;
            }
            catch (CosmosException ex)
            {
             //   _logger.LogError(ex, "Error retrieving recall orders for customer {CustomerId}", customerId);
                throw;
            }
        }

        public async Task UpdateAsync(RecallOrder recallOrder)
        {
            try
            {
                recallOrder.LastModified = DateTime.UtcNow;

                var response = await _container.ReplaceItemAsync(
                recallOrder,
                    recallOrder.Id,
                    new PartitionKey(recallOrder.PartitionKey));

              //  _logger.LogInformation("Updated recall order {RecallOrderId} with request charge {RequestCharge} RUs",
                  //  recallOrder.Id, response.RequestCharge);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
            //    _logger.LogWarning("Recall order {RecallOrderId} not found for update", recallOrder.Id);
                throw new KeyNotFoundException($"Recall order with ID {recallOrder.Id} not found");
            }
            catch (CosmosException ex)
            {
               // _logger.LogError(ex, "Error updating recall order {RecallOrderId}", recallOrder.Id);
                throw;
            }
        }
    }
}