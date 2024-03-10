using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
	public class LikesHubConnectionService
	{
		private  HubConnection _likesConnection;
		private readonly AuthService _authService;
		public LikesHubConnectionService(AuthService authService )
		{
			
			_authService=authService;
		}

		public HubConnection LikesConnection => _likesConnection;

		public async Task Init()
		{
			_likesConnection = BuildLikesHubConnection($"{PortConst.Port_Forward_Http}/likeshub");
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
