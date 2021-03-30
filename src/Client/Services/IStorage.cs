using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IStorage
    {
        Task<Page<ShelveItem>> ReadShelveItemsAsync(string username, int? take = null, string? continuationTokenReader = null);
        Task InsertShelveItemAsync(ShelveItem item);
        Task UpdateShelveItemAsync(ShelveItem item);
        Task DeleteShelveItemAsync(ShelveItem item);
        ShelveItem EditingCache {get; set;}
    }
}
