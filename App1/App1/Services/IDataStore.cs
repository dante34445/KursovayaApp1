using App1.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IDataStore<T, U>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(U id);
        Task<T> GetItemAsync(U id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
