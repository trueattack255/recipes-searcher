using System;
using System.Collections.Generic;
using System.Linq;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class SearchService : ISearchService
    {
        private Dictionary<string, Recipe> _recipes;
        private Dictionary<string, List<string>> _ingredients;

        public double Percent { get; set; }

        public SearchService(IDataService dataService)
        {
            var recipeList = dataService.GetCollection();

            _recipes = recipeList.ToDictionary(it => it.Id);
            _ingredients = recipeList
                .SelectMany(it => it.Ingredients, (recipe, ing) => new
                {
                    recipeId = recipe.Id,
                    ingredient = ing.Name
                })
                .GroupBy(it => it.ingredient)
                .Select(it => new
                {
                    name = it.Key,
                    recipes = it.Select(x => x.recipeId)
                        .Distinct()
                        .ToList()
                })
                .ToDictionary(it => it.name, it => it.recipes);
        }

        public List<Ingredient> SearchIngredients(IEnumerable<string> collection)
        {
            return collection.Where(it => _ingredients.ContainsKey(it))
                .Select(it => new Ingredient { Name = it })
                .ToList();
        }

        public List<Recipe> SearchRecipes(IEnumerable<string> collection)
        {
            var buffer = new Dictionary<string, Recipe>(_recipes);

            foreach (var item in collection)
            {
                if (_ingredients.ContainsKey(item))
                {
                    foreach (var rcp in _ingredients[item])
                    {
                        foreach (var ing in buffer[rcp].Ingredients)
                        {
                            if (ing.Name.Equals(item))
                            {
                                ing.IsContain = true;
                                buffer[rcp].Concurrency++;
                            }
                        }
                    }
                }
            }

            var result = new List<Recipe>();

            foreach (var rcp in buffer.Values)
            {
                var percent = Math.Round(rcp.Concurrency / rcp.Ingredients.Length * 100, 1, MidpointRounding.AwayFromZero);

                rcp.Concurrency = percent;

                if (percent >= Percent)
                {
                    result.Add(rcp);
                }
            }

            return result.OrderByDescending(it => it.Concurrency)
                .ToList();
        }
    }
}
