using Newtonsoft.Json;
using System.Text;

namespace Whiskey_TastingTale_Frontend.Services
{
    public static class RestApiHelper
    {
        public static string server_uri = "https://localhost:7299/";
        public static async Task<object> Delete(string url)
        {
            object result = null;
            Exception badEx = null;

            using (var client = new HttpClient())
            {
                string apiUrl = url;
                bool badStatus = false;
                string badMessage = "";

                //TODO : url 이랑 연결 안되면 죽는 이슈 
                var response = await client.DeleteAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    badStatus = true;
                    badMessage = $"{response.StatusCode}";
                }
                return await response.Content.ReadAsStringAsync();
            }

            if (badEx != null) throw badEx;

            return result;
        }
        public static async Task<object> Put(string url, object data)
        {
            object result = null;
            Exception badEx = null;

            using (var client = new HttpClient())
            {
                string apiUrl = url;
                bool badStatus = false;
                string badMessage = "";

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                var test = JsonConvert.SerializeObject(data);

                //TODO : url 이랑 연결 안되면 죽는 이슈 
                var response = await client.PutAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    badStatus = true;
                    badMessage = $"{response.StatusCode}";
                }
                return await response.Content.ReadAsStringAsync();
            }

            if (badEx != null) throw badEx;

            return result;
        }
        public static async Task<object> Post(string url, object data)
        {
            object result = null;
            Exception badEx = null;

            using (var client = new HttpClient())
            {
                string apiUrl = url;
                bool badStatus = false;
                string badMessage = "";

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                var test = JsonConvert.SerializeObject(data);

                //TODO : url 이랑 연결 안되면 죽는 이슈 
                var response = await client.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    badStatus = true;
                    badMessage = $"{response.StatusCode}";
                }
                return await response.Content.ReadAsStringAsync();
            }

            if (badEx != null) throw badEx;

            return result;
        }

        public static async Task<object> Get(string url)
        {
            object result = null;
            Exception badEx = null;

            using (var client = new HttpClient())
            {
                string apiUrl = url;
                bool badStatus = false;
                string badMessage = "";

                //TODO : url 이랑 연결 안되면 죽는 이슈 
                var response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    badStatus = true;
                    badMessage = $"{response.StatusCode}";
                }
                return await response.Content.ReadAsStringAsync();
            }

            if (badEx != null) throw badEx;

            return result;
        }

    }
}
