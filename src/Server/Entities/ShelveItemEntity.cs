using Azure;
using Azure.Data.Tables;
using SharedModels;
using System;

namespace Server.Entities
{
    public class ShelveItemEntity : ITableEntity
    {
        #region TableEntity
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        #endregion

        public string Name { get; init; }

        public decimal Quantity { get; init; }

        public DateTime? ExpirationTime { get; init; }

        public static ShelveItemEntity FromClientEntity(IClientEntity entity)
        {
            var client = entity as ShelveItem;
            return new ShelveItemEntity
            {
                PartitionKey = client.PartitionKey,
                RowKey = client.RowKey,
                Name = client.Name,
                Quantity = client.Quantity,
                ExpirationTime = client.ExpirationTime
            };
        }
    }
}
