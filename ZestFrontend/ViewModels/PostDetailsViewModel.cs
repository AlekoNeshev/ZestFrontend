﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiIcons.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ZestFrontend.DTOs;
using ZestFrontend.Filters;
using ZestFrontend.Pages;
using ZestFrontend.Parameters;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Post), "Post")]
	public partial class PostDetailsViewModel : ObservableObject, INotifyPropertyChanged
	{
		AuthService _authService;
		PostsService _postsService;
		LikesService _likesService;
		SignalRConnectionService _signalRConnectionService;
		LikesHubConnectionService _likesHubConnectionService;
		CommentsHubConnectionService _commentHubConnectionService;
		CommentService _commentService;
		MediaService _mediaService;
		CommentFilter _filter;
		private Task TaskInit;
		
		public PostDetailsViewModel( PostsService postsService, LikesService likesService, CommentService commentService, MediaService mediaService, LikesHubConnectionService likesHubConnectionService, CommentsHubConnectionService commentHubConnectionService, SignalRConnectionService signalRConnectionService, AuthService authService)
		{
			_authService = authService;
			_postsService = postsService;
			_likesService = likesService;
			_commentService = commentService;
			_mediaService = mediaService;
			_likesHubConnectionService=likesHubConnectionService;
			_commentHubConnectionService=commentHubConnectionService;
			_signalRConnectionService = signalRConnectionService;
			TaskInit = Init();
			ReplyCommand = new ReplyCommand(ExecuteReplyCommand);
			_ = new MauiIcon();
			_filter = CommentFilter.All;
		}
		private async Task Init()
		{
			
			
			_commentHubConnectionService.CommentsConnection.On<int>("CommentDeleted", UpdateComment);
			
			_likesHubConnectionService.LikesConnection.On<int>("CommentLiked", UpdateComment);

		}
		public CarouselView Carousel {  get; set; }
		public ICommand ReplyCommand { get; }
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
		bool isRefreshing;
		[ObservableProperty]
		string filter;

		[ObservableProperty]
		string replyText;
		public ObservableCollection<PostResourcesDTO> Resources { get; private set; } = new();


		[RelayCommand]
		async Task LikePostAsync()
		{
			if (Post.Like == null)
			{

				await _likesService.Like(Post.Id, 0, true);
			}
			else if (Post.Like.Value == true)
			{
				await _likesService.RemoveLike(Post.Like.Id, Post.Id, 0);
			}
			else if (Post.Like.Value == false)
			{
				await _likesService.RemoveLike(Post.Like.Id, Post.Id, 0);
				await _likesService.Like(Post.Id, 0, true);
			}
		}
		[RelayCommand]
		async Task DislikePostAsync()
		{
			if (Post.Like == null)
			{

				await _likesService.Like(Post.Id, 0, false);
			}
			else if (Post.Like.Value == false)
			{
				await _likesService.RemoveLike(Post.Like.Id, Post.Id, 0);
			}
			else if (Post.Like.Value == true)
			{
				await _likesService.RemoveLike(Post.Like.Id, Post.Id, 0);
				await _likesService.Like(Post.Id, 0, false);
			}
		}
		[RelayCommand]
		async Task DeletePostAsync()
		{
			var response = await _postsService.DeletePost(Post.Id);
			
			
		}

		public async Task GetComments()
		{
		
			DateTime lastDate = new DateTime();
			if (Comments.Count==0)
			{
				lastDate = DateTime.Now;
			}
			else
			{
				lastDate = Comments.Last().PostedOn;
			}
			var comments = await _commentService.GetComments(Post.Id, lastDate, 5);
			if (comments != null)
			{
				foreach (var comment in comments.Reverse())
				{
					comment.IsOwner = comment.Publisher==_authService.Username;
					await IsOwner(comment.Replies, 0);
					Comments.Add(comment);
					
				}
			}
			_filter = CommentFilter.All;
			Filter = _filter.ToString();
		}
		public async Task GetTrendingComments()
		{
			Comments.Clear();
			var skipIds = Comments.Select(x => x.Id).ToArray();


			foreach (var comment in await _commentService.GetTrendingPostsAsync(50, Post.Id, skipIds))
			{
				comment.IsOwner = comment.Publisher==_authService.Username;
				await IsOwner(comment.Replies, 0);
				Comments.Add(comment);
			}
			_filter = CommentFilter.Trending;
			Filter = _filter.ToString();

		}
		public async Task IsOwner(IEnumerable<CommentDTO> comments, int level)
		{
			foreach (var comment in comments)
			{
				
				comment.IsOwner = comment.Publisher==_authService.Username;
				comment.AreRepliesVisible = level<=4;
				if(comment.Replies == null)
				{
					return;
				}
				await IsOwner(comment.Replies, level + 1);
				
			}

		}
		[RelayCommand]
		async Task SendAsync(string text)
		{
			
		 var response = await _commentService.PostComment(Post.Id, text);
			var content = await response.Content.ReadAsStringAsync();
			AddComment(int.Parse(content));

		}
		async partial void OnPostChanged(PostDTO value)
		{
			Comments.Clear();
			await GetComments();
			DealWithResource();

		}
		
		[RelayCommand]
		async Task RefreshAsync()
		{
		   await GetComments();
			IsRefreshing = false;
		}
		[RelayCommand]
		async Task GoBackAsync()
		{
			await Shell.Current.GoToAsync("..");
		}
		[RelayCommand]
		async Task LikeCommentAsync(CommentDTO commentDTO)
		{
			if (commentDTO.Like == null)
			{

				await _likesService.Like(Post.Id, commentDTO.Id, true);
			}
			else if (commentDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, Post.Id, commentDTO.Id);
			}
			else if (commentDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, Post.Id, commentDTO.Id);
				await _likesService.Like(Post.Id, commentDTO.Id, true);
			}
		}
		[RelayCommand]
		async Task DislikeCommentAsync(CommentDTO commentDTO)
		{
			if (commentDTO.Like == null)
			{

				await _likesService.Like(Post.Id, commentDTO.Id, false);
			}
			else if (commentDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, Post.Id, commentDTO.Id);
			}
			else if (commentDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, Post.Id, commentDTO.Id);
				await _likesService.Like(Post.Id, commentDTO.Id, false);
			}
		}
		[RelayCommand]
		async Task ReplyCommentAsync(CommentDTO comment)
		{
			comment.IsReplyVisible = !comment.IsReplyVisible;
			
		}
		
		public async Task SendReplyAsync(int comment, string text)
		{
			var response = await _commentService.PostComment(Post.Id, text, comment);

			
			var content = await response.Content.ReadAsStringAsync();
			string[] parts = content.Trim('[', ']').Split(',');
			int firstNumber = int.Parse(parts[0]);
			int secondNumber = int.Parse(parts[1]);
			var reply = await _commentService.GetSingleComment(firstNumber);
			reply.IsOwner = reply.Publisher == _authService.Username;
			var commentToFind = FindCommentById(secondNumber, Comments);
			commentToFind.Replies.Add(reply);

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
			comment.Like = updatedComment.Like;
			comment.Text = updatedComment.Text;
			comment.Publisher = updatedComment.Publisher;
		}
		public async void AddComment(int id)
		{
			var comment = await _commentService.GetSingleComment(id);
			Comments.Add(comment);
		}
		[RelayCommand]
		async Task DeleteCommentAsync(CommentDTO commentDTO)
		{
			await _commentService.DeleteComment(commentDTO.Id);
		}
		public async void DealWithResource()
		{
			Resources.Clear();
			if (Post.ResourceType == null)
			{
				IsMediaPlayerVisible = false;
				IsCarouselVisible = false;
				return;
			}
			if (Post.ResourceType.Trim() == "image")
			{
				IsCarouselVisible = true;
				IsMediaPlayerVisible = false;
				PostResourcesDTO[] results = await _mediaService.GetPhotosByPostId(Post.Id);

				Resources = new ObservableCollection<PostResourcesDTO>(results); 
				
				OnPropertyChanged(nameof(Resources));
				
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
		[RelayCommand]
		async Task LoadMoreComments()
		{
			if (_filter == CommentFilter.All)
				await GetComments();
			else
				await GetTrendingComments();
		}
		[RelayCommand]
		async Task FilterCommentsAsync()
		{
			if(_filter == CommentFilter.Trending)
			{
				await GetComments();
			}
			else
			{
				await GetTrendingComments();
			}
		}
		private async void ExecuteReplyCommand(ReplyCommandParameter parameter)
		{
			var comment = int.Parse(parameter.Comment);
			var text = parameter.ReplyText;
			await SendReplyAsync(comment, text);
			var commentToFind = FindCommentById(comment, Comments);
			commentToFind.IsReplyVisible = false;

		}
		[RelayCommand]
		async Task GoToCommentDetailPageAsync(CommentDTO comment)
		{
			if (comment == null) return;

			await Shell.Current.GoToAsync($"{nameof(CommentDetailsPage)}?id={comment.Id}", true,
				new Dictionary<string, object>
			{
			{"Comment", comment }
			});

		}

		public async Task OnNavigatedTo()
		{
			if (TaskInit is not null && !TaskInit.IsCompleted) await TaskInit;
			
			await _signalRConnectionService.AddConnectionToGroup(_likesHubConnectionService.LikesConnection.ConnectionId, new string[] { $"pd-{Post.Id}", Post.Id.ToString() });
			await _signalRConnectionService.AddConnectionToGroup(_commentHubConnectionService.CommentsConnection.ConnectionId, new string[] { $"comment-{Post.Id}" });
		}
		public async Task OnNavigatedFrom()
		{
		
			await _signalRConnectionService.RemoveConnectionToGroup(_likesHubConnectionService.LikesConnection.ConnectionId);
			await _signalRConnectionService.RemoveConnectionToGroup(_commentHubConnectionService.CommentsConnection.ConnectionId);
		}
	}
}
