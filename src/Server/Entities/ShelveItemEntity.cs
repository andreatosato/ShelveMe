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

        public string Name { get; set; }

        public double Quantity { get; set; }

        public DateTime? ExpirationTime { get; set; }

        public static ShelveItemEntity FromClientEntity(IClientEntity entity)
        {
            var client = entity as ShelveItem;
            return new ShelveItemEntity
            {
                PartitionKey = client.PartitionKey,
                RowKey = string.IsNullOrEmpty(entity.RowKey) ? Guid.NewGuid().ToString("N") : entity.RowKey,
                Name = client.Name,
                Quantity = (double)client.Quantity,
                ExpirationTime = client.ExpirationTime
            };
        }
    }
}
