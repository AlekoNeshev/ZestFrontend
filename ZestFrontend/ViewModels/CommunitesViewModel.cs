using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
			Init();
		}

		public ObservableCollection<CommunityDTO> Communities { get; } = new();
		[ObservableProperty]
		bool areFiltersVisible;
		[ObservableProperty]
		bool isRefreshing;
		[ObservableProperty]
		string searchText;

		private bool isInSearchMode;

		public bool IsInSearchMode
		{
			get { return isInSearchMode; }
			set { isInSearchMode = value; }
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
		void FilterBtnAsync()
		{
			AreFiltersVisible = !AreFiltersVisible;
		}
		[RelayCommand]
		async Task GetAllComsAsync()
		{
			if (_filter != CommunitiesFilterOptions.All || IsInSearchMode == true)
			{
				Communities.Clear();
				await GetCommunities();
				SearchText = string.Empty;
				IsInSearchMode = false;
			}
		}
		[RelayCommand]
		async Task GetPopularComsAsync()
		{
			if (_filter != CommunitiesFilterOptions.Popular || IsInSearchMode == true)
			{
				Communities.Clear();
				await GetPopularCommunities();
				SearchText = string.Empty;
				IsInSearchMode = false;
			}
		}
		[RelayCommand]
		async Task GetFollowedComsAsync()
		{
			if (_filter != CommunitiesFilterOptions.Followed || IsInSearchMode == true)
			{
				Communities.Clear();
				await GetFollowedCommunities();
				IsInSearchMode = false;
				SearchText = string.Empty;
			}
		}
		[RelayCommand]
		async Task SearchCommunitiesAsync()
		{
			if (!string.IsNullOrWhiteSpace(SearchText))
			{
				await GetCommunitiesBySearchAsync(SearchText);
				IsInSearchMode = true;
			}
			else if (_filter == CommunitiesFilterOptions.All)
			{
				Communities.Clear();
				await GetCommunities();
				SearchText = string.Empty;
			}
			else if (_filter == CommunitiesFilterOptions.Popular)
			{
				Communities.Clear();
				await GetPopularCommunities();
				SearchText = string.Empty;
			}
			else if (_filter == CommunitiesFilterOptions.Followed)
			{
				Communities.Clear();
				await GetFollowedCommunities();
				SearchText = string.Empty;
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
			else if (_filter == CommunitiesFilterOptions.Popular)
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
			if (Communities.Count > 0)
			{
				if (!string.IsNullOrWhiteSpace(SearchText) && IsInSearchMode == true)
				{
					await GetCommunitiesBySearchAsync(SearchText);
				}
				else if (_filter == CommunitiesFilterOptions.All)
				{
					await GetCommunities();
				}
				else if (_filter == CommunitiesFilterOptions.Popular)
				{
					await GetPopularCommunities();
				}
				else if (_filter == CommunitiesFilterOptions.Followed)
				{
					await GetFollowedCommunities();
				}
			}
		}

		public async void Init()
		{
			await GetCommunities();
		}
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

			foreach (var item in await _communityService.GetCommunitiesBySearch(text, 50, Communities.Select(x => x.Id).ToArray()))
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
			
			foreach (var item in await _communityService.GetCommunitiesByAccount(_authService.Id, 50, Communities.Count))
			{
				Communities.Add(item);
			}
			_filter = CommunitiesFilterOptions.Followed;
		}
		
	}
}
