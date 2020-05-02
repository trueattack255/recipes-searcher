using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mobile.Interfaces
{
    public interface IWebService
    {   
        string Site { get; }

        Task<JObject> Get(int id);
        Task<JObject> Get(int skip, int limit);
        Task<JObject> CheckReceipt(string code);
        Task<JObject> GetIngredients(string hash);
        Task<JObject> Search(string con, string ing);
    }
}
