using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Post), "Post")]
	public partial class PostDetailsViewModel : ObservableObject
	{
		AuthService authService;
		PostsService postsService;
		LikesService likesService;
		HubConnection connection;
		HubConnection likesConnection;
		CommentService commentService;
		MediaService mediaService;
		int repliedId = -1;
		public PostDetailsViewModel(AuthService authService, PostsService postsService, LikesService likesService, CommentService commentService, MediaService mediaService)
		{
			this.authService = authService;
			this.postsService = postsService;
			this.likesService = likesService;
			this.commentService = commentService;
			this.mediaService = mediaService;
			connection = new HubConnectionBuilder().WithUrl("https://localhost:7183/commentshub").Build();
			likesConnection = new HubConnectionBuilder().WithUrl("https://localhost:7183/likeshub").Build();
			connection.On("CommentPosted", GetComments);
			likesConnection.On<int>("CommentLiked", UpdateComment);
			connection.StartAsync();
			likesConnection.StartAsync();
		
		}
		[ObservableProperty]
		PostDTO post;
		[ObservableProperty]
		string source;
		public ObservableCollection<CommentDTO> Comments { get; private set; } = new();
		[ObservableProperty]
		bool isBusy;
		[ObservableProperty]
		bool isCarouselVisible;
		[ObservableProperty]
		bool isMediaPlayerVisible;
		[ObservableProperty]
		string replyText;
		public ObservableCollection<PostResourcesDTO> Resources { get; private set; } = new();

		[RelayCommand]
		async Task DislikePostAsync()
		{
			await likesService.Like(authService.Id, Post.Id, 0, false);
		}
		[RelayCommand]
		async Task LikePostAsync()
		{
			await likesService.Like(authService.Id, Post.Id, 0, true);
		}

		public async void GetComments()
		{
			Comments.Clear();
			var comments = await commentService.GetComments(Post.Id);
			foreach (var comment in comments)
			{
				Comments.Add(comment);
			}


			//Source = ("https://localhost:7183/api/PostRescources/ivan/19e1ca76-f360-4dc4-b940-71c329c8ea8b.jpg");
			//Source = MediaSource.FromUri("https://localhost:7183/api/PostRescources/ivan/download.jpg");

		}
		[RelayCommand]
		async Task SendAsync(string text)
		{
			
		 await commentService.PostComment(Post.Id, authService.Id, text);
			
		}
		partial void OnPostChanged(PostDTO value)
		{
			GetComments();
			DealWithResource();
		}
		[RelayCommand]
		async Task RefreshAsync()
		{
		    GetComments();
		}
		[RelayCommand]
		async Task GoBackAsync()
		{
			await Shell.Current.GoToAsync("..");
		}
		[RelayCommand]
		async Task LikeCommentAsync(CommentDTO commentDTO)
		{
			await likesService.Like(authService.Id, 0, commentDTO.Id, true);
		}
		[RelayCommand]
		async Task DislikeCommentAsync(CommentDTO commentDTO)
		{
			await likesService.Like(authService.Id, 0, commentDTO.Id, false);
		}
		[RelayCommand]
		async Task ReplyCommentAsync(CommentDTO comment)
		{
			// Toggle the visibility of the reply field
			comment.IsReplyVisible = !comment.IsReplyVisible;
			repliedId = comment.Id;
		}
		[RelayCommand]
		async Task SendReplyAsync(string text)
		{
			await commentService.PostComment(Post.Id, authService.Id, text, repliedId);
			repliedId = -1;
		}

		public async void UpdateComment(int id)
		{
			var updatedComment = await commentService.GetSingleComment(id);
			var comment = Comments.Where(x => x.Id==id).First();
			comment.Likes = updatedComment.Likes;
			comment.Dislikes = updatedComment.Dislikes;
		}
		[RelayCommand]
		async Task DeleteCommentAsync(CommentDTO commentDTO)
		{
			await commentService.DeleteComment(commentDTO.Id);
		}
		public async void DealWithResource()
		{
			Resources.Clear();
			if(Post.ResourceType == null)
			{
				IsMediaPlayerVisible = false;
				IsCarouselVisible = false;
				return;
			}
			if (Post.ResourceType.Trim()=="image")
			{
				IsCarouselVisible = true;
				IsMediaPlayerVisible = false;
				var results = await mediaService.GetPhotosByPostId(Post.Id);


				foreach (var result in results)
				{
					Resources.Add(result);
				}
			}
			else if (Post.ResourceType.Trim()=="video")
			{
				IsCarouselVisible = false;
				IsMediaPlayerVisible = true;
				var results = await mediaService.GetPhotosByPostId(Post.Id);


				Source = results.First().Source;
			}
		    else
			{
				IsMediaPlayerVisible = false;
				IsCarouselVisible = false;
				return;
			}
		}
	}
}
