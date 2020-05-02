using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface IDataService
    {
        Recipe GetById(string id);
        List<Recipe> GetByRange(int skip, int limit);
        List<Recipe> GetCollection();
    }
}
