﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Follower), "Follower")]
	public partial class ChatViewModel : ObservableObject
	{
        MessageService _messageService;
		AuthService _authService;
		MessageHubConnectionService _messageHubConnection;
		SignalRConnectionService _signalRConnectionService;
		public event EventHandler NewMessageReceived;
		public event EventHandler OnOpenScreen;
		
        public ChatViewModel(MessageService messageService, MessageHubConnectionService messageHubConnectionService, SignalRConnectionService signalRConnectionService, AuthService authService)
        {
			_messageHubConnection = messageHubConnectionService;
			 this._messageHubConnection.Init();
			_messageHubConnection.MessageConnection.On<int>("MessageSent", (id) => GetSingleMessage(id));
			this._messageService = messageService;
			this._authService = authService;
			
			this._signalRConnectionService = signalRConnectionService;
			
			
		}
		
		public ObservableCollection<MessageGroup> Messages { get; private set; } = new();
		
		[ObservableProperty]
		FollowerDTO follower;
		[ObservableProperty]
		bool isLoadingMessages;
		async partial void OnFollowerChanged(FollowerDTO value)
		{
			Messages.Clear();
			await GetMessages();
			await Task.Delay(100);
			await OnOpen();
			
		}
		[RelayCommand]
		async Task SendAsync(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			await _messageService.SendMessage(Follower.Id, text);
		}
		[RelayCommand]
		async Task GetMoreMessagesAsync()
		{
			await LoadMoreMessages();
		}
		public async Task GetMessages()
		{
			
			DateTime lastDate = new DateTime();
			if (Messages.Count==0)
			{
				lastDate = DateTime.UtcNow;
			}
			else
			{
				lastDate = Messages.First().FirstOrDefault().CreatedOn + (DateTime.UtcNow - DateTime.Now); ;
			}
			var p = await _messageService.GetMessages(Follower.Id, lastDate, 40);
			
			var groups = p.GroupBy(m => m.CreatedOn.Date);
			foreach (var item in groups.Reverse())
			{
				var messageGroup = new MessageGroup { Date = item.Key };

				foreach (var message in item.OrderBy(x => x.CreatedOn))
				{
					messageGroup.Add(message);
					message.IsOwner = message.SenderUsername == _authService.Username;
					message.CreatedOn = message.CreatedOn + (DateTime.Now - DateTime.UtcNow);
				}
				Messages.Add(messageGroup);

			}
		}
		public async Task LoadMoreMessages()
		{
			IsLoadingMessages = true;
			DateTime lastDate = new DateTime();
			if (Messages.Count==0)
			{
				lastDate = DateTime.UtcNow;
			}
			else
			{
				lastDate = Messages.First().FirstOrDefault().CreatedOn +  (DateTime.UtcNow - DateTime.Now); 
			}
			var messages = await _messageService.GetMessages(Follower.Id, lastDate, 40);
			var groups = messages.GroupBy(m => m.CreatedOn.Date);
			foreach (var item in groups)
			{
				var messageGroup = new MessageGroup { Date = item.Key };
				var group = Messages.FirstOrDefault(x => x.Date == item.Key);
				if (group != null)
				{

					foreach (var message in item.OrderByDescending(x => x.CreatedOn))
					{
						message.IsOwner = message.SenderUsername == _authService.Username;
						message.CreatedOn = message.CreatedOn +  (DateTime.Now - DateTime.UtcNow);
						group.Insert(0, message);
					}
				}
				else
				{

					foreach (var message in item.OrderBy(x => x.CreatedOn))
					{
						message.IsOwner = message.SenderUsername == _authService.Username;
						message.CreatedOn = message.CreatedOn +  (DateTime.Now - DateTime.UtcNow);
						messageGroup.Add(message);
					}
					Messages.Insert(0, messageGroup);
				}

			}

			IsLoadingMessages = false;
		}
			
		public async Task GetSingleMessage(int messageId)
		{
			var message = await _messageService.FindById(messageId);
			message.IsOwner = message.SenderUsername == _authService.Username;
			message.CreatedOn = message.CreatedOn + (DateTime.Now - DateTime.UtcNow);
			if (Messages.Count >0)
			{
				if (Messages.LastOrDefault().Date.Date == message.CreatedOn.Date)
				{
					Messages.LastOrDefault().Add(message);
				}
				else
				{
					var messageGroup = new MessageGroup { Date = message.CreatedOn.Date };
					messageGroup.Add(message);
					Messages.Add(messageGroup);

				}
			}
			
			OnNewMessageReceived();

		}
		
		protected virtual void OnNewMessageReceived()
		{
			NewMessageReceived?.Invoke(this, EventArgs.Empty);
		}
		 async protected virtual Task OnOpen()
		{
			
			OnOpenScreen?.Invoke(this, EventArgs.Empty);
		}
		public async void OnNavigatedTo()
		{
			int comparisonResult = string.Compare(_authService.Id, Follower.Id);
			string firstHubId, secondHubId;

			if (comparisonResult >= 0)
			{
				firstHubId = _authService.Id;
				secondHubId = Follower.Id;
			}
			else
			{
				firstHubId = Follower.Id;
				secondHubId = _authService.Id;
			}
			while (_messageHubConnection.MessageConnection.ConnectionId == null)
			{
				await Task.Delay(100);
			}
			if (_messageHubConnection.MessageConnection.ConnectionId!= null)
			{
				await _signalRConnectionService.AddConnectionToGroup(_messageHubConnection.MessageConnection.ConnectionId, new string[] { $"chat-{firstHubId}{secondHubId}" });
			}
			_authService.Groups.Add($"chat-{firstHubId}{secondHubId}");
		}
		public async void OnNavigatedFrom()
		{
			int comparisonResult = string.Compare(_authService.Id, Follower.Id);
			string firstHubId, secondHubId;

			if (comparisonResult >= 0)
			{
				firstHubId = _authService.Id;
				secondHubId = Follower.Id;
			}
			else
			{
				firstHubId = Follower.Id;
				secondHubId = _authService.Id;
			}
			if (_messageHubConnection.MessageConnection.ConnectionId!= null)
			{
				await _signalRConnectionService.RemoveConnectionToGroup(_messageHubConnection.MessageConnection.ConnectionId);
			}
			_authService.Groups.Clear();
		}
	}
}
