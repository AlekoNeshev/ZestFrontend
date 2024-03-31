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
		
        public UsersViewModel(AccountService accountService, FollowersService followersService, AuthService authService)
        {
            this._accountService = accountService;
			this._authService = authService;
			this._followersService = followersService;
			GetUsers();
        }
       
		[ObservableProperty]
		bool isButtonVisible;
		[ObservableProperty]
		bool isRefreshing;
		[ObservableProperty]
		string searchText;
		public ObservableCollection<UserDTO> Users { get; } = new();

		private bool isInSearchMode;

		public bool IsInSearchMode
		{
			get { return isInSearchMode; }
			set { isInSearchMode = value; }
		}
		public async Task GetUsers()
		{
			foreach (var user in await _accountService.GetAllAccounts(50, Users.Count))
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
		public async Task SearchUsers()
		{
			foreach (var item in await _accountService.GetAccountsBySearch(SearchText, _authService.Token, 50, Users.Select(x => x.Id).ToArray()))
			{
				Users.Add(item);
			}
		}
		
		[RelayCommand]
		async Task SearchUsersAsync()
		{
			if (!string.IsNullOrWhiteSpace(SearchText))
			{
				Users.Clear();
				await SearchUsers();
				IsInSearchMode = true;
			}
			else
			{
				Users.Clear();
				await GetUsers();
				IsInSearchMode = false;
			}
		}
		
		[RelayCommand]
		async Task RefreshAsync()
		{
			Users.Clear();
			await GetUsers();
			IsRefreshing = false;
		}
		[RelayCommand]
		async Task LoadMoreUsersAsync()
		{
			if (!string.IsNullOrEmpty(SearchText) && IsInSearchMode == true)
			{
				await SearchUsers();
			}
			else
			{
				await GetUsers();
			}
		}
	}
}
