using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
	public class SignalRConnectionService
	{
        HttpClient _httpClient;
        public SignalRConnectionService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
		public async Task<HttpResponseMessage> AddConnectionToGroup(string connectionId, string[] groupsId)
		{
			var url = $"https://localhost:7183/api/SignalRGroups/addConnectionToGroup/{connectionId}";
			var body = JsonConvert.SerializeObject(groupsId);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> RemoveConnectionToGroup(string connectionId)
		{
			var url = $"https://localhost:7183/api/SignalRGroups/removeConnectionToGroup/{connectionId}";
			var body = JsonConvert.SerializeObject("");
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
	}
}
