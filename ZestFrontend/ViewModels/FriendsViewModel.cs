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
			GetFriends();
        }
		public ObservableCollection<FollowerDTO> Friends { get; private set; } = new();
		public async void GetFriends()
		{
			foreach (var item in await _followersService.GetFriends())
			{
				Friends.Add(item);
			}
		}
		[RelayCommand]
		async Task GoToChatPageAsync(FollowerDTO follower)
		{
			if (follower== null) return;

			await Shell.Current.GoToAsync($"{nameof(ChatPage)}?id={follower.FollowerId}", true,
				new Dictionary<string, object>
			{
			{"Follower", follower }
			});
		}
		[RelayCommand]
		async Task RefreshAsync()
		{
			Friends.Clear();
			GetFriends();
		}
	}
}
