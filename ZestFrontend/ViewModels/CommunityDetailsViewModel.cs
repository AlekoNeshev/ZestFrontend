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
        CommunityService communityService;
        PostsService postsService;
		LikesService likesService;
		AuthService authService;
		SignalRConnectionService _signalRConnectionService;
		LikesHubConnectionService connection;
		private Task InitTask;
		PostsFilterOptions _filter;
		public CommunityDetailsViewModel(CommunityService communityService, PostsService postsService, LikesService likesService, AuthService authService, LikesHubConnectionService likesHubConnectionService, SignalRConnectionService signalRConnectionService)
		{ 
			this.communityService = communityService;
			this.postsService = postsService;
			this.likesService=likesService;
			this.authService=authService;
			this.connection = likesHubConnectionService;
			_signalRConnectionService = signalRConnectionService;
			_filter = PostsFilterOptions.None;
			InitTask = Init();
		}
		private async Task Init()
		{
			
			connection.LikesConnection.On<int>("PostLiked", UpdatePost);
		}

		public async void UpdatePost(int id)
		{
			var updatedPost = await postsService.GetSinglePost(id);
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

				await likesService.Like(postDTO.Id, 0, false);
			}
			else if (postDTO.Like.Value == false)
			{
				await likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
			}
			else if (postDTO.Like.Value == true)
			{
				await likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
				await likesService.Like(postDTO.Id, 0, false);
			}
		}
		[RelayCommand]
		async Task LikePostAsync(PostDTO postDTO)
		{
			if (postDTO.Like == null)
			{

				await likesService.Like(postDTO.Id, 0, true);
			}
			else if (postDTO.Like.Value == true)
			{
				await likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
			}
			else if (postDTO.Like.Value == false)
			{
				await likesService.RemoveLike(postDTO.Like.Id, postDTO.Id, 0);
				await likesService.Like(postDTO.Id, 0, true);
			}
		}
		[RelayCommand]
		async Task DeletePostAsync(PostDTO postDTO)
		{
			await postsService.DeletePost(postDTO.Id);
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
			foreach (var post in await postsService.GetPosts(lastDate, Community.Id, 10))
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


			foreach (var post in await postsService.GetTrendingPostsAsync(50, Community.Id, skipIds))
			{
				post.IsOwner = post.Publisher == authService.Username;
				Posts.Add(post);
			}
			_filter = PostsFilterOptions.Trending;

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
		[RelayCommand]
		async Task ChangeFollowshipStatusAsync()
		{
			if (Community.IsSubscribed)
			{
				
				var result = await communityService.Unfollow(Community.Id);
				if (result.StatusCode == HttpStatusCode.OK)
				{
					ButtonText = "Follow";
					Community.IsSubscribed = false;
				}
			}
			else
			{

				var result = await communityService.Follow(Community.Id);
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
			await GetPosts();
			await _signalRConnectionService.AddConnectionToGroup(connection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
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
