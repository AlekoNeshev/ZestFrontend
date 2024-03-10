using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Follower), "Follower")]
	public partial class ChatViewModel : ObservableObject
	{
        MessageService messageService;
		AuthService authService;
		MessageHubConnectionService hubConnection;
		SignalRConnectionService signalRConnectionService;
		private Task InitTask;
        public ChatViewModel(MessageService messageService, MessageHubConnectionService messageHubConnectionService, SignalRConnectionService signalRConnectionService, AuthService authService)
        {
            this.messageService = messageService;
			this.authService = authService;
			hubConnection = messageHubConnectionService;
			this.signalRConnectionService = signalRConnectionService;
			InitTask = Init();
			
		}
		private async Task Init()
		{
			
			await this.hubConnection.Init();
			hubConnection.MessageConnection.On<int>("MessageSent", (id) => GetSingleMessage(id));
		}
		public ObservableCollection<MessageDTO> Messages { get; private set; } = new();
		[ObservableProperty]
		FollowerDTO follower;
		public async void GetMessages()
		{
			Messages.Clear();
			foreach (var item in await messageService.GetMessages(Follower.FollowerId))
			{
				item.IsOwner = item.SenderUsername == authService.Username;
				Messages.Add(item);
			}
		}
		partial void OnFollowerChanged(FollowerDTO value)
		{
			
			GetMessages();
			
		}
		public async void OnNavigatedTo()
		{
			if (InitTask is not null && !InitTask.IsCompleted) await InitTask;
			int comparisonResult = string.Compare(authService.Id, Follower.FollowerId);
			string firstHubId, secondHubId;

			if (comparisonResult >= 0)
			{
				firstHubId = authService.Id;
				secondHubId = Follower.FollowerId;
			}
			else
			{
				firstHubId = Follower.FollowerId;
				secondHubId = authService.Id;
			}
			
			await signalRConnectionService.AddConnectionToGroup(hubConnection.MessageConnection.ConnectionId, new string[] { $"chat-{firstHubId}{secondHubId}" });
		}
		public async void OnNavigatedFrom()
		{
			int comparisonResult = string.Compare(authService.Id, Follower.FollowerId);
			string firstHubId, secondHubId;

			if (comparisonResult >= 0)
			{
				firstHubId = authService.Id;
				secondHubId = Follower.FollowerId;
			}
			else
			{
				firstHubId = Follower.FollowerId;
				secondHubId = authService.Id;
			}

			await signalRConnectionService.RemoveConnectionToGroup(hubConnection.MessageConnection.ConnectionId);
			
		}
		[RelayCommand]
		async Task SendAsync(string text)
		{
			await messageService.SendMessage(Follower.FollowerId, text);
		}
		public async void GetSingleMessage(int messageId)
		{
			var message = await messageService.FindById(messageId);
			message.IsOwner = message.SenderUsername == authService.Username;
			Messages.Insert(Messages.Count, message);
		}
	}
}
