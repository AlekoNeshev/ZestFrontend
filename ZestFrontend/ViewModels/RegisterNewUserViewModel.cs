using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
   public partial class RegisterNewUserViewModel : ObservableObject
    {
        AccountService accountService;
        AuthService authService;
        public RegisterNewUserViewModel(AccountService accountService)
        {
            this.accountService = accountService;
            this.authService = AuthService.Instance;
        }
        [ObservableProperty]
        string firstName;
        [ObservableProperty]
        string lastName;
        [ObservableProperty]
        string email;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        string username;
        [ObservableProperty]
        DateTime birthdate;
        [RelayCommand]
        async Task CreateAccountAsync()
        {
            /*var response = await accountService.CreateAccount(FirstName, LastName, Username, Email, Password, Birthdate.Date);
			var content = await response.Content.ReadAsStringAsync();
            authService.Id = content;
			await Shell.Current.GoToAsync($"{nameof(PostsPage)}");*/
		}
    }
}
