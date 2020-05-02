using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Interfaces
{
    interface IRecipesContract
    {
        Task<ActionResult<Recipe>> GetById(string id);
        Task<ActionResult<List<Recipe>>> GetByRange(int skip, int limit);
        Task<ActionResult<List<Ingredient>>> GetIngredients(string id);
        Task<ActionResult<Note>> CheckReceipt(string t, string s, string fn, string i, string fp);
        Task<ActionResult<List<Recipe>>> Search(string con, string ing);
    }
}
