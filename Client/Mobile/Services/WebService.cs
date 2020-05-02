using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mobile.Interfaces;
using Newtonsoft.Json.Linq;

namespace Mobile.Services
{
    public class WebService : IWebService
    {
        public string Site { get; }
        private readonly HttpClient _client;

        private Func<string, JObject> GetJError => (msg) => JObject.FromObject(new { error = new { error_msg = msg } });

        public WebService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.Timeout = TimeSpan.FromSeconds(60);

            Site = "http://localhost:8080";
        }

        public async Task<JObject> Get(int id)
        {
            return await GetJObjectAsync("/api/recipes/getbyid?id=" + id);
        }

        public async Task<JObject> Get(int skip, int limit)
        {
            return await GetJObjectAsync("/api/recipes/getbyrange?skip=" + skip + "&limit=" + limit);
        }

        public async Task<JObject> CheckReceipt(string code)
        {
            return await GetJObjectAsync("/api/recipes/checkreceipt?" + code);
        }

        public async Task<JObject> GetIngredients(string hash)
        {
            return await GetJObjectAsync("/api/recipes/getingredients?id=" + hash);
        }

        public async Task<JObject> Search(string con, string ing)
        {
            return await GetJObjectAsync("/api/recipes/search?con=" + con + "&ing=" + ing);
        }

        private async Task<JObject> GetJObjectAsync(string query)
        {
            try
            {
                var response = await _client.GetAsync(Site + query);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.BadRequest: break;
                    default: return GetJError("Не удалось получить ответ от сервера");
                }

                var content = await response.Content.ReadAsStringAsync();
                return JObject.Parse(content);
            }
            catch (Exception)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                return GetJError("Ошибка подключения к серверу");
            }
        }
    }
}
