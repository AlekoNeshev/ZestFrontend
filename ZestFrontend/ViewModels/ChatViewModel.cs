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
		HubConnection hubConnection;
        public ChatViewModel(MessageService messageService, AuthService authService)
        {
            this.messageService = messageService;
			this.authService = authService;
			hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7183/messagehub").Build();	
			hubConnection.On("MessageSent", GetMessages);			
			hubConnection.StartAsync();
			
		}
		public ObservableCollection<MessageDTO> Messages { get; private set; } = new();
		[ObservableProperty]
		FollowerDTO follower;
		public async void GetMessages()
		{
			Messages.Clear();
			foreach (var item in await messageService.GetMessages(Follower.FollowerId, authService.Id))
			{
				Messages.Add(item);
			}
		}
		partial void OnFollowerChanged(FollowerDTO value)
		{
			GetMessages();
		}
		[RelayCommand]
		async Task SendAsync(string text)
		{
			await messageService.SendMessage(authService.Id, Follower.FollowerId, text);
		}
	}
}
