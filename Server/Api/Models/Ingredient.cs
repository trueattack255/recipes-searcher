using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Api.Models
{
    public class Ingredient
    {
        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
        [BsonElement("amount")]
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [BsonElement("unit_measure")]
        [JsonProperty("unit_measure")]
        public string UnitMeasure { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        [BsonDefaultValue(false)]
        [BsonElement("contained")]
        [JsonProperty("contained")]
        public bool IsContain { get; set; }
    }
}