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
		AuthService _authService;
		PostsService _postsService;
		LikesService _likesService;
		SignalRConnectionService _signalRConnectionService;
		LikesHubConnectionService _likesHubConnectionService;
		CommentsHubConnectionService _commentHubConnectionService;
		CommentService _commentService;
		MediaService _mediaService;
		int repliedId = -1;
		public PostDetailsViewModel(AuthService authService, PostsService postsService, LikesService likesService, CommentService commentService, MediaService mediaService, LikesHubConnectionService likesHubConnectionService, CommentsHubConnectionService commentHubConnectionService, SignalRConnectionService signalRConnectionService)
		{
			_authService = authService;
			_postsService = postsService;
			_likesService = likesService;
			_commentService = commentService;
			_mediaService = mediaService;
			_likesHubConnectionService=likesHubConnectionService;
			_commentHubConnectionService=commentHubConnectionService;
			_signalRConnectionService = signalRConnectionService;
			_commentHubConnectionService.Init();
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
			await _likesService.Like(_authService.Id, Post.Id, 0, false);
		}
		[RelayCommand]
		async Task LikePostAsync()
		{
			await _likesService.Like(_authService.Id, Post.Id, 0, true);

		}
		[RelayCommand]
		async Task DeletePostAsync()
		{
			await _postsService.DeletePost(Post.Id);
		}

		public async void GetComments()
		{
			Comments.Clear();
			var comments = await _commentService.GetComments(Post.Id);
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
			
		 await _commentService.PostComment(Post.Id, _authService.Id, text);
			
		}
		partial void OnPostChanged(PostDTO value)
		{
			GetComments();
			DealWithResource();
			
			_commentHubConnectionService.CommentsConnection.On("CommentPosted", GetComments);
			_likesHubConnectionService.LikesConnection.On<int>("CommentLiked", UpdateComment);
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
			await _likesService.Like(_authService.Id, Post.Id, commentDTO.Id, true);
		}
		[RelayCommand]
		async Task DislikeCommentAsync(CommentDTO commentDTO)
		{
			await _likesService.Like(_authService.Id, Post.Id, commentDTO.Id, false);
		}
		[RelayCommand]
		async Task ReplyCommentAsync(CommentDTO comment)
		{
			comment.IsReplyVisible = !comment.IsReplyVisible;
			repliedId = comment.Id;
		}
		[RelayCommand]
		async Task SendReplyAsync(string text)
		{
			await _commentService.PostComment(Post.Id, _authService.Id, text, repliedId);
			repliedId = -1;
		}
		public CommentDTO FindCommentById(int id, IEnumerable<CommentDTO> comments)
		{
			foreach (var comment in comments)
			{
				if (comment.Id == id)
				{
					return comment; 
				}

				var foundInReplies = FindCommentById(id, comment.Replies);
				if (foundInReplies != null)
				{
					return foundInReplies; 
				}
			}

			return null; 
		}
		public async void UpdateComment(int id)
		{
			var updatedComment = await _commentService.GetSingleComment(id);
			var comment = FindCommentById(id, Comments);
			comment.Likes = updatedComment.Likes;
			comment.Dislikes = updatedComment.Dislikes;
		}
		[RelayCommand]
		async Task DeleteCommentAsync(CommentDTO commentDTO)
		{
			await _commentService.DeleteComment(commentDTO.Id);
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
				var results = await _mediaService.GetPhotosByPostId(Post.Id);


				foreach (var result in results)
				{
					Resources.Add(result);
				}
			}
			else if (Post.ResourceType.Trim()=="video")
			{
				IsCarouselVisible = false;
				IsMediaPlayerVisible = true;
				var results = await _mediaService.GetPhotosByPostId(Post.Id);


				Source = results.First().Source;
			}
		    else
			{
				IsMediaPlayerVisible = false;
				IsCarouselVisible = false;
				return;
			}
		}
		
		public async void OnNavigatedTo()
		{
			await _signalRConnectionService.RemoveConnectionToGroup(_likesHubConnectionService.LikesConnection.ConnectionId);
			await _signalRConnectionService.RemoveConnectionToGroup(_commentHubConnectionService.CommentsConnection.ConnectionId);
			await _signalRConnectionService.AddConnectionToGroup(_likesHubConnectionService.LikesConnection.ConnectionId, new string[] { $"pd-{Post.Id}", Post.Id.ToString() });
			await _signalRConnectionService.AddConnectionToGroup(_commentHubConnectionService.CommentsConnection.ConnectionId, new string[] { $"message-{Post.Id}" });
		}
		
	}
}
