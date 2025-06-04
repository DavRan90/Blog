using System.Text.Json;

namespace Blog.DAL
{
    public class ElementAPIManager
    {
        private static Uri BaseAddress = new Uri("https://localhost:7195/");
        //private static Uri BaseAddress = new Uri("https://davidkassabokenapi-g6ayfwgdhef4amc7.northeurope-01.azurewebsites.net");

        public static async Task<List<Models.Element>> GetAllElements()
        {
            List<Models.Element> elements = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Element");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    elements = JsonSerializer.Deserialize<List<Models.Element>>(responseString);
                }
            }


            return elements;
        }

        public static async Task<Models.Element> GetElement(int id)
        {
            Models.Element element = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync($"api/Element/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    element = JsonSerializer.Deserialize<Models.Element>(responseString);
                }
            }


            return element;
        }

        public static async Task AddElement(Models.Element element)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(element);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/Element", httpContent);

            }
        }

        public static async Task UpdateElement(Models.Element element)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(element);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"api/Element/{element.Id}", httpContent);

            }
        }

        public static async Task DeleteElement(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage response = await client.DeleteAsync($"api/Element/{id}");

            }
        }
    }
}
