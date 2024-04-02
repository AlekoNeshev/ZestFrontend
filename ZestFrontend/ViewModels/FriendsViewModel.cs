using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
	public partial class FriendsViewModel : ObservableObject
	{
        FollowersService _followersService;
		AuthService _authService;
        public FriendsViewModel(FollowersService followersService, AuthService authService)
        {
            this._followersService = followersService;
			this._authService = authService;
			Init();
        }
		public ObservableCollection<FollowerDTO> Friends { get; private set; } = new();
		[ObservableProperty]
		bool isRefreshing;
		[ObservableProperty]
		string searchText;

		private bool isInSearchMode;

		public bool IsInSearchMode
		{
			get { return isInSearchMode; }
			set { isInSearchMode = value; }
		}
		public async void Init()
		{
			await GetFriends();
		}
		[RelayCommand]
		async Task GoToChatPageAsync(FollowerDTO follower)
		{
			if (follower== null) return;

			await Shell.Current.GoToAsync($"{nameof(ChatPage)}?id={follower.Id}", true,
				new Dictionary<string, object>
			{
			{"Follower", follower }
			});
		}
		[RelayCommand]
		async Task RefreshAsync()
		{
			Friends.Clear();
			await GetFriends();
			IsRefreshing = false;
		}
		[RelayCommand]
		async Task SearchFollowersAsync()
		{
			if (!string.IsNullOrWhiteSpace(SearchText))
			{
				Friends.Clear();
				await SearchFriends();
				IsInSearchMode = true;
			}
			else
			{
				Friends.Clear();
				await GetFriends();
				IsInSearchMode=false;
			}
		}
		[RelayCommand]
		async Task LoadMoreFriendsAsync()
		{
			if(!string.IsNullOrEmpty(SearchText) && IsInSearchMode == true)
			{
				await SearchFollowersAsync();
			}
			else
			{
				await GetFriends();
			}
		}
		public async Task GetFriends()
		{
			foreach (var item in await _followersService.GetFriends(50, Friends.Count))
			{
				Friends.Add(item);
			}
		}
		public async Task SearchFriends()
		{
			foreach (var item in await _followersService.GetAccountsBySearch(SearchText, 50, Friends.Select(x => x.Id).ToArray()))
			{
				Friends.Add(item);
			}
		}
	}
}
