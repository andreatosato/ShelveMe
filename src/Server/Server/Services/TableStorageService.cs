﻿using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using SharedModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public class TableStorageService
    {
        private readonly TableClient client;
        private readonly ILogger<TableStorageService> logger;

        public TableStorageService(ILogger<TableStorageService> logger, string connectionString, string tableName)
        {
            client = new TableClient(connectionString, tableName);
            client.CreateIfNotExists();
            this.logger = logger;
        }

        public async Task CreateEntity<T>(T entity, CancellationToken cancellationToken)
            where T: class, ITableEntity, new()
        {
            var response = await client.AddEntityAsync<T>(entity, cancellationToken: cancellationToken);
            if (response.Status != 200 && response.Status != 201)
            {
                logger.LogError(await new StreamReader(response.ContentStream).ReadToEndAsync());
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task UpdateEntity<T>(T entity, CancellationToken cancellationToken)
            where T : class, ITableEntity, new()
        {
            var response = await client.UpsertEntityAsync<T>(entity, TableUpdateMode.Replace, cancellationToken: cancellationToken);
            if (response.Status != 200 && response.Status != 201)
            {
                logger.LogError(await new StreamReader(response.ContentStream).ReadToEndAsync());
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task DeleteEntity<T>(T entity, CancellationToken cancellationToken)
            where T : class, ITableEntity, new()
        {
            var response = await client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, cancellationToken: cancellationToken);
            if (response.Status != 200 && response.Status != 201)
            {
                logger.LogError(await new StreamReader(response.ContentStream).ReadToEndAsync());
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<Page<T>> QueryAsync<T>(string partitionKey, string rowKey, int take, string continuationTokenPage = null, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            Page<T> response = new();
            var tableResponse = client.QueryAsync<T>(t => t.PartitionKey == partitionKey && t.RowKey == rowKey, maxPerPage: take, cancellationToken: cancellationToken);
            await foreach (var page in tableResponse.AsPages(continuationTokenPage, take))
            {
                response = new Page<T> 
                {
                    Values = page.Values,
                    ContinuationToken = page.ContinuationToken 
                };
            }
            return response;
        }
    }
}