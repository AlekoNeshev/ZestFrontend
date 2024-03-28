using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public async Task Init()
		{
			_deleteConnection = BuildLikesHubConnection($"{PortConst.Port_Forward_Http}/deletehub");
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
