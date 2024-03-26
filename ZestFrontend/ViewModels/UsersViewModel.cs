using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {

		AuthService _authService;
		AccountService _accountService;
		FollowersService _followersService;
		
        public UsersViewModel( AccountService accountService, FollowersService followersService, AuthService authService)
        {
            this._accountService = accountService;
			this._authService = authService;
			this._followersService = followersService;
			GetUsers();
        }
       
		[ObservableProperty]
		bool isButtonVisible;
		public ObservableCollection<UserDTO> Users { get; } = new();
		
		public async Task GetUsers()
		{
			foreach (var user in await _accountService.GetAllAccounts())
			{
				Users.Add(user);
			}
		}	
		
		[RelayCommand]
		async Task GoToUserDetailPageAsync(UserDTO user)
		{
			if (user == null) return;

			await Shell.Current.GoToAsync($"{nameof(UserDetailsPage)}?id={user.Id}", true,
				new Dictionary<string, object>
			{
			{"User", user }
			});
		}
		
		[RelayCommand]
		async Task SearchUsersAsync(string text)
		{
		   Users.Clear();
			foreach (var item in await _accountService.GetAccountsBySearch(text, _authService.Token))
			{
				Users.Add(item);
			}
		}
		
		[RelayCommand]
		async Task RefreshAsync()
		{
			Users.Clear();
			await GetUsers();
		}
	}
}
