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
    public interface ITableStorageService<T>
        where T: class, ITableEntity, new()
    {
        Task CreateEntity(T entity, CancellationToken cancellationToken);
        Task UpdateEntity(T entity, CancellationToken cancellationToken);
        Task DeleteEntity(T entity, CancellationToken cancellationToken);
        Task<Page<T>> QueryAsync(string partitionKey, string rowKey, int take, string continuationTokenPage = null, CancellationToken cancellationToken = default);
    }
}
