using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Models
{
    public class RecipeContext
    {
        private readonly IMongoDatabase _database;

        public RecipeContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Recipe> Recipes => _database.GetCollection<Recipe>("recipes");
    }
}