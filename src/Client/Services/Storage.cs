using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class Storage : IStorage
    {
        private readonly HttpClient storageClient;
        public Storage(HttpClient client)
        {
            storageClient = client;
        }
        public ShelveItem EditingCache {get; set;}

        public Task DeleteShelveItemAsync(ShelveItem item)
        {
            return storageClient.DeleteAsync($"StorageDelete/{item.PartitionKey}/{item.RowKey}");
        }

        public Task InsertShelveItemAsync(ShelveItem item)
        {
            return storageClient.PostAsJsonAsync("StorageInsert", item);
        }

        public async Task<Page<ShelveItem>> ReadShelveItemsAsync(string username, int? take = null, string? continuationTokenReader = null)
        {
            var response = await storageClient.PostAsJsonAsync("StorageGet", new PageRequest {
                PartitionKey = username,
                Take = take ?? 0,
                ContinuationToken = continuationTokenReader
            });
            
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            if(stream.Length == 0)
                return null;

            return await JsonSerializer.DeserializeAsync<Page<ShelveItem>>(await response.Content.ReadAsStreamAsync());
        }

        public Task UpdateShelveItemAsync(ShelveItem item)
        {
            return storageClient.PutAsJsonAsync("StorageUpdate", item);
        }
    }
}
