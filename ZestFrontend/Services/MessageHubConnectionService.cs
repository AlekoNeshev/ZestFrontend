using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.Constants;

namespace ZestFrontend.Services
{
    public class MessageHubConnectionService
	{
		private HubConnection _messageConnection;
		private readonly AuthService _authService;
		private SignalRConnectionService _signalRConnectionService;
		public MessageHubConnectionService(AuthService authService, SignalRConnectionService signalRConnectionService)
		{
			_authService=authService;
			_signalRConnectionService=signalRConnectionService;
		}

		public HubConnection MessageConnection => _messageConnection;

		public async void Init()
		{
			_messageConnection = BuildLikesHubConnection($"{PortConst.Port_Forward_Http}/messagehub");
			await StartConnections();
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

		private async Task StartConnections()
		{
			await _messageConnection.StartAsync();
			_messageConnection.Closed += _messageConnection_Closed;
		}

		private async Task _messageConnection_Closed(Exception arg)
		{
			await _messageConnection.StartAsync();
			if (_authService.Groups.Count != 0)
			{
				await _signalRConnectionService.AddConnectionToGroup(MessageConnection.ConnectionId, _authService.Groups.Where(x => x.Contains("message")).ToArray());
			}
		}
	}
}
