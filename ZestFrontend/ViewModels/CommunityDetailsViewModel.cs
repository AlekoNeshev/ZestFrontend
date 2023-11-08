using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    [QueryProperty(nameof(Community), "Community")]
    public partial class CommunityDetailsViewModel : ObservableObject
    {
        CommunityService communityService;
        public CommunityDetailsViewModel(CommunityService communityService)
        {
            this.communityService = communityService;
            
        }

        [ObservableProperty]
        CommunityDTO community;

        [RelayCommand]
        async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync(nameof(CommunitiesPage));
        }
    }
}
