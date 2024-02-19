using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using static System.Net.WebRequestMethods;

namespace ZestFrontend.Services
{
    public class AccountService
    {
        HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> CreateAccount(string firstName, string lastName, string username, string email, string password, DateTime birthdate)
        {
            var content = new AccountDTO{ FirstName = firstName, LastName = lastName, Username = username, Email = email, Birthdate = birthdate, Password = password,CreatedOn1 = DateTime.Now };
            var url = $"https://localhost:7183/api/Account/add";
			var body = JsonConvert.SerializeObject(content);
			var response = await _httpClient.PostAsJsonAsync(url, content);
			response.EnsureSuccessStatusCode();
			return response;
		}

        public async Task<AccountDTO> GetCurrentAccount(int id)
        {
            var url = $"https://localhost:7183/api/Account/{id}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AccountDTO>();
            }
            else
                return null;
        }
		public async Task<List<UserDTO>> GetAllAccounts(int accountId)
		{
			var url = $"https://localhost:7183/api/Account/getAll/{accountId}";
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
