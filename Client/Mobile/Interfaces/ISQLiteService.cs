using System.Threading.Tasks;
using System.Collections.Generic;
using Mobile.Models;

namespace Mobile.Interfaces
{
    public interface ISQLiteService
    {
        Task<List<Receipt>> GetItemsAsync();
        Task<Receipt> GetItemAsync(int id);
        Task<Receipt> GetItemAsync(string hash);
        Task<int> DeleteItemAsync(Receipt item);
        Task<int> SaveItemAsync(Receipt item);
    }
}
