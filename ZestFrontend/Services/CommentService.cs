using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class CommentService
    {
        HttpClient _httpClient;
        AuthService _authService;
        public CommentService(HttpClient client, AuthService authService )
        {
            this._httpClient=client;
            this._authService=authService;
        }

        public async Task<CommentDTO[]> GetComments(int postId, DateTime lastDatel, int takeCount)
        {
			string lastDate = lastDatel.ToString("yyyy-MM-ddTHH:mm:ss");
			var url = $"{PortConst.Port_Forward_Http}/api/Comments/getCommentsByPost/{postId}/{lastDate}/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CommentDTO[]>();
            }
            else
                return null;
        }
        public async Task<HttpResponseMessage> PostComment(int postId, string text,int commentId = 0)
        {
            var url = $"{PortConst.Port_Forward_Http}/api/Comments/add/post/{postId}/comment/{commentId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(text);
            var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<CommentDTO> GetSingleComment(int id)
        {
            var url = $"{PortConst.Port_Forward_Http}/api/Comments/{id}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result =  await response.Content.ReadFromJsonAsync<CommentDTO>();
                if(result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }
        public async Task<HttpResponseMessage> DeleteComment(int commentId, int postId)
        {
            var url = $"{PortConst.Port_Forward_Http}/api/Comments/remove/{commentId}/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PutAsync(url, new StringContent("", Encoding.UTF8, "application/json")); response.EnsureSuccessStatusCode();
            return response;
        }
		public async Task<List<CommentDTO>> GetTrendingPostsAsync(int takeCount, int postId, int[] skipIds = null)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/Comments/getByTrending/{takeCount}/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(skipIds);



			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<CommentDTO>>();
			}

			return null;
		}
	}
}
