using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.ViewModels
{
  
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel() { }

        [RelayCommand]
        async Task GoToLogin()
        {
            await Shell.Current.GoToAsync(nameof(Login));
        }
        [RelayCommand]
        async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterNewUser));
        }
    }
}
