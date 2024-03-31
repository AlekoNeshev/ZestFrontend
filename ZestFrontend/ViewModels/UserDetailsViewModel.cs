using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		FollowersService _followersService;
		CommunityService _communityService;
        public UserDetailsViewModel(FollowersService followersService, CommunityService communityService)
        { 
            this._followersService = followersService;
			this._communityService = communityService;
        }
        [ObservableProperty]
        UserDTO user;
		[ObservableProperty]
		string buttonText;
		public ObservableCollection<CommunityDTO> Communities { get; } = new();

		public async void GetComs()
		{
			Communities.Clear();
			foreach (var item in await _communityService.GetCommunitiesByAccount(User.Id, 50, Communities.Count))
			{
				Communities.Add(item);
			}
		}
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
			GetComs();
		}
		[RelayCommand]
		async Task ChangeFollowshipStatusAsync()
		{
			if (User.IsFollowed)
			{

				var result = await _followersService.Unfollow(User.Id);
				if (result.StatusCode == HttpStatusCode.OK)
				{
					ButtonText = "Follow";
					User.IsFollowed = false;
				}
			}
			else
			{

				var result = await _followersService.Follow(User.Id);
				if (result.IsSuccessStatusCode)
				{
					ButtonText = "Unfollow";
					User.IsFollowed = true;
				}
			}
		}
		[RelayCommand]
		async Task GoToCommunityDetailPageAsync(CommunityDTO community)
		{
			if (community== null) return;

			await Shell.Current.GoToAsync($"{nameof(CommunityDetailsPage)}?id={community.Name}", true,
				new Dictionary<string, object>
			{
			{"Community", community }
			});
		}
	}
}
