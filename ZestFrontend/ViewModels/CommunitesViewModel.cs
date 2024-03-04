using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    public partial class CommunitesViewModel : ObservableObject
    {
        CommunityService communityService;
        AuthService authService;
        public CommunitesViewModel(CommunityService communityService, AuthService authService) 
        { 
            this.communityService = communityService;
            this.authService = authService;
            GetCommunities();
        }

        public ObservableCollection<CommunityDTO> Communities { get; } = new();

       
        public async void GetCommunities()
        {
            foreach (var item in await communityService.GetCommunities())
            {
                Communities.Add(item);
            }
        }
        [RelayCommand]
         async Task GoToCommunityDetailPageAsync(CommunityDTO community)
         {
            if (community== null) return;

            await Shell.Current.GoToAsync($"{nameof(CommunityDetailsPage)}?id={community.Name}", true,
                new Dictionary<string, object>
            {
            {"Community", community }
            });
        }
		[RelayCommand]
		async Task GoToAddCommunityPageAsync()
		{
			await Shell.Current.GoToAsync($"{nameof(AddCommunityPage)}");
		}
	}
}
