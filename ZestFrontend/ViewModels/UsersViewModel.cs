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

		AuthService authService;
		AccountService accountService;
		FollowersService followersService;
		
        public UsersViewModel( AccountService accountService, FollowersService followersService, AuthService authService)
        {
            this.accountService = accountService;
			this.authService = authService;
			this.followersService = followersService;
			GetUsers();
        }
        [ObservableProperty]
		string search;
		[ObservableProperty]
		bool isButtonVisible;
		public ObservableCollection<UserDTO> Users { get; } = new();
		
		public async void GetUsers()
		{
		
			foreach (var user in await accountService.GetAllAccounts())
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
		async Task SearchUsersAsync()
		{
		   Users.Clear();
			/*foreach (var item in await a.GetPostsBySearch(Search))
			{
				Accounts.Add(item);
			}*/
		}
		public void OnNavaigated()
		{

		}
		[RelayCommand]
		async Task RefreshAsync()
		{
			Users.Clear();
			GetUsers();
		}
	}
}
