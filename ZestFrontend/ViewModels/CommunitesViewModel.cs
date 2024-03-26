using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Filters;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    public partial class CommunitesViewModel : ObservableObject
    {
        CommunityService _communityService;
        AuthService _authService;
		CommunitiesFilterOptions _filter;
		public CommunitesViewModel(CommunityService communityService, AuthService authService) 
        { 
            this._communityService = communityService;
            this._authService = authService;
            _filter = CommunitiesFilterOptions.Popular;
            GetCommunities();
        }

        public ObservableCollection<CommunityDTO> Communities { get; } = new();
		[ObservableProperty]
		bool areFiltersVisible;

		public async void GetCommunities()
        {
            Communities.Clear();
            if (_filter != CommunitiesFilterOptions.All)
            {

                foreach (var item in await _communityService.GetCommunities())
                {
                    Communities.Add(item);

                }
                _filter = CommunitiesFilterOptions.All;
            }
        }
		public async Task GetCommunitiesBySearchAsync(string text)
		{
			Communities.Clear();
			

				foreach (var item in await _communityService.GetCommunitiesBySearch(text))
				{
					Communities.Add(item);

				}
				
			
		}
		public async void GetPopularCommunities()
		{
            
			Communities.Clear();
			int[] skipIds = Communities.Select(x => x.Id).ToArray();
			if (_filter != CommunitiesFilterOptions.Popular)
			{


				foreach (var item in await _communityService.GetTrendingCommunitiesAsync(50, skipIds))
				{
					Communities.Add(item);

				}
                _filter = CommunitiesFilterOptions.Popular;
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
		[RelayCommand]
		async Task FilterBtnAsync()
		{
			AreFiltersVisible = !AreFiltersVisible;
		}
        [RelayCommand]
        async Task GetAllComsAsync()
        {
            GetCommunities();
        }
        [RelayCommand]
        async Task GetPopularComsAsync()
        {
            GetPopularCommunities();
        }
        [RelayCommand]
        async Task SearchCommunitiesAsync(string text)
        {
            await GetCommunitiesBySearchAsync(text);
        }
	}
}
