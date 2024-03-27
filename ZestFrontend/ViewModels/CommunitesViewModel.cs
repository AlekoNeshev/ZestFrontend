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
		[ObservableProperty]
		bool isRefreshing;

		public async Task GetCommunities()
		{
			foreach (var item in await _communityService.GetCommunities(Communities.Count, 20))
			{
				Communities.Add(item);
			}
			_filter = CommunitiesFilterOptions.All;
		}
		public async Task GetCommunitiesBySearchAsync(string text)
		{
			Communities.Clear();

			foreach (var item in await _communityService.GetCommunitiesBySearch(text))
			{
				Communities.Add(item);
			}
		}
		public async Task GetPopularCommunities()
		{
			int[] skipIds = Communities.Select(x => x.Id).ToArray();

			foreach (var item in await _communityService.GetTrendingCommunitiesAsync(50, skipIds))
			{
				Communities.Add(item);

			}
			_filter = CommunitiesFilterOptions.Popular;
		}
		public async Task GetFollowedCommunities()
		{
			Communities.Clear();
			foreach (var item in await _communityService.GetCommunitiesByAccount(_authService.Id))
			{
				Communities.Add(item);
			}
			_filter = CommunitiesFilterOptions.Followed;
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
			if (_filter != CommunitiesFilterOptions.All)
			{
				Communities.Clear();
				await GetCommunities();
			}
		}
		[RelayCommand]
		async Task GetPopularComsAsync()
		{
			if (_filter != CommunitiesFilterOptions.Popular)
			{
				Communities.Clear();
				await GetPopularCommunities();
			}
		}
		[RelayCommand]
		async Task GetFollowedComsAsync()
		{
			if (_filter != CommunitiesFilterOptions.Followed)
			{
				Communities.Clear();
				await GetFollowedCommunities();
			}
		}
		[RelayCommand]
		async Task SearchCommunitiesAsync(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				await GetCommunitiesBySearchAsync(text);
			}
			else if (_filter == CommunitiesFilterOptions.All)
			{
				Communities.Clear();
				await GetCommunities();
			}
			else if (_filter == CommunitiesFilterOptions.Popular)
			{
				Communities.Clear();
				await GetPopularCommunities();
			}
			else if (_filter == CommunitiesFilterOptions.Followed)
			{
				Communities.Clear();
				await GetFollowedCommunities();
			}
		}
		[RelayCommand]
		async Task RefreshAsync()
		{
			Communities.Clear();
			if (_filter == CommunitiesFilterOptions.All)
			{	
				await GetCommunities();
			}
			else if(_filter == CommunitiesFilterOptions.Popular)
			{
				await GetPopularCommunities();
			}
			else if (_filter == CommunitiesFilterOptions.Followed)
			{
				await GetFollowedCommunities();
			}
			IsRefreshing = false;
		}
		[RelayCommand]
		async Task LoadMoreComsAsync()
		{
			if(_filter == CommunitiesFilterOptions.All)
			{
				await GetCommunities();
			}
			else if(_filter == CommunitiesFilterOptions.Popular)
			{
				await GetPopularCommunities();
			}
		}
	}
}
