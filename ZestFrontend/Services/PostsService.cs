using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class PostsService
    {
        HttpClient _httpClient;
		AuthService _authService;
        public PostsService(HttpClient httpClient, AuthService authService )
        {
            _httpClient = httpClient;
			_authService = authService;
        }

        public async Task<List<PostDTO>> GetPosts(DateTime lastDatel, int minimumSkipCount, int takeCount)
        {
			string lastDate = lastDatel.ToString("yyyy-MM-ddTHH:mm:ss");
			var url = $"https://localhost:7183/api/Post/getByDate/{lastDate}/{minimumSkipCount}/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
            }
            else
                return null;
        }
        public async Task<PostDTO> GetSinglePost(int id)
        {
            var url = $"https://localhost:7183/api/Post/{id}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PostDTO>();
            }
            else
                return null;
        }
        public async Task<List<PostDTO>> GetPostsByCommunity(int communityId)
        {
			var url = $"https://localhost:7183/api/Post/getByCommunity/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
			}
			else
				return null;
		}
		public async Task<List<PostDTO>> GetPostsBySearch(string text)
		{
			var url = $"https://localhost:7183/api/Post/getBySearch/{text}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
			}
			else
				return null;
		}
		public async Task<HttpResponseMessage> AddPost( string title, string content, int communityId)
		{
			var url = $"https://localhost:7183/api/Post/add/{title}/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(content);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> DeletePost(int postId)
		{
			var url = $"https://localhost:7183/api/Post/remove/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PutAsync(url, new StringContent("", Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
	}
}
