using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class CommunityService
    {
        HttpClient _httpClient;
        public CommunityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CommunityDTO>> GetCommunities()
        {
            var url = $"https://localhost:7183/api/Community/getAll";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityDTO>>();
            }
            else
                return null;
        }
    }
}
