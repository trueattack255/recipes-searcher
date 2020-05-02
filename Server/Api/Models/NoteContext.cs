using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Models
{
    public class NoteContext
    {
        private readonly IMongoDatabase _database;

        public NoteContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Note> Errors => _database.GetCollection<Note>("errors");
        public IMongoCollection<Note> Receipts => _database.GetCollection<Note>("receipts");
    }
}