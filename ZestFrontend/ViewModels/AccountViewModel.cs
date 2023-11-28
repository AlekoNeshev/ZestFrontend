using CommunityToolkit.Mvvm.ComponentModel;
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
    }
}
