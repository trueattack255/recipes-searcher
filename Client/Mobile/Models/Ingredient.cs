using Newtonsoft.Json;

namespace Mobile.Models
{
    public class Ingredient
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("unit_measure")]
        public string UnitMeasure { get; set; }

        [JsonProperty("contained")]
        public bool IsContain { get; set; }
    }
}
