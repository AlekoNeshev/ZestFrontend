using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
		CommentsHubConnectionService _commentHubConnectionService;
		private Task InitTask;
		PostsFilterOptions _filter;
		public PostsViewModel(PostsService postsService, LikesService service, LikesHubConnectionService likesHubConnectionService, SignalRConnectionService signalRConnectionService, AuthService authService, CommunityService communityService, CommentsHubConnectionService commentHubConnectionService)
		{

			this._postsService = postsService;
			this._likesService = service;
			this._authService=authService;
			this._likesConnection = likesHubConnectionService;
			this._communityService = communityService;
			_signalRConnectionService = signalRConnectionService;
			_filter = PostsFilterOptions.None;
			InitTask = Init();
			_commentHubConnectionService=commentHubConnectionService;
		}

		private async Task Init()
		{
			await GetPosts();

			await this._likesConnection.Init();
			await this._commentHubConnectionService.Init();
			_likesConnection.LikesConnection.On<int>("PostLiked", UpdatePost);
			_commentHubConnectionService.CommentsConnection.On<int>("PostDeleted", UpdatePost);
		}

		[ObservableProperty]
		string search;
		[ObservableProperty]
		bool isRefreshing;
		[ObservableProperty]
		bool isBtnVisible;
		[ObservableProperty]
		bool areFiltersVisible;


		public ObservableCollection<PostDTO> Posts { get; } = new();

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
			foreach (var post in await _postsService.GetPosts(lastDate, 0, 10))
			{
				post.IsOwner = post.Publisher == _authService.Username;
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Last;

		}
		public async Task GetTrendingPosts()
		{
			Posts.Clear();
			var skipIds = Posts.Select(x => x.Id).ToArray();


			foreach (var post in await _postsService.GetTrendingPostsAsync(50, 0, skipIds))
			{
				post.IsOwner = post.Publisher == _authService.Username;
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Trending;

		}
		public async Task GetFollowedPosts()
		{
			Posts.Clear();
			var skipIds = Posts.Select(x => x.Id).ToArray();




			foreach (var post in await _postsService.GetFollowedPostsAsync(50, skipIds))
			{
				post.IsOwner = post.Publisher == _authService.Username;
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Followed;


		}
		[RelayCommand]
		async Task GetLatestPostsAsync()
		{
			if (_filter != PostsFilterOptions.Last)
			{
				Posts.Clear();
				await GetPosts();
			}
		}
		[RelayCommand]
		async Task GetTrendingPostsAsync()
		{
			if (_filter != PostsFilterOptions.Trending)
			{
				await GetTrendingPosts();
			}
		}
		[RelayCommand]
		async Task GetFollowedPostsAsync()
		{
			if (_filter != PostsFilterOptions.Followed)
			{
				await GetFollowedPosts();
			}
		}

		[RelayCommand]
		async Task ShowFollowedComsAsync()
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
			else if(postDTO.Like.Value == true) 
			{
				await _likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
			}
			else if(postDTO.Like.Value == false)
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
		async Task SearchPosts(string text)
		{
			Posts.Clear();
			foreach (var item in await _postsService.GetPostsBySearch(text))
			{
				Posts.Add(item);
			}
			await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
		}
		[RelayCommand]
		async Task RefreshAsync()
		{
			Posts.Clear();
			GetPosts();
			IsRefreshing = false;
			await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
		}
		[RelayCommand]
		async Task FilterBtnAsync()
		{
			AreFiltersVisible = !AreFiltersVisible;
		}
		[RelayCommand]
		async Task LoadMorePostsAsync()
		{
			if (_filter == PostsFilterOptions.Last)
			{
				await GetPosts();
			}
			await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.TakeLast(10).Select(x => x.Id.ToString()).ToArray());
		}
		public async Task onNavigatedTo()
		{
			if (InitTask is not null && !InitTask.IsCompleted) await InitTask;

			await _signalRConnectionService.AddConnectionToGroup(_likesConnection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
		}
		public async Task OnNavigatedFrom()
		{
			await _signalRConnectionService.RemoveConnectionToGroup(_likesConnection.LikesConnection.ConnectionId);
		}
	}
}

