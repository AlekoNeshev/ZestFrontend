using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.Constants;

namespace ZestFrontend.Services
{
    public class DeleteHubConnectionService
	{
		private HubConnection _deleteConnection;
		private readonly AuthService _authService;
		private SignalRConnectionService _signalRConnectionService;
		public DeleteHubConnectionService(AuthService authService, SignalRConnectionService signalRConnectionService )
		{
			_authService=authService;
			_signalRConnectionService=signalRConnectionService;
		}

		public HubConnection DeleteConnection => _deleteConnection;

		public void Init()
		{
			_deleteConnection = BuildLikesHubConnection($"{PortConst.Port_Forward_Http}/deletehub");
			 StartConnections();
		}
		private HubConnection BuildLikesHubConnection(string url)
		{
			var connection = new HubConnectionBuilder()
				.WithUrl(url, options =>
				{
					options.AccessTokenProvider = async () =>
					{
						string accessToken = _authService.Token;
						return accessToken;
					};
				})
				.Build();

			return connection;
		}

		private async void StartConnections()
		{
			await _deleteConnection.StartAsync();
			_deleteConnection.Closed +=_commentsConnection_Closed;
		}

		private async Task _commentsConnection_Closed(Exception arg)
		{
			await _deleteConnection.StartAsync();
			if (_authService.Groups.Count != 0)
			{
				await _signalRConnectionService.AddConnectionToGroup(DeleteConnection.ConnectionId, _authService.Groups.Where(x => x.Contains("pdd")).ToArray());
			}
		}
	}
}
