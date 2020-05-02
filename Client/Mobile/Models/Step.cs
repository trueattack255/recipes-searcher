using Newtonsoft.Json;

namespace Mobile.Models
{
    public class Step
    {
        [JsonProperty("no")]
        public int No { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
    }
}