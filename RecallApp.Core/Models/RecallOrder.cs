// RecallApp.Core/Models/RecallOrder.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecallApp.Core.Models
{
    /// <summary>
    /// Represents a bulk recall order request submitted by a customer
    /// </summary>
    public class RecallOrder
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; } = string.Empty;

        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("status")]
        public RecallOrderStatus Status { get; set; } = RecallOrderStatus.Submitted;

        [JsonPropertyName("items")]
        public List<RecallOrderItem> Items { get; set; } = new List<RecallOrderItem>();

        [JsonPropertyName("totalItems")]
        public int TotalItems => Items.Count;

        [JsonPropertyName("lastModified")]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("type")]
        public string Type { get; set; } = "RecallOrder";

        // For Cosmos DB partitioning - ensure this property always has a value
        [JsonPropertyName("partitionKey")]
        public string PartitionKey => CustomerId ?? Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Represents the status of a recall order
    /// </summary>
    public enum RecallOrderStatus
    {
        Submitted,
        Processing,
        Completed,
        Failed
    }

    /// <summary>
    /// Represents an individual item in a recall order request
    /// </summary>
    public class RecallOrderItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("productId")]
        public string ProductId { get; set; } = string.Empty;

        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public RecallItemStatus Status { get; set; } = RecallItemStatus.Pending;
    }

    /// <summary>
    /// Represents the status of an individual recall item
    /// </summary>
    public enum RecallItemStatus
    {
        Pending,
        Processing,
        Approved,
        Rejected,
        Completed
    }

    /// <summary>
    /// Represents an individual order decomposed from a bulk recall request
    /// This will be sent to Service Bus for processing
    /// </summary>
    public class IndividualRecallOrder
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("parentOrderId")]
        public string ParentOrderId { get; set; } = string.Empty;

        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; } = string.Empty;

        [JsonPropertyName("productId")]
        public string ProductId { get; set; } = string.Empty;

        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("status")]
        public RecallItemStatus Status { get; set; } = RecallItemStatus.Pending;
    }
}