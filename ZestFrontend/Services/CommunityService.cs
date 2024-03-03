
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class CommunityService
    {
        HttpClient _httpClient;
		AuthService _authService;
        public CommunityService(HttpClient httpClient )
        {
            _httpClient = httpClient;
			_authService = AuthService.Instance;
        }

        public async Task<List<CommunityDTO>> GetCommunities()
        {
            var url = $"https://localhost:7183/api/Community/getAll";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityDTO>>();
            }
            else
                return null;
        }
        public async Task<HttpResponseMessage> Follow(int communityId)       
        {
			var url = $"https://localhost:7183/api/CommunityFollowers/account/add/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			
			return response;
		}
		public async Task<HttpResponseMessage> Unfollow(int communityId)
		{
			var url = $"https://localhost:7183/api/CommunityFollowers/account/delete/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.DeleteAsync(url);

			return response;
		}
		public async Task<HttpResponseMessage> IsSubscribed(int communityId)
		{
			var url = $"https://localhost:7183/api/CommunityFollowers/account/add/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));

			return response;
		}
        public async Task<HttpResponseMessage> AddCommunity(string name, string description)
        {
			var url = $"https://localhost:7183/api/Community/add/{name}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(description);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
	}
}
