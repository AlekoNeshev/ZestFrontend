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
using static System.Net.Mime.MediaTypeNames;

namespace ZestFrontend.Services
{
	public class PostsService
	{
		HttpClient _httpClient;
		AuthService _authService;
		public PostsService(HttpClient httpClient, AuthService authService)
		{

			_httpClient = httpClient;
			_authService = authService;
		}

		public async Task<PostDTO[]> GetPosts(DateTime lastDatel, int communityId, int takeCount)
		{
			try
			{
				string lastDate = lastDatel.ToString("yyyy-MM-ddTHH:mm:ss");
				var url = $"{PortConst.Port_Forward_Http}/api/Post/getByDate/{lastDate}/{communityId}/{takeCount}";
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
				var response = await _httpClient.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadFromJsonAsync<PostDTO[]>();
				}
				else
					return null;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<PostDTO> GetSinglePost(int id)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Post/{id}";
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
			var url = $"{PortConst.Port_Forward_Http}/api/Post/getByCommunity/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
			}
			else
				return null;
		}
		public async Task<List<PostDTO>> GetPostsBySearch(string text, int takeCount, int[] skipIds = null)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Post/getBySearch/{text}/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(skipIds);

			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
			}
			else
				return null;
		}
		public async Task<HttpResponseMessage> AddPost(string title, string content, int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Post/add/{title}/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(content);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> DeletePost(int postId)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Post/remove/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PutAsync(url, new StringContent("", Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<List<PostDTO>> GetTrendingPostsAsync(int takeCount, int communityId, int[] skipIds = null)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Post/getByTrending/{takeCount}/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(skipIds);



			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
			}

			return null;
		}
		public async Task<List<PostDTO>> GetFollowedPostsAsync(int takeCount, int[] skipIds = null)
		{
			var requestUri = $"{PortConst.Port_Forward_Http}/api/Post/getByFollowed/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(skipIds);



			var response = await _httpClient.PostAsync(requestUri, new StringContent(body, Encoding.UTF8, "application/json"));

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
			}

			return null;
		}

	}
}
