using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Server.Entities;
using Server.Services;
using SharedModels;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server
{
    public class ItemsFunctions
    {
        private readonly ITableStorageService tableStorageService;

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
            
            var item = await JsonSerializer.DeserializeAsync<ShelveItem>(req.Body);
            logger.LogDebug("Storage to insert {@item}", item);

            var entity = ShelveItemEntity.FromClientEntity(item);
            await tableStorageService.CreateEntityAsync(entity);

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(entity);
            return response;
        }
    }
}
