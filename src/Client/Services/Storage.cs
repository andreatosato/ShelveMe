using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

        public Task DeleteShelveItemAsync(ShelveItem item)
        {
            throw new NotImplementedException();
        }

        public async Task InsertShelveItemAsync(ShelveItem item)
        {
            await storageClient.PostAsJsonAsync("StorageInsert", item);
        }

        public Task<List<ShelveItem>> ReadShelveItemsAsync(ShelveItem item, int take, int skip)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShelveItemAsync(ShelveItem item)
        {
            throw new NotImplementedException();
        }
    }
}
