
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using ZestFrontend.Constants;
using ZestFrontend.DTOs;

namespace ZestFrontend.Services
{
    public class CommunityService
    {
        HttpClient _httpClient;
		AuthService _authService;
        public CommunityService(HttpClient httpClient , AuthService authService)
        {
            _httpClient = httpClient;
			_authService = authService;
        }

        public async Task<List<CommunityDTO>> GetCommunitiesByAccount(string accountId, int takeCount, int skipCount)
        {
            var url = $"{PortConst.Port_Forward_Http}/Zest/Community/getByAccountId/{accountId}/{takeCount}/{skipCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityDTO>>();
            }
            else
                return null;
        }
		public async Task<List<CommunityDTO>> GetCommunities(int skipCount, int takeCount)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Community/getAll/{takeCount}/{skipCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<CommunityDTO>>();
			}
			else
				return null;
		}
		public async Task<List<CommunityDTO>> GetCommunitiesBySearch(string text, int takeCount, int[] skipIds = null)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Community/getBySearch/{text}/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(skipIds);

			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<CommunityDTO>>();
			}
			else
				return null;
		}
		public async Task<HttpResponseMessage> Follow(int communityId)       
        {
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityFollowers/account/add/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			
			return response;
		}
		public async Task<HttpResponseMessage> Unfollow(int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityFollowers/account/delete/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.DeleteAsync(url);

			return response;
		}
		public async Task<HttpResponseMessage> IsSubscribed(int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityFollowers/account/add/community/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));

			return response;
		}
        public async Task<HttpResponseMessage> AddCommunity(string name, string description)
        {
			var url = $"{PortConst.Port_Forward_Http}/Zest/Community/add";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var communityInfo = new CommunityBaseDTO { Name = name, Description = description };
			var body = JsonConvert.SerializeObject(communityInfo);
			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<HttpResponseMessage> DeleteCommunity(int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Community/delete/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.DeleteAsync(url);
			response.EnsureSuccessStatusCode();
			return response;
		}

		public async Task<HttpResponseMessage> AddCommunityModerator(string accountId, int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityModerators/add/{accountId}/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<UserDTO[]> GetModeratorsByCommunity(int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityModerators/getModerators/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<UserDTO[]>(); 
		}
		public async Task<UserDTO[]> GetModeratorCandidatesByCommunity(int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityModerators/getCandidates/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<UserDTO[]>(); 
		}
		public async Task<HttpResponseMessage> ApproveCandidate(string accountId, int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityModerators/approveCandidate/{accountId}/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<bool> IsModerator(string accountId, int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityModerators/isModerator/{accountId}/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<bool>();
		}
		public async Task<HttpResponseMessage> RemoveModerator(string accountId, int communityId)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/CommunityModerators/removeModerator/{accountId}/{communityId}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.PostAsync(url, new StringContent("data"));
			response.EnsureSuccessStatusCode();
			return response;
		}
		public async Task<List<CommunityDTO>> GetTrendingCommunitiesAsync(int takeCount, int[] skipIds = null)
		{
			var url = $"{PortConst.Port_Forward_Http}/Zest/Community/getByPopularityId/{takeCount}";
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var body = JsonConvert.SerializeObject(skipIds);

			var response = await _httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<CommunityDTO>>();
			}

			return null;
		}
	}
}
