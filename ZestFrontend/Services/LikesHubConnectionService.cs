﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
	public class LikesHubConnectionService
	{
		private  HubConnection _likesConnection;
		private readonly AuthService _authService;
		private SignalRConnectionService _signalRConnectionService;
		public LikesHubConnectionService(AuthService authService, SignalRConnectionService signalRConnectionService )
		{
			_authService=authService;
			_signalRConnectionService=signalRConnectionService;
		}

		public HubConnection LikesConnection => _likesConnection;

		public async void Init()
		{
			_likesConnection = BuildLikesHubConnection($"{PortConst.Port_Forward_Http}/likeshub");
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

		private void StartConnections()
		{
			 _likesConnection.StartAsync();
			_likesConnection.Closed += _likesConnection_Closed;
		}

		private async Task _likesConnection_Closed(Exception arg)
		{
			await _likesConnection.StartAsync();
			if (_authService.Groups.Count != 0)
			{
				await _signalRConnectionService.AddConnectionToGroup(LikesConnection.ConnectionId, _authService.Groups.Where(x => x.Contains("pdl")).ToArray());
			}
		}
	}
}
