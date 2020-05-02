using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class DataService : IDataService
    {
        private readonly RecipeContext _context;

        public DataService(IOptions<Settings> settings)
        {
            _context = new RecipeContext(settings);
        }

        public Recipe GetById(string id)
        {
            var filter = Builders<Recipe>.Filter.Eq("_id", ObjectId.Parse(id));
            return _context.Recipes
                .Find(filter)
                .FirstOrDefault();
        }

        public List<Recipe> GetByRange(int skip, int limit)
        {
            return _context.Recipes.Find(_ => true)
                .Limit(limit)
                .Skip(skip)
                .ToList();
        }

        public List<Recipe> GetCollection()
        {
            return _context.Recipes
                .Find(_ => true)
                .ToList();
        }
    }
}