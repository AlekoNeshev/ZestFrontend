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
        public MediaService(HttpClient httpClient, AuthService authService )
        {
            this._httpClient = httpClient;
			this._authService = authService;
        }
		public async Task<byte[]> GetMedia(string name)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/PostRescources/ivan/{name}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadAsByteArrayAsync();
			else
				return null;
		}
		public async Task<PostResourcesDTO[]> GetPhotosByPostId(int postId)
		{
			var url = $"{PortConst.Port_Forward_Http}/api/PostRescources/getByPostId/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<PostResourcesDTO[]>();
			}
			else
				return null;
		}
		public async Task<HttpResponseMessage> UploadImage(int postId, FileResult[] postedFiles)
		{
			var request = $"{PortConst.Port_Forward_Http}/api/PostRescources/uploadFile/{postId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var content = new MultipartFormDataContent();
			

			if (postedFiles != null && postedFiles.Length > 0)
			{
				for (int i = 0; i < postedFiles.Length; i++)
				{
					var file = postedFiles[i];
					content.Add(new StreamContent(await file.OpenReadAsync()), $"postedFiles", file.FileName);
				}
			}




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
