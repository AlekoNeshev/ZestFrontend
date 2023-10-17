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
    
      public partial class LoginViewModel : ObservableObject
    {
        LoginService service;
        public LoginViewModel(LoginService service) 
        {
            this.service = service;
        }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;
         [RelayCommand]
        async Task Login()
        {
             AccountDTO account = await service.GetAccount(Username, Password);
            if (account == null)
            {
                return;
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(PostsPage)}");
            }
        }
    }
}
