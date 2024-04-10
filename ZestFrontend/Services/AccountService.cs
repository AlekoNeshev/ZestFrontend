using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ZestFrontend.Constants;
using ZestFrontend.DTOs;
using static System.Net.Mime.MediaTypeNames;

namespace ZestFrontend.Services
{
    public class AccountService
    {
        HttpClient _httpClient;
        public AccountService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<AccountDTO> CreateAccount(string accessToken, string name, string email)
        {
           
            var url = $"{PortConst.Port_Forward_Http}/Zest/Account/add/{name}/{email}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			if (response.IsSuccessStatusCode)
			{
				if (response.Content != null && response.Content.Headers.ContentLength > 0)
				{
					return await response.Content.ReadFromJsonAsync<AccountDTO>();
				}
				else { return null; }

			}
			else
				return null;
		}

        public async Task<AccountDTO> GetCurrentAccount(string accessToken)
        {
            var url = $"{PortConst.Port_Forward_Http}/Zest/Account/get";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null && response.Content.Headers.ContentLength > 0)
                {
					return await response.Content.ReadFromJsonAsync<AccountDTO>();
				}
				else { return null; }
              
            }
            else
                return null;
        }
		public async Task<List<UserDTO>> GetAllAccounts(int takeCount, int skipCount)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Account/getAll/{takeCount}/{skipCount}";
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<UserDTO>>();
			}
			else
				return null;
		}
		public async Task<List<UserDTO>> GetAccountsBySearch(string text, string accessToken, int takeCount, string[] skipIds = null)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Account/getBySearch/{text}/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var body = Newtonsoft.Json.JsonConvert.SerializeObject(skipIds);

			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<UserDTO>>();
			}
			else
				return null;
		}
		
	}
}
