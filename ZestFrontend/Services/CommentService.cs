using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public CommentService(HttpClient client)
        {
            this._httpClient=client;
        }

        public async Task<CommentDTO[]> GetComments(int postId)
        {

            var url = $"https://localhost:7183/api/Comments/getCommentsByPost/{postId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CommentDTO[]>();
            }
            else
                return null;
        }
        public async Task<HttpResponseMessage> PostComment(int postId, int accountId, string text,int commentId = 0)
        {
            var url = $"https://localhost:7183/api/Comments/add/{accountId}/post/{postId}/comment/{commentId}";
            var body = JsonConvert.SerializeObject(text);
            var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
