using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(User), "User")]
	public partial class UserDetailsViewModel : ObservableObject
	{
		FollowersService followersService;
		AuthService authService;
        public UserDetailsViewModel(FollowersService followersService, AuthService authService)
        { 
            this.followersService = followersService;
			this.authService = authService;
        }
        [ObservableProperty]
        UserDTO user;
		[ObservableProperty]
		string buttonText;
		partial void OnUserChanged(UserDTO value)
		{
			if (value.IsFollowed)
			{
				ButtonText = "Unfollow";
			}
			else
			{
				ButtonText = "Follow";
			}
		}
		[RelayCommand]
		async Task ChangeFollowshipStatusAsync()
		{
			if (User.IsFollowed)
			{

				var result = await followersService.Unfollow(User.Id);
				if (result.StatusCode == HttpStatusCode.OK)
				{
					ButtonText = "Follow";
					User.IsFollowed = false;
				}
			}
			else
			{

				var result = await followersService.Follow(User.Id);
				if (result.IsSuccessStatusCode)
				{
					ButtonText = "Unfollow";
					User.IsFollowed = true;
				}
			}
		}
	}
}
