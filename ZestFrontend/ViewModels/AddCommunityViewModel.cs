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
        CommunityService _communityService;
        AuthService _authService;
        public AddCommunityViewModel(CommunityService communityService, AuthService authService) 
        {
            this._communityService = communityService;
            this._authService = authService;
        }
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string description;
        [RelayCommand]
        async Task CreateCommunityAsync()
        {
            var response = await _communityService.AddCommunity(Name, Description);
            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"{nameof(CommunitiesPage)}");


            }
        }
    }
}
