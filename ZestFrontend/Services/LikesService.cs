using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
    public class LikesService
    {
        HttpClient _httpClient;
        public LikesService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> Like(int likerId, int postId, int commentId, bool value)
        {
            var url = $"https://localhost:7183/api/Likes/add/{likerId}/post/{postId}/comment/{commentId}/value/{value}";
            var response = await _httpClient.PostAsync(url, new StringContent("data"));
            response.EnsureSuccessStatusCode();
            return response;

        }
    }
}
