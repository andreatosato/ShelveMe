using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Server.Entities;
using Server.Services;
using SharedModels;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ItemsFunctions
    {
        private readonly ITableStorageService tableStorageService;
        private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
        public ItemsFunctions(ITableStorageService tableStorageService)
        {
            this.tableStorageService = tableStorageService;
        }

        [Function("StorageInsert")]
        public async Task<HttpResponseData> InsertAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "StorageInsert")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StorageInsert");
            
            var item = await JsonSerializer.DeserializeAsync<ShelveItem>(req.Body, JsonOptions);
            logger.LogDebug("Storage to insert {@item}", item);

            var entity = ShelveItemEntity.FromClientEntity(item);
            var entityInserted = await tableStorageService.CreateEntityAsync(entity);

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(entityInserted);
            return response;
        }

        [Function("StorageUpdate")]
        public async Task<HttpResponseData> UpdateAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "StorageUpdate")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StorageUpdate");
            
            var item = await JsonSerializer.DeserializeAsync<ShelveItem>(req.Body, JsonOptions);
            logger.LogDebug("Storage to update {@item}", item);

            var entity = ShelveItemEntity.FromClientEntity(item);
            await tableStorageService.UpdateEntityAsync(entity);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(entity);
            return response;
        }

        [Function("StorageDelete")]
        public async Task<HttpResponseData> DeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "StorageDelete/{partitionKey}/{rowKey}")] HttpRequestData req,
            string partitionKey,
            string rowKey,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StorageDelete");
            
            await tableStorageService.DeleteEntityAsync(partitionKey, rowKey);

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [Function("StorageGet")]
        public async Task<HttpResponseData> GetAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "StorageGet")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StorageGet");
            
            var pageRequest = await JsonSerializer.DeserializeAsync<PageRequest>(req.Body, JsonOptions);
            logger.LogDebug("Storage to delete {@item}", pageRequest);

            var result = await tableStorageService.QueryAsync<ShelveItemEntity>(
                pageRequest.PartitionKey,
                pageRequest.Take, 
                pageRequest.ContinuationToken);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
