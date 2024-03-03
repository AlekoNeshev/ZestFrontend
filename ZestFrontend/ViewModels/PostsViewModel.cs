using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZestFrontend.DTOs;
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
		public PostsViewModel(PostsService postsService, LikesService service, LikesHubConnectionService likesHubConnectionService, SignalRConnectionService signalRConnectionService,AuthService authService)
		{

			this.postsService = postsService;
			this.likesService = service;
			this.authService=authService;
			this.connection = likesHubConnectionService;
			_signalRConnectionService = signalRConnectionService;
			 this.connection.Init();
			GetPosts();


		}
		[ObservableProperty]
		string search;
		[ObservableProperty]
		bool isRefreshing;

		public ObservableCollection<PostDTO> Posts { get; } = new();
		public async void UpdatePost(int id)
		{
			var updatedPost = await postsService.GetSinglePost(id);
			var post = Posts.Where(x => x.Id==id).First();
			post.Likes = updatedPost.Likes;
			post.Dislikes = updatedPost.Dislikes;
		}
		public async void GetPosts()
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
			foreach (var post in await postsService.GetPosts(lastDate, Posts.Count, 50))
			{
				post.IsOwner = post.Publisher == authService.Username;
				Posts.Add(post);
			}
			

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

		public async void onNavigatedTo()
		{
			
			connection.LikesConnection.On<int>("PostLiked", UpdatePost);
			await _signalRConnectionService.AddConnectionToGroup(connection.LikesConnection.ConnectionId, Posts.Select(x => x.Id.ToString()).ToArray());
		}
	}
}

