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
        FollowersService followersService;
		AuthService authService;
        public FriendsViewModel(FollowersService followersService)
        {
            this.followersService = followersService;
			this.authService = AuthService.Instance;
			GetFriends();
        }
		public ObservableCollection<FollowerDTO> Friends { get; private set; } = new();
		public async void GetFriends()
		{
			foreach (var item in await followersService.GetFriends())
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
