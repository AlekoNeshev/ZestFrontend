using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
	public class MessageHubConnectionService
	{
		private HubConnection _messageConnection;
		private readonly AuthService _authService;
		public MessageHubConnectionService()
		{

			_authService=AuthService.Instance;
		}

		public HubConnection MessageConnection => _messageConnection;

		public async void Init()
		{
			_messageConnection = BuildLikesHubConnection("https://localhost:7183/messagehub");
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
			await _messageConnection.StartAsync();
		}
	}
}
