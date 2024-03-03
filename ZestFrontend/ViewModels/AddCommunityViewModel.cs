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
    public partial class AddCommunityViewModel : ObservableObject
    {
        CommunityService communityService;
        AuthService authService;
        public AddCommunityViewModel(CommunityService communityService ) 
        {
            this.communityService = communityService;
            this.authService = AuthService.Instance;
        }
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string description;
        [RelayCommand]
        async Task CreateCommunityAsync()
        {
            var response = await communityService.AddCommunity(Name, Description);
            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"{nameof(CommunitiesPage)}");


            }
        }
    }
}
