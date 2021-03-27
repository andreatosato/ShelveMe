using Azure.Data.Tables;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ITableStorageService
    {
        Task CreateEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task UpdateEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task DeleteEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task<Page<T>> QueryAsync<T>(string partitionKey, string rowKey, int take, string continuationTokenPage = null, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new();
    }

    public class TableStorageOptions
    {
        public string StorageAccount { get; set; }
        public string TableName { get; set; }
    }
}
