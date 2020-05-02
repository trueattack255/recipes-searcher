using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Api.Models
{
    public class Recipe
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("title")]
        [JsonProperty("title")]
        public string Title { get; set; }
        [BsonElement("photo")]
        [JsonProperty("photo")]
        public string Photo { get; set; }
        [BsonElement("category")]
        [JsonProperty("category")]
        public string Category { get; set; }

        [BsonIgnore]
        [BsonRepresentation(BsonType.Double)]
        [BsonElement("concurrency")]
        [JsonProperty("concurrency")]
        public double Concurrency { get; set; }

        [BsonElement("recipe_steps")]
        [JsonProperty("recipe_steps")]
        public Step[] RecipeSteps { get; set; }
        [BsonElement("ingredients")]
        [JsonProperty("ingredients")]
        public Ingredient[] Ingredients { get; set; }
    }
}