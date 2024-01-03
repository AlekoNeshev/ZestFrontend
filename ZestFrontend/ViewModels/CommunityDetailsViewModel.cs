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
		HubConnection connection;
		public CommunityDetailsViewModel(CommunityService communityService, PostsService postsService, LikesService likesService, AuthService authService)
		{
			this.communityService = communityService;
			this.postsService = postsService;
			this.likesService=likesService;
			this.authService=authService;
			connection = new HubConnectionBuilder().WithUrl("https://localhost:7183/likeshub").Build();
			connection.On<int>("SignalLike", (id) => UpdatePost(id));
			connection.StartAsync();
		}
		public async void UpdatePost(int id)
		{
			var updatedPost = await postsService.GetSinglePost(id);
			var post = Posts.Where(x => x.Id==id).First();
			post.Likes = updatedPost.Likes;
			post.Dislikes = updatedPost.Dislikes;
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
			await likesService.Like(authService.Id, postDTO.Id, 0, false);
		}
		[RelayCommand]
		async Task LikePostAsync(PostDTO postDTO)
		{
			await likesService.Like(authService.Id, postDTO.Id, 0, true);
		}
		public async void GetComments()
		{
			Posts.Clear();
			var posts = await postsService.GetPostsByCommunity(Community.Id);
			foreach (var post in posts)
			{
				Posts.Add(post);
			}
		}
		partial void OnCommunityChanged(CommunityDTO value)
		{
			if (value.IsSubscribed)
			{
				ButtonText = "Unfollow";
			}
			else
			{
				ButtonText = "Follow";
			}
			GetComments();
		}
		[RelayCommand]
		async Task ChangeFollowshipStatusAsync()
		{
			if (Community.IsSubscribed)
			{
				
				var result = await communityService.Unfollow(authService.Id, Community.Id);
				if (result.StatusCode == HttpStatusCode.OK)
				{
					ButtonText = "Follow";
					Community.IsSubscribed = false;
				}
			}
			else
			{

				var result = await communityService.Follow(authService.Id, Community.Id);
				if (result.IsSuccessStatusCode)
				{
					ButtonText = "Unfollow";
					Community.IsSubscribed = true;
				}
			}
		}
		[RelayCommand]
		async Task AddPost()
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
	}
}
