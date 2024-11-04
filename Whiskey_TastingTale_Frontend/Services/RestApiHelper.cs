using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Whiskey_TastingTale_Frontend.Services
{
    public class RestApiHelper
    {
        private readonly UserState _userState; 
        public string server_uri; 

        private readonly IConfiguration _configuration;


        public RestApiHelper(UserState userState, IConfiguration configuration)
        {
            _userState = userState;
            _configuration = configuration;
            server_uri = _configuration["BackendSettings:ServerUrl"];
        }

        private HttpClient GetHttpClient()
        {
            var token = _userState.Token ?? "";

            var client = new HttpClient();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<object> Delete(string url)
        {
            object result = null;
            try
            {
                using (var client = GetHttpClient())
                {
                    var response = await client.DeleteAsync(url);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                result = $"[ERR] Request failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                result = $"[ERR] Unexpected error: {ex.Message}";
            }
            return result;
        }
        public async Task<object> Put(string url, object data)
        {
            object result = null;
            try
            {
                using (var client = GetHttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                result = $"[ERR] Request failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                result = $"[ERR] Unexpected error: {ex.Message}";
            }
            return result;
        }
        public async Task<object> Post(string url, object data)
        {
            object result = null;

            try
            {
                using (var client = GetHttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                result = $"[ERR] Request failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                result = $"[ERR] Unexpected error: {ex.Message}";
            }
            return result;
        }

        public async Task<object> PostImage(string url, MultipartFormDataContent content)
        {
            object result = null;

            try
            {
                using (var client = GetHttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                result = $"[ERR] Request failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                result = $"[ERR] Unexpected error: {ex.Message}";
            }

            return result;
        }

        public async Task<object> Get(string url)
        {
            object result = null;

            try
            {
                using (var client = GetHttpClient())
                {
                    var response = await client.GetAsync(url);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                result = $"[ERR] Request failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                result = $"[ERR] Unexpected error: {ex.Message}";
            }

            return result;
        }

    }
}
