using Newtonsoft.Json;
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
	public class MessageService
	{
        HttpClient _httpClient;
		AuthService _authService;
        public MessageService(HttpClient httpClient, AuthService authService)
        {
             this._httpClient = httpClient;
			this._authService = authService;
        }
		public async Task<MessageDTO> FindById(int id)
		{
			var url = $"https://localhost:7183/api/Messages/get/{id}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<MessageDTO>();
			}
			else
				return null;
		}
		public async Task<MessageDTO[]> GetMessages( string receiverId)
		{
			var url = $"https://localhost:7183/api/Messages/get/receiver/{receiverId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<MessageDTO[]>();
			}
			else
				return null;
		}
		public async Task<HttpResponseMessage> SendMessage( string receiverId, string text)
		{
			var url = $"https://localhost:7183/api/Messages/add/receiver/{receiverId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(text);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
	}
}
