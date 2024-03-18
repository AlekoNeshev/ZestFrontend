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
		PostsService postsService;
		LikesHubConnectionService connection;
		LikesService likesService;
		AuthService authService;
		SignalRConnectionService _signalRConnectionService;
		CommunityService _communityService;

		private Task InitTask;
		PostsFilterOptions _filter;
		public PostsViewModel(PostsService postsService, LikesService service, LikesHubConnectionService likesHubConnectionService, SignalRConnectionService signalRConnectionService, AuthService authService, CommunityService communityService)
		{

			this.postsService = postsService;
			this.likesService = service;
			this.authService=authService;
			this.connection = likesHubConnectionService;
			this._communityService = communityService;
			_signalRConnectionService = signalRConnectionService;
			_filter = PostsFilterOptions.None;
			InitTask = Init();

		}

		private async Task Init()
		{
			await GetPosts();

			await this.connection.Init();
			connection.LikesConnection.On<int>("PostLiked", UpdatePost);
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
			var updatedPost = await postsService.GetSinglePost(id);
			var post = Posts.Where(x => x.Id==id).FirstOrDefault();
			if (post != null)
			{
				post.Likes = updatedPost.Likes;
				post.Dislikes = updatedPost.Dislikes;
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
			foreach (var post in await postsService.GetPosts(lastDate, 0, 10))
			{
				post.IsOwner = post.Publisher == authService.Username;
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Last;

		}
		public async Task GetTrendingPosts()
		{
			Posts.Clear();
			var skipIds = Posts.Select(x => x.Id).ToArray();


			foreach (var post in await postsService.GetTrendingPostsAsync(50, 0, skipIds))
			{
				post.IsOwner = post.Publisher == authService.Username;
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Trending;

		}
		public async Task GetFollowedPosts()
		{
			Posts.Clear();
			var skipIds = Posts.Select(x => x.Id).ToArray();




			foreach (var post in await postsService.GetFollowedPostsAsync(50, skipIds))
			{
				post.IsOwner = post.Publisher == authService.Username;
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
			await likesService.Like(postDTO.Id, 0, false);
		}
		[RelayCommand]
		async Task LikePostAsync(PostDTO postDTO)
		{
			await likesService.Like(postDTO.Id, 0, true);
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
		async Task SearchPosts()
		{
			Posts.Clear();
			foreach (var item in await postsService.GetPostsBySearch(Search))
			{
				Posts.Add(item);
			}
			await _signalRConnectionService.AddConnectionToGroup(connection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
		}
		[RelayCommand]
		async Task RefreshAsync()
		{
			Posts.Clear();
			GetPosts();
			IsRefreshing = false;
			await _signalRConnectionService.AddConnectionToGroup(connection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
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
		}
		public async Task onNavigatedTo()
		{
			if (InitTask is not null && !InitTask.IsCompleted) await InitTask;

			await _signalRConnectionService.AddConnectionToGroup(connection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
		}
		public async Task OnNavigatedFrom()
		{
			await _signalRConnectionService.RemoveConnectionToGroup(connection.LikesConnection.ConnectionId);
		}
	}
}

