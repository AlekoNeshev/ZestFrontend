using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiIcons.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using ZestFrontend.DTOs;
using ZestFrontend.Filters;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	public partial class PostsViewModel : ObservableObject
	{
		PostsService _postsService;
		LikesHubConnectionService _likesConnection;
		LikesService _likesService;
		AuthService _authService;
		SignalRConnectionService _signalRConnectionService;
		CommunityService _communityService;
		DeleteHubConnectionService _commentHubConnectionService;
		PostsFilterOptions _filter;
		public PostsViewModel(PostsService postsService, LikesService service, LikesHubConnectionService likesHubConnectionService, SignalRConnectionService signalRConnectionService, AuthService authService, CommunityService communityService, DeleteHubConnectionService commentHubConnectionService)
		{
			this._likesConnection = likesHubConnectionService;
			_commentHubConnectionService=commentHubConnectionService;
			this._likesConnection.Init();
			this._commentHubConnectionService.Init();
			_likesConnection.LikesConnection.On<int>("PostLiked", UpdatePost);
			_commentHubConnectionService.DeleteConnection.On<int>("PostDeleted", UpdatePost);
			this._postsService = postsService;
			this._likesService = service;
			this._authService=authService;
		
			this._communityService = communityService;
			_signalRConnectionService = signalRConnectionService;
			_filter = PostsFilterOptions.None;
			Initilaze();
		
			_ = new MauiIcon();
			IsInSearchMode = false;
			
		}
	
		[ObservableProperty]
		string search;
		[ObservableProperty]
		bool isRefreshing;
		[ObservableProperty]
		bool isBtnVisible;
		[ObservableProperty]
		bool areFiltersVisible;
		[ObservableProperty]
		string searchText;

		private bool isInSearchMode;

		public bool IsInSearchMode
		{
			get { return isInSearchMode; }
			set { isInSearchMode = value; }
		}

		public ObservableCollection<PostDTO> Posts { get; } = new();

		
		[RelayCommand]
		async Task GetLatestPostsAsync()
		{
			if (_filter != PostsFilterOptions.Last || IsInSearchMode == true)
			{
				Posts.Clear();
				await GetPosts();
				if (_likesConnection.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.RemoveConnectionToGroup(_likesConnection.LikesConnection.ConnectionId);
					await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
				}
				SearchText = string.Empty;
				IsInSearchMode = false;
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
				await GetTrendingPosts();
				if (_likesConnection.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.RemoveConnectionToGroup(_likesConnection.LikesConnection.ConnectionId);
					await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
				}
				IsInSearchMode = false;
				SearchText = string.Empty;
				_authService.Groups.Clear();
				_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
			}
		}
		[RelayCommand]
		async Task GetFollowedPostsAsync()
		{
			if (_filter != PostsFilterOptions.Followed || IsInSearchMode == true)
			{
				Posts.Clear();
				await GetFollowedPosts();
				if (_likesConnection.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.RemoveConnectionToGroup(_likesConnection.LikesConnection.ConnectionId);
					await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
				}
				IsInSearchMode = false;
				SearchText = string.Empty;
				_authService.Groups.Clear();
				_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
			}
		}

		[RelayCommand]
		void ShowFollowedComs()
		{
			IsBtnVisible = !IsBtnVisible;
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
			else if (_filter == PostsFilterOptions.Followed)
			{
				Posts.Clear();
				await GetFollowedPosts();
			}
			if (_likesConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
			}
			_authService.Groups.Clear();
			_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
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
			else if (_filter == PostsFilterOptions.Followed)
			{
				await GetFollowedPosts();
			}
			IsRefreshing = false;
			if (_likesConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.RemoveConnectionToGroup(_likesConnection.LikesConnection.ConnectionId);
				await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
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
				else if (_filter == PostsFilterOptions.Followed)
				{
					await GetFollowedPosts();
				}
				if (_likesConnection.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.TakeLast(40).Select(x => x.Id.ToString()).ToArray());
				}
				
				}
				_authService.Groups.AddRange(Posts.TakeLast(40).Select(x => x.Id.ToString()).ToList());
		}
		public async void UpdatePost(int id)
		{
			var updatedPost = await _postsService.GetSinglePost(id);
			var post = Posts.Where(x => x.Id==id).FirstOrDefault();
			if (post != null)
			{
				post.Likes = updatedPost.Likes;
				post.Dislikes = updatedPost.Dislikes;
				post.Like = updatedPost.Like;
				post.Title = updatedPost.Title;
				post.Text = updatedPost.Text;
				post.Publisher = updatedPost.Publisher;
			}
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
			foreach (var post in await _postsService.GetPosts(lastDate, 0, 40))
			{
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Last;

		}
		
		public async Task GetTrendingPosts()
		{

			var skipIds = Posts.Select(x => x.Id).ToArray();


			foreach (var post in await _postsService.GetTrendingPostsAsync(40, 0, skipIds))
			{

				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Trending;

		}
		public async Task GetFollowedPosts()
		{

			var skipIds = Posts.Select(x => x.Id).ToArray();

			foreach (var post in await _postsService.GetFollowedPostsAsync(40, skipIds))
			{
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Followed;


		}
		public async Task SearchPosts()
		{
			foreach (var item in await _postsService.GetPostsBySearch(SearchText, 40, 0, Posts.Select(x => x.Id).ToArray()))
			{
				Posts.Add(item);
			}
		}
		public async void Initilaze()
		{
			await GetPosts();
			
		}
		public async Task onNavigatedTo()
		{
			while (_likesConnection.LikesConnection.ConnectionId == null)
			{
				await Task.Delay(100); 
			}
			
			if (_likesConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
			}
			_authService.Groups.AddRange(Posts.Select(x => x.Id.ToString()).ToList());
		}
		public async Task OnNavigatedFrom()
		{
			if (_likesConnection.LikesConnection.ConnectionId != null)
			{
				await _signalRConnectionService.RemoveConnectionToGroup(_likesConnection.LikesConnection.ConnectionId);
			}
			_authService.Groups.Clear();
		}
	}
}

