
using Auth0.ManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class AccountService
    {
        HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<string[]> CreateAccount(string accessToken, string name, string email)
        {
           
            var url = $"{PortConst.Port_Forward_Http}/api/Account/add/{name}/{email}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var response = await _httpClient.PostAsJsonAsync(url, new StringContent("data"));
			response.EnsureSuccessStatusCode();
          
			return JsonSerializer.Deserialize<string[]>(await response.Content.ReadAsStringAsync());
		}

        public async Task<AccountDTO> GetCurrentAccount(string accessToken)
        {
            var url = $"{PortConst.Port_Forward_Http}/api/Account/get";
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
		public async Task<List<UserDTO>> GetAllAccounts()
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Account/getAll";
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<UserDTO>>();
			}
			else
				return null;
		}
	}
}
