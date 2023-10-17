﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class LoginService
    {
        HttpClient _httpClient;
        public LoginService (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       public async Task<AccountDTO> GetAccount(string username, string password)
        {
            var url = $"https://localhost:7183/api/Account/email/{username}/password/{password}";
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