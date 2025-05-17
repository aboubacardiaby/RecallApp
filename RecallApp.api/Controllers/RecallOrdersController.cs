using Microsoft.AspNetCore.Mvc;
using RecallApp.Core.Models;
using RecallApp.Core.Services;


namespace RecallApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecallOrdersController : ControllerBase
{
    private readonly IRecallOrderService _recallOrderService;
    private readonly ILogger<RecallOrdersController> _logger;

    public RecallOrdersController(
        IRecallOrderService recallOrderService,
        ILogger<RecallOrdersController> logger)
    {
        _recallOrderService = recallOrderService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecallOrder(RecallOrder recallOrder)
    {
        try
        {
            _logger.LogInformation("Received recall order request with {ItemCount} items", recallOrder.Items.Count);

            // Validate the request
            if (recallOrder.Items.Count == 0)
            {
                return BadRequest("Recall order must contain at least one item");
            }

            // Process the request
            var result = await _recallOrderService.CreateRecallOrderAsync(recallOrder);

            return CreatedAtAction(nameof(GetRecallOrder), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating recall order");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecallOrder(string id)
    {
        try
        {
            var order = await _recallOrderService.GetRecallOrderAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recall order {RecallOrderId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCustomerRecallOrders(string customerId)
    {
        try
        {
            var orders = await _recallOrderService.GetCustomerRecallOrdersAsync(customerId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recall orders for customer {CustomerId}", customerId);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}
