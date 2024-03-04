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
        AccountService service;
        AuthService authService;
        public AccountViewModel(AccountService service, AuthService authService)
        {
            this.service = service;
            this.authService = authService;
            GetAccountDetails();
        }
        [ObservableProperty]
        AccountDTO account;
        public async void GetAccountDetails()
        {
            Account = await service.GetCurrentAccount(authService.Id);
        }
        [RelayCommand]
        async Task LogoutAsync()
        {
            try
            {
                  await authService.LogoutAsync();
                authService.Token ="";
                authService.Username = "";
                authService.Id = "";
				await Shell.Current.GoToAsync($"{nameof(MainPage)}");
			}
            catch (Exception ex)
            {

            }
        }
    }
}
