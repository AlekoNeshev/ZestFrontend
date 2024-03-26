using Auth0.ManagementApi.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    public partial class AccountViewModel : ObservableObject
    {
        AccountService _accountService;
        AuthService _authService;
        public AccountViewModel(AccountService service, AuthService authService)
        {
            this._accountService = service;
            this._authService = authService;
            GetAccountDetails();
        }
        [ObservableProperty]
        AccountDTO account;
        public async void GetAccountDetails()
        {
            Account = await _accountService.GetCurrentAccount(_authService.Token);
        }
        [RelayCommand]
        async Task LogoutAsync()
        {
            try
            {
                  await _authService.LogoutAsync();
                _authService.Token ="";
                _authService.Username = "";
                _authService.Id = "";
				await Shell.Current.GoToAsync($"{nameof(MainPage)}");
			}
            catch (Exception ex)
            {

            }
        }
    }
}
