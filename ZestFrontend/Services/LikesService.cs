using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
    public class LikesService
    {
        HttpClient _httpClient;
        AuthService _authService;
        public LikesService(HttpClient httpClient, AuthService authService)
        {
            this._httpClient = httpClient;
            this._authService = authService;
        }
        public async Task<HttpResponseMessage> Like( int postId, int commentId, bool value)
        {
            var url = $"https://localhost:7183/api/Likes/add/post/{postId}/comment/{commentId}/value/{value}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
            response.EnsureSuccessStatusCode();
            return response;

        }
    }
}
