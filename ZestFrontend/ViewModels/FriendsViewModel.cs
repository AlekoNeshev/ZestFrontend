using CommunityToolkit.Mvvm.ComponentModel;
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
        public FriendsViewModel(FollowersService followersService, AuthService authService)
        {
            this.followersService = followersService;
			this.authService = authService;
			GetFriends();
        }
		public ObservableCollection<FollowerDTO> Friends { get; private set; } = new();
		public async void GetFriends()
		{
			foreach (var item in await followersService.GetFriends(authService.Id))
			{
				Friends.Add(item);
			}
		}
	}
}
