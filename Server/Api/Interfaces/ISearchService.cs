using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface ISearchService
    {
        double Percent { get; set; }

        List<Recipe> SearchRecipes(IEnumerable<string> collection);
        List<Ingredient> SearchIngredients(IEnumerable<string> collection);
    }
}
