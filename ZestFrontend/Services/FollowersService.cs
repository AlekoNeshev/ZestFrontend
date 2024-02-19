using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
	public class FollowersService
	{
        HttpClient _httpClient;
        public FollowersService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
		public async Task<HttpResponseMessage> Follow(int followerId, int followedId)
		{
			var url = $"https://localhost:7183/api/Followers/add/{followerId}/followed/{followedId}";
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> Unfollow(int followerId, int followedId)
		{
			var url = $"https://localhost:7183/api/Followers/delete/{followerId}/followed/{followedId}";
			var response = await _httpClient.DeleteAsync(url);
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<FollowerDTO[]> GetFriends(int id)
		{
			var url = $"https://localhost:7183/api/Followers/account/{id}";
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<FollowerDTO[]>();
			}
			else
				return null;
		}
	}
}
