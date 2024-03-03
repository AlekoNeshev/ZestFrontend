using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
	public class CommentsHubConnectionService
	{
		private HubConnection _commentsConnection;
		private readonly AuthService _authService;
		public CommentsHubConnectionService( )
		{

			_authService=AuthService.Instance;
		}

		public HubConnection CommentsConnection => _commentsConnection;

		public async void Init()
		{
			_commentsConnection = BuildLikesHubConnection("https://localhost:7183/commentshub");
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
			await _commentsConnection.StartAsync();
		}
	}
}
