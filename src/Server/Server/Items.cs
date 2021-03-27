using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SharedModels;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server
{
    public class ItemsFunctions
    {
        [Function("StorageInsert")]
        public async Task<HttpResponseData> InsertAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "StorageInsert")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StorageInsert");
            
            var item = await JsonSerializer.DeserializeAsync<ShelveItem>(req.Body);
            logger.LogDebug("Storage to insert {@item}", item);

            




            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(item);
            return response;
        }
    }
}
