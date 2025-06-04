using System.Text.Json;

namespace Blog.DAL
{
    public class SiteAPIManager
    {
        private static Uri BaseAddress = new Uri("https://localhost:7195/");
        //private static Uri BaseAddress = new Uri("https://davidkassabokenapi-g6ayfwgdhef4amc7.northeurope-01.azurewebsites.net");

        public static async Task<List<Models.Site>> GetAllSites()
        {
            List<Models.Site> sites = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Sites");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    sites = JsonSerializer.Deserialize<List<Models.Site>>(responseString);
                }
            }


            return sites;
        }

        public static async Task<Models.Site> GetSite(int id)
        {
            Models.Site sites = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync($"api/Sites/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    sites = JsonSerializer.Deserialize<Models.Site>(responseString);
                }
            }


            return sites;
        }

        public static async Task AddSite(Models.Site site)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(site);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/Sites", httpContent);

            }
        }

        public static async Task UpdateSite(Models.Site site)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(site);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"api/Sites/{site.Id}", httpContent);

            }
        }

        public static async Task DeleteSite(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage response = await client.DeleteAsync($"api/Sites/{id}");

            }
        }
    }
}
