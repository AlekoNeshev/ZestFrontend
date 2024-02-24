﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        public ChatViewModel(MessageService messageService, AuthService authService, MessageHubConnectionService messageHubConnectionService, SignalRConnectionService signalRConnectionService)
        {
            this.messageService = messageService;
			this.authService = authService;
			hubConnection = messageHubConnectionService;
			this.signalRConnectionService = signalRConnectionService;
			
			
		}
		public ObservableCollection<MessageDTO> Messages { get; private set; } = new();
		[ObservableProperty]
		FollowerDTO follower;
		public async void GetMessages()
		{
			Messages.Clear();
			foreach (var item in await messageService.GetMessages(Follower.FollowerId, authService.Id))
			{
				item.IsOwner = item.SenderUsername == authService.Username;
				Messages.Add(item);
			}
		}
		partial void OnFollowerChanged(FollowerDTO value)
		{
			hubConnection.Init();
			GetMessages();
			hubConnection.MessageConnection.On<int>("MessageSent",(id) => GetSingleMessage(id));
		}
		public async void OnNavigatedTo()
		{
			int firstHubId = Math.Max(authService.Id, Follower.FollowerId);
			int secondHubId = Math.Min(authService.Id, Follower.FollowerId);
			await signalRConnectionService.RemoveConnectionToGroup(hubConnection.MessageConnection.ConnectionId);
			await signalRConnectionService.AddConnectionToGroup(hubConnection.MessageConnection.ConnectionId, new string[] { $"chat-{firstHubId}{secondHubId}" });
		}
		[RelayCommand]
		async Task SendAsync(string text)
		{
			await messageService.SendMessage(authService.Id, Follower.FollowerId, text);
		}
		public async void GetSingleMessage(int messageId)
		{
			var message = await messageService.FindById(messageId);
			message.IsOwner = message.SenderUsername == authService.Username;
			Messages.Insert(Messages.Count, message);
		}
	}
}
