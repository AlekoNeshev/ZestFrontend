using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class PostsService
    {
        HttpClient _httpClient;
        public PostsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PostDTO>> GetPosts()
        {
            var url = $"https://localhost:7183/api/Post/getByDate";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PostDTO>>();
            }
            else
                return null;
        }
    }
}
