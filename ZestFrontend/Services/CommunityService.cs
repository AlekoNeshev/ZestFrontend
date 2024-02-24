
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public async Task<List<CommunityDTO>> GetCommunities(int accountId)
        {
            var url = $"https://localhost:7183/api/Community/getAll/{accountId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityDTO>>();
            }
            else
                return null;
        }
        public async Task<HttpResponseMessage> Follow(int accountId, int communityId)       
        {
			var url = $"https://localhost:7183/api/CommunityFollowers/account/add/{accountId}/community/{communityId}";
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			
			return response;
		}
		public async Task<HttpResponseMessage> Unfollow(int accountId, int communityId)
		{
			var url = $"https://localhost:7183/api/CommunityFollowers/account/delete/{accountId}/community/{communityId}";
			var response = await _httpClient.DeleteAsync(url);

			return response;
		}
		public async Task<HttpResponseMessage> IsSubscribed(int accountId, int communityId)
		{
			var url = $"https://localhost:7183/api/CommunityFollowers/account/add/{accountId}/community/{communityId}";
			var response = await _httpClient.PostAsync(url, new StringContent("data"));

			return response;
		}
        public async Task<HttpResponseMessage> AddCommunity(string name,int creatorId ,string description)
        {
			var url = $"https://localhost:7183/api/Community/add/{name}/creator/{creatorId}";
			var body = JsonConvert.SerializeObject(description);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
	}
}
