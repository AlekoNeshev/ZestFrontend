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
		public CommentsHubConnectionService(AuthService authService )
		{

			_authService=authService;
		}

		public HubConnection CommentsConnection => _commentsConnection;

		public async Task Init()
		{
			_commentsConnection = BuildLikesHubConnection($"{PortConst.Port_Forward_Http}/commentshub");
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
			_commentsConnection.Closed +=_commentsConnection_Closed;
		}

		private async Task _commentsConnection_Closed(Exception arg)
		{
			await _commentsConnection.StartAsync();
		}
	}
}
