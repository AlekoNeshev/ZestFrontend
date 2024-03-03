using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
	public class LikesHubConnectionService
	{
		private  HubConnection _likesConnection;
		private readonly AuthService _authService;
		public LikesHubConnectionService( )
		{
			
			_authService=AuthService.Instance;
		}

		public HubConnection LikesConnection => _likesConnection;

		public async void Init()
		{
			_likesConnection = BuildLikesHubConnection("https://localhost:7183/likeshub");
			await StartConnections();
		}
		private HubConnection BuildLikesHubConnection(string url)
		{
			var connection = new HubConnectionBuilder()
				.WithUrl(url, option =>
				{
					option.Headers["userId"] = _authService.Id.ToString();
				})
				.Build();

			

			return connection;
		}

		private async Task StartConnections()
		{
			await _likesConnection.StartAsync();
		}

		
	}
}
