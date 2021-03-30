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
        Task<T> CreateEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task UpdateEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task DeleteEntityAsync(string partitionKey, string rowKey, CancellationToken cancellationToken = default);
        Task<Page<T>> QueryAsync<T>(string partitionKey, int take, string continuationTokenPage, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new();
        
        Task<List<Page<T>>> QueryAsync<T>(string partitionKey, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new();
    }

    public class TableStorageOptions
    {
        public string StorageAccount { get; set; }
        public string TableName { get; set; }
    }
}
