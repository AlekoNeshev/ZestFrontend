using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
	public class MessageService
	{
        HttpClient _httpClient;
        public MessageService(HttpClient httpClient)
        {
             this._httpClient = httpClient;
        }
		public async Task<MessageDTO> FindById(int id)
		{
			var url = $"https://localhost:7183/api/Messages/get/{id}";
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<MessageDTO>();
			}
			else
				return null;
		}
		public async Task<MessageDTO[]> GetMessages(int senderId, int receiverId)
		{
			var url = $"https://localhost:7183/api/Messages/get/{senderId}/receiver/{receiverId}";
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<MessageDTO[]>();
			}
			else
				return null;
		}
		public async Task<HttpResponseMessage> SendMessage(int senderId, int receiverId, string text)
		{
			var url = $"https://localhost:7183/api/Messages/add/{senderId}/receiver/{receiverId}";
			var body = JsonConvert.SerializeObject(text);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
	}
}
