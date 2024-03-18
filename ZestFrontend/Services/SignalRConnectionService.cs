using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
	public class SignalRConnectionService
	{
        HttpClient _httpClient;
		AuthService _authService;
        public SignalRConnectionService(HttpClient httpClient, AuthService authService)
        {
            this._httpClient = httpClient;
			this._authService = authService;
		}
		public async Task<HttpResponseMessage> AddConnectionToGroup(string connectionId, string[] groupsId)
		{
			if (connectionId == null)
			{
				throw new Exception();
			}
			var url = $"{PortConst.Port_Forward_Http}/api/SignalRGroups/addConnectionToGroup/{connectionId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(groupsId);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> RemoveConnectionToGroup(string connectionId)
		{
		if(connectionId == null)
			{
				throw new Exception();
			}	
			var url = $"{PortConst.Port_Forward_Http}/api/SignalRGroups/removeConnectionToGroup/{connectionId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject("");
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			try 
			{
				response.EnsureSuccessStatusCode();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return response;
		}
	}
}
