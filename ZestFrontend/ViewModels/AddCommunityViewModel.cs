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
        public AddCommunityViewModel(CommunityService communityService) 
        {
            this._communityService = communityService;
  
        }
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string description;
        [RelayCommand]
        async Task CreateCommunityAsync()
        {
			if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description))
			{
                return;
            }
            var response = await _communityService.AddCommunity(Name, Description);
            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"..");
            }
        }
    }
}
