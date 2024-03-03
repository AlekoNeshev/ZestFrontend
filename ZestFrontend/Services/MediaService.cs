using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class MediaService
    {
        HttpClient _httpClient;
		AuthService _authService;
        public MediaService(HttpClient httpClient )
        {
            this._httpClient = httpClient;
			this._authService = AuthService.Instance;
        }
		public async Task<byte[]> GetMedia(string name)
		{
			var url = $"https://localhost:7183/api/PostRescources/ivan/{name}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadAsByteArrayAsync();
			else
				return null;
		}
		public async Task<PostResourcesDTO[]> GetPhotosByPostId(int postId)
		{
			var url = $"https://localhost:7183/api/PostRescources/getByPostId/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<PostResourcesDTO[]>();
			}
			else
				return null;
		}
		public async Task<HttpResponseMessage> UploadImage(int postId, FileResult postedFile)
		{
			var request = $"https://localhost:7183/api/PostRescources/ivan/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var content = new MultipartFormDataContent();
			if(postedFile != null)
			content.Add(new StreamContent(await postedFile.OpenReadAsync()), "postedFile", postedFile.FileName);

			
			var response = await _httpClient.PostAsync(request, content);

			
			if (response.IsSuccessStatusCode)
			{
				
				Console.WriteLine("Upload successful");
			}
			else
			{
				
				Console.WriteLine("Error uploading file: " + response.StatusCode);
			}
			return response;
		}
		
	}
}
