using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using ZestFrontend.DTOs;
using ZestFrontend.Filters;
using ZestFrontend.Pages;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    [QueryProperty(nameof(Community), "Community")]
    public partial class CommunityDetailsViewModel : ObservableObject
    {
        CommunityService _communityService;
        PostsService _postsService;
		LikesService _likesService;
		AuthService _authService;
		SignalRConnectionService _signalRConnectionService;
		LikesHubConnectionService _likesHubConnection;
		PostsFilterOptions _filter;
		public CommunityDetailsViewModel(CommunityService communityService, PostsService postsService, LikesService likesService, AuthService authService, LikesHubConnectionService likesHubConnectionService, SignalRConnectionService signalRConnectionService)
		{ 
			this._communityService = communityService;
			this._postsService = postsService;
			this._likesService=likesService;
			this._authService=authService;
			this._likesHubConnection = likesHubConnectionService;
			_signalRConnectionService = signalRConnectionService;
			_filter = PostsFilterOptions.None;
			
			_likesHubConnection.LikesConnection.On<int>("PostLiked", UpdatePost);
		}
		[ObservableProperty]
		string searchText;

		private bool isInSearchMode;

		public bool IsInSearchMode
		{
			get { return isInSearchMode; }
			set { isInSearchMode = value; }
		}
		async partial void OnCommunityChanged(CommunityDTO value)
		{
			if (value.IsSubscribed)
			{
				ButtonText = "Unfollow";
			}
			else
			{
				ButtonText = "Follow";
			}
			Posts.Clear();
			await GetPosts();
		}
		public async void UpdatePost(int id)
		{
			var updatedPost = await _postsService.GetSinglePost(id);
			var post = Posts.Where(x => x.Id==id).FirstOrDefault();
			if(post != null)
			{
				post.Likes = updatedPost.Likes;
				post.Dislikes = updatedPost.Dislikes;
				post.Like = updatedPost.Like;
			}	
		}
	
		[ObservableProperty]
		CommunityDTO community;
		[ObservableProperty]
		string buttonText;
		[ObservableProperty]
		bool isRefreshing;
		[ObservableProperty]
		bool areFiltersVisible;
		public ObservableCollection<PostDTO> Posts { get; private set; } = new();

		[RelayCommand]
        async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync(nameof(CommunitiesPage));
        }
		[RelayCommand]
		async Task DislikePostAsync(PostDTO postDTO)
		{
			if (postDTO.Like == null)
			{

				await _likesService.Like(postDTO.Id, 0, false);
			}
			else if (postDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
			}
			else if (postDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
				await _likesService.Like(postDTO.Id, 0, false);
			}
		}
		[RelayCommand]
		async Task LikePostAsync(PostDTO postDTO)
		{
			if (postDTO.Like == null)
			{

				await _likesService.Like(postDTO.Id, 0, true);
			}
			else if (postDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
			}
			else if (postDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
				await _likesService.Like(postDTO.Id, 0, true);
			}
		}
		[RelayCommand]
		async Task DeletePostAsync(PostDTO postDTO)
		{
			await _postsService.DeletePost(postDTO.Id);
		}
		[RelayCommand]
		async Task ChangeFollowshipStatusAsync()
		{
			if (Community.IsSubscribed)
			{
				var result = await _communityService.Unfollow(Community.Id);
				if (result.StatusCode == HttpStatusCode.OK)
				{
					ButtonText = "Follow";
					Community.IsSubscribed = false;
				}
			}
			else
			{
				var result = await _communityService.Follow(Community.Id);
				if (result.IsSuccessStatusCode)
				{
					ButtonText = "Unfollow";
					Community.IsSubscribed = true;
				}
			}
		}
		[RelayCommand]
		async Task AddPostAsync()
		{
			if (Community== null) return;

			await Shell.Current.GoToAsync($"{nameof(AddPostPage)}?id={Community.Name}", true,
				new Dictionary<string, object>
			{
			{"Community", Community }
			});
		}

		[RelayCommand]
		async Task GoToPostDetailPageAsync(PostDTO post)
		{
			if (post == null) return;

			await Shell.Current.GoToAsync($"{nameof(PostDetailsPage)}?id={post.Id}", true,
				new Dictionary<string, object>
			{
			{"Post", post }
			});
		}
		[RelayCommand]
		async Task GoToModeratorsDetailPageAsync()
		{
			if (Community == null) return;

			await Shell.Current.GoToAsync($"{nameof(CommunityModeratorsPage)}?id={Community.Id}", true,
				new Dictionary<string, object>
			{
			{"Community", Community }
			});
		}
		[RelayCommand]
		async Task LoadMorePostsAsync()
		{
			if (Posts.Count > 0)
			{

				if (!string.IsNullOrWhiteSpace(SearchText) && IsInSearchMode == true)
				{
					await SearchPosts();
				}
				else if (_filter == PostsFilterOptions.Last)
				{
					await GetPosts();
				}
				else if (_filter == PostsFilterOptions.Trending)
				{
					await GetTrendingPosts();
				}
				if (_likesHubConnection.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.AddConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId, Posts.TakeLast(40).Select(x => x.Id.ToString()).ToArray());
				}

			}
			_authService.Groups.AddRange(Posts.TakeLast(40).Select(x => x.Id.ToString()).ToList());
		}
		[RelayCommand]
		async Task RefreshAsync()
		{
			Posts.Clear();
			if (_filter == PostsFilterOptions.Last)
			{
				await GetPosts();
			}
			else if (_filter == PostsFilterOptions.Trending)
			{
				await GetTrendingPosts();
			}

			IsRefreshing = false;
			if (_likesHubConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.RemoveConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId);
				await _signalRConnectionService.AddConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
			}
			_authService.Groups.Clear();
			_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
		}
		[RelayCommand]
		void FilterBtn()
		{
			AreFiltersVisible = !AreFiltersVisible;
		}
		[RelayCommand]
		async Task GetLatestPostsAsync()
		{
			if (_filter != PostsFilterOptions.Last || IsInSearchMode == true)
			{
				Posts.Clear();
				await GetPosts();
				IsInSearchMode = false;
				if (_likesHubConnection.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.RemoveConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId);
					await _signalRConnectionService.AddConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
				}
				_authService.Groups.Clear();
				_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
			}
		}
		[RelayCommand]
		async Task GetTrendingPostsAsync()
		{
			if (_filter != PostsFilterOptions.Trending || IsInSearchMode == true)
			{
				Posts.Clear();
				IsInSearchMode = false;
				await GetTrendingPosts();
				if (_likesHubConnection.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.RemoveConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId);
					await _signalRConnectionService.AddConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
				}
				_authService.Groups.Clear();
				_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
			}
		}
		[RelayCommand]
		async Task SearchPostsAsync()
		{
			if (!string.IsNullOrWhiteSpace(SearchText))
			{
				Posts.Clear();
				await SearchPosts();
				IsInSearchMode = true;
			}
			else if (_filter == PostsFilterOptions.Last)
			{
				Posts.Clear();
				await GetPosts();
			}
			else if (_filter == PostsFilterOptions.Trending)
			{
				Posts.Clear();
				await GetTrendingPosts();
			}

			if (_likesHubConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.AddConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
			}
			_authService.Groups.Clear();
			_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
		}
		[RelayCommand]
		async Task DeleteCommunityAsync(int communityId)
		{
			await DeleteCommuity(communityId);
			await Shell.Current.GoToAsync("..");
		}
		public async Task GetPosts()
		{
			DateTime lastDate = new DateTime();
			if (Posts.Count==0)
			{
				lastDate = DateTime.Now;
			}
			else
			{
				lastDate = Posts.Last().PostedOn;
			}
			foreach (var post in await _postsService.GetPosts(lastDate, Community.Id, 20))
			{
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Last;
		}
		public async Task GetTrendingPosts()
		{
			Posts.Clear();
			var skipIds = Posts.Select(x => x.Id).ToArray();

			foreach (var post in await _postsService.GetTrendingPostsAsync(20, Community.Id, skipIds))
			{
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Trending;

		}
		public async Task SearchPosts()
		{
			foreach (var item in await _postsService.GetPostsBySearch(SearchText, 40, Community.Id, Posts.Select(x => x.Id).ToArray()))
			{
				Posts.Add(item);
			}
		}
		public async Task DeleteCommuity(int communityId)
		{
			await _communityService.DeleteCommunity(communityId);
		}
		
		
		public async Task onNavigatedTo()
		{
			
			if (_likesHubConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.AddConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
			}
			_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
		}
		public async Task OnNavigatedFrom()
		{
			if (_likesHubConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.RemoveConnectionToGroup(_likesHubConnection.LikesConnection.ConnectionId);
			}
			_authService.Groups.Clear();
		}
	}
}
