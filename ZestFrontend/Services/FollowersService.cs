using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
	public class FollowersService
	{
        HttpClient _httpClient;
		AuthService _authService;
        public FollowersService(HttpClient httpClient, AuthService authService )
        {
            this._httpClient = httpClient;
			this._authService = authService;
        }
		public async Task<HttpResponseMessage> Follow( string followedId)
		{

			var url = $"{PortConst.Port_Forward_Http}/api/Followers/add/followed/{followedId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> Unfollow(string followedId)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Followers/delete/followed/{followedId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.DeleteAsync(url);
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<FollowerDTO[]> GetFriends()
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Followers/getFriends";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
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
