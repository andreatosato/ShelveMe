using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IStorage
    {
        Task<List<ShelveItem>> ReadShelveItemsAsync(ShelveItem item, int take, int skip);
        Task InsertShelveItemAsync(ShelveItem item);
        Task UpdateShelveItemAsync(ShelveItem item);
        Task DeleteShelveItemAsync(ShelveItem item);
    }
}
