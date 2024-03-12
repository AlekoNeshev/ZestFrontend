using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	public partial class NavigationViewModel : ObservableObject
	{
		private CommunityService _communityService;
		public NavigationViewModel(CommunityService communityService) 
		{
			this._communityService = communityService;
			GetComs();
		}
		[ObservableProperty]
		bool isBtnVisible;
		public ObservableCollection<CommunityDTO> Communities { get; } = new();
		public async void GetComs()
		{
			Communities.Clear();
			foreach (var item in await _communityService.GetCommunitiesByAccount())
			{
				Communities.Add(item);
			}
		}
		[RelayCommand]
		async Task ShowFollowedComsAsync()
		{
			IsBtnVisible = !IsBtnVisible;
		}
		[RelayCommand]
		async Task GoToPostsAsync()
		{
			await Shell.Current.GoToAsync($"{nameof(PostsPage)}");
		}
		[RelayCommand]
		async Task GoToCommunitiesAsync()
		{
			await Shell.Current.GoToAsync($"{nameof(CommunitiesPage)}");
		}
		[RelayCommand]
		async Task GoToChatsAsync()
		{
			await Shell.Current.GoToAsync($"{nameof(FriendsPage)}");
		}
		[RelayCommand]
		async Task GoToUsersAsync()
		{
			await Shell.Current.GoToAsync($"{nameof(UsersPage)}");
		}
		[RelayCommand]
		async Task GoToAccountAsync()
		{
			await Shell.Current.GoToAsync($"{nameof(AccountPage)}");
		}
	}
}
