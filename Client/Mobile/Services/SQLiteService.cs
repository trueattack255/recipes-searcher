using System.Collections.Generic;
using System.Threading.Tasks;
using Mobile.Interfaces;
using Mobile.Models;
using SQLite;
using Xamarin.Forms;

namespace Mobile.Services
{
    public class SQLiteService : ISQLiteService
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly string _fileName = "history.db";

        public SQLiteService()
        {
            string databasePath = DependencyService
                .Get<ISQLite>()
                .GetDatabasePath(_fileName);

            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<Receipt>();
        }

        public async Task<List<Receipt>> GetItemsAsync()
        {
            return await _database
                .Table<Receipt>()
                .ToListAsync();
        }

        public async Task<Receipt> GetItemAsync(int id)
        {
            return await _database.GetAsync<Receipt>(id);
        }

        public async Task<Receipt> GetItemAsync(string hash)
        {
            //var result = await _database.GetAsync<Receipt>(it => it.Hash == hash);
            return await _database
                .Table<Receipt>()
                .FirstOrDefaultAsync(it => it.Hash == hash);
        }

        public async Task<int> DeleteItemAsync(Receipt item)
        {
            return await _database.DeleteAsync(item);
        }

        public async Task<int> SaveItemAsync(Receipt item)
        {
            if (item.Id != 0)
            {
                await _database.UpdateAsync(item);
                return item.Id;
            }

            return await _database.InsertAsync(item);
        }
    }
}
