using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace Api.Models
{
    public class Note
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("query")]
        [JsonProperty("query")]
        public string Body { get; set; } = string.Empty;
        [BsonElement("code")]
        [JsonProperty("code")]
        public string Code { get; set; }
        [BsonElement("date")]
        [JsonProperty("date")]
        public string Date { get; set; } = $"{DateTime.Now:G}";
    }
}