using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
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
    }
}
