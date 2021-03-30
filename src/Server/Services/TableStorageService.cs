using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public class TableStorageService : ITableStorageService
    {
        private readonly TableClient client;
        private readonly ILogger<TableStorageService> logger;

        public TableStorageService(ILogger<TableStorageService> logger, IOptions<TableStorageOptions> options)
        {
            client = new TableClient(options.Value.StorageAccount, options.Value.TableName);
            client.CreateIfNotExists();
            this.logger = logger;
        }

        private async Task<T> GetElementByPrimaryKey<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default)
            where T: class, ITableEntity, new()
        {
            return await client.GetEntityAsync<T>(partitionKey, rowKey, cancellationToken: cancellationToken);
        }

        public async Task<T> CreateEntityAsync<T>(T entity, CancellationToken cancellationToken)
            where T: class, ITableEntity, new()
        {
            var response = await client.AddEntityAsync<T>(entity, cancellationToken: cancellationToken);
            if (!(response.Status >= 200 && response.Status <= 299))
            {
                logger.LogError(await new StreamReader(response.ContentStream).ReadToEndAsync());
                throw new Exception(response.ReasonPhrase);
            }
            return await GetElementByPrimaryKey<T>(entity.PartitionKey, entity.RowKey, cancellationToken);
        }

        public async Task UpdateEntityAsync<T>(T entity, CancellationToken cancellationToken)
            where T : class, ITableEntity, new()
        {
            var response = await client.UpsertEntityAsync<T>(entity, TableUpdateMode.Replace, cancellationToken: cancellationToken);
            if (!(response.Status >= 200 && response.Status <= 299))
            {
                logger.LogError(await new StreamReader(response.ContentStream).ReadToEndAsync());
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task DeleteEntityAsync(string partitionKey, string rowKey, CancellationToken cancellationToken)
        {
            var response = await client.DeleteEntityAsync(partitionKey, rowKey, cancellationToken: cancellationToken);
            if (!(response.Status >= 200 && response.Status <= 299))
            {
                logger.LogError(await new StreamReader(response.ContentStream).ReadToEndAsync());
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<Page<T>> QueryAsync<T>(string partitionKey, int take, string continuationTokenPage, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            Page<T> response = new();
            var tableResponse = client.QueryAsync<T>(t => t.PartitionKey == partitionKey, take == 0 ? null : take, cancellationToken: cancellationToken);
            await foreach (var page in tableResponse.AsPages(continuationTokenPage, take == 0 ? null : take))
            {
                response = new Page<T>
                {
                    Values = page.Values,
                    ContinuationToken = page.ContinuationToken
                };
                return response;
            }
            return null;
        }

        public async Task<List<Page<T>>> QueryAsync<T>(string partitionKey, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            List<Page<T>> response = new();
            var tableResponse = client.QueryAsync<T>(t => t.PartitionKey == partitionKey, cancellationToken: cancellationToken);
            await foreach (var page in tableResponse.AsPages())
            {
                response.Add(new Page<T>
                {
                    Values = page.Values,
                    ContinuationToken = page.ContinuationToken
                });                
            }
            return response;
        }
    }
}
