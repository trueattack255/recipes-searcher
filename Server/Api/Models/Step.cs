using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Api.Models
{
    public class Step
    {
        [BsonElement("no")]
        [JsonProperty("no")]
        public int No { get; set; }
        [BsonElement("info")]
        [JsonProperty("info")]
        public string Info { get; set; }
    }
}