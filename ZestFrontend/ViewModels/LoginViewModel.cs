using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.ViewModels
{
    
      public partial class LoginViewModel : ObservableObject
    {
        public LoginViewModel() { }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;
    }
}
