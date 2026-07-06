using Newtonsoft.Json;
using System.Text;

namespace RestaurantManagerMVC.Helpers
{
    public static class ApiContext
    {
        private static IHttpClientFactory _factory = null!;

        public static void Configure(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private static HttpClient Client => _factory.CreateClient("RestaurantApi");

        public static T? Get<T>(string url)
        {
            try
            {
                var response = Client.GetAsync(url).Result;

                var json = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Status: {(int)response.StatusCode} - {json}");

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("API GET HATA DETAY: " + ex.ToString());
            }
        }

        public static (bool Success, string? Error) Post<T>(string url, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = Client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode) return (true, null);
            return (false, response.Content.ReadAsStringAsync().Result);
        }

        public static (bool Success, string? Error) Put<T>(string url, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = Client.PutAsync(url, content).Result;

            if (response.IsSuccessStatusCode) return (true, null);
            return (false, response.Content.ReadAsStringAsync().Result);
        }

        public static (bool Success, string? Error) Delete(string url)
        {
            var response = Client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, response.Content.ReadAsStringAsync().Result);
        }
    }
}
