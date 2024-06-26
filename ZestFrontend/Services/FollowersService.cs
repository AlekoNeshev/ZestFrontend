﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.Constants;
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

			var url = $"{PortConst.Port_Forward_Http}/Zest/Followers/add/followed/{followedId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> Unfollow(string followedId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Followers/delete/followed/{followedId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.DeleteAsync(url);
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<FollowerDTO[]> GetFriends(int takeCount, int skipCount)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Followers/getFriends/{takeCount}/{skipCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<FollowerDTO[]>();
			}
			else
				return null;
		}
		public async Task<List<FollowerDTO>> GetAccountsBySearch(string text, int takeCount, string[] skipIds = null)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Followers/getBySearch/{text}/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(skipIds);

			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<FollowerDTO>>();
			}
			else
				return null;
		}
	}
}
