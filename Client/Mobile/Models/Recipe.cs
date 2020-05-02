using Newtonsoft.Json;

namespace Mobile.Models
{
    public class Recipe
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("photo")]
        public string Photo { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("concurrency")]
        public double Concurrency { get; set; }

        [JsonProperty("recipe_steps")]
        public Step[] RecipeSteps { get; set; }
        [JsonProperty("ingredients")]
        public Ingredient[] Ingredients { get; set; }
    }
}
