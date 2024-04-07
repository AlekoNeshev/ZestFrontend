using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiIcons.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ZestFrontend.Constants;
using ZestFrontend.DTOs;
using ZestFrontend.Filters;
using ZestFrontend.Pages;
using ZestFrontend.Parameters;
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
		DeleteHubConnectionService _deleteHubConnectionService;
		CommentService _commentService;
		CommunityService _communityService;
		MediaService _mediaService;
		CommentFilter _filter;
		private bool _isNavigatingToCommentDetailPage;


		public PostDetailsViewModel(PostsService postsService, LikesService likesService, CommentService commentService, MediaService mediaService, LikesHubConnectionService likesHubConnectionService, DeleteHubConnectionService deleteHubConnectionService, SignalRConnectionService signalRConnectionService, AuthService authService, CommunityService communityService)
		{
			_authService = authService;
			_postsService = postsService;
			_likesService = likesService;
			_commentService = commentService;
			_mediaService = mediaService;
			_likesHubConnectionService=likesHubConnectionService;
			_deleteHubConnectionService=deleteHubConnectionService;
			_signalRConnectionService = signalRConnectionService;
			ReplyCommand = new ReplyCommand(ExecuteReplyCommand);
			_deleteHubConnectionService.DeleteConnection.On<int>("CommentDeleted", UpdateComment);
			_likesHubConnectionService.LikesConnection.On<int>("CommentLiked", UpdateComment);
			_ = new MauiIcon();
			_filter = CommentFilter.All;
			filterButtonText = "See trending";
			_communityService=communityService;
		}
		public CarouselView Carousel { get; set; }
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
		string filterButtonText;
		[ObservableProperty]
		string replyText;
		public ObservableCollection<PostResourcesDTO> Resources { get; private set; } = new();

		async partial void OnPostChanged(PostDTO value)
		{
			Post.IsModerator = await _communityService.IsModerator(_authService.Id, Post.CommunityId);
			if ((Post.Publisher == _authService.Username|| Post.IsModerator || _authService.IsAdmin) && Post.Publisher != "Unknown")
			{
				Post.IsOwner = true;
			}

			Comments.Clear();
			await GetComments();
			await DealWithResource();

		}
		[RelayCommand]
		async Task DeleteCommentAsync(CommentDTO commentDTO)
		{
			await _commentService.DeleteComment(commentDTO.Id, Post.Id);
		}
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
			await _postsService.DeletePost(Post.Id);
		}
		[RelayCommand]
		async Task SendAsync(string text)
		{
			if(string.IsNullOrWhiteSpace(text))
			{
				return;
			}	
			var response = await _commentService.PostComment(Post.Id, text);
			var content = await response.Content.ReadAsStringAsync();
			AddComment(int.Parse(content));

		}


		[RelayCommand]
		async Task RefreshAsync()
		{
			Comments.Clear();
			if (_filter == CommentFilter.All)
			{
				await GetComments();
			}
			else if (_filter == CommentFilter.Trending)
			{
				await GetTrendingComments();
			}
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
		void ReplyComment(CommentDTO comment)
		{
			comment.IsReplyVisible = !comment.IsReplyVisible;

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
			Comments.Clear();
			if (_filter == CommentFilter.Trending)
			{
				await GetComments();
			}
			else
			{
				await GetTrendingComments();
			}
		}
		[RelayCommand]
		async Task GoToCommentDetailPageAsync(CommentDTO comment)
		{
			if (comment == null) return;
			_isNavigatingToCommentDetailPage = true;
			await Shell.Current.GoToAsync($"{nameof(CommentDetailsPage)}?id={comment.Id}", true,
				new Dictionary<string, object>
			{
			{"Comment", comment },
			{"postId", Post }
			});

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
					if ((Post.IsModerator == true || comment.Publisher==_authService.Username || _authService.IsAdmin)  && comment.Publisher != "Unknown")
					{
						comment.IsOwner = true;
					}
					await IsOwner(comment.Replies, 0);
					Comments.Add(comment);

				}
			}
			_filter = CommentFilter.All;
			FilterButtonText = "See trending";
		}
		public async Task GetTrendingComments()
		{
			
			var skipIds = Comments.Select(x => x.Id).ToArray();

			foreach (var comment in await _commentService.GetTrendingPostsAsync(50, Post.Id, skipIds))
			{
				if ((Post.IsModerator == true || comment.Publisher==_authService.Username || _authService.IsAdmin)  && comment.Publisher != "Unknown")
				{
					comment.IsOwner = true;
				}
				await IsOwner(comment.Replies, 0);
				Comments.Add(comment);
			}
			_filter = CommentFilter.Trending;
			FilterButtonText = "See all";

		}
		public async Task IsOwner(IEnumerable<CommentDTO> comments, int level)
		{
			foreach (var comment in comments)
			{
				if((Post.IsModerator == true || comment.Publisher==_authService.Username || _authService.IsAdmin)  && comment.Publisher != "Unknown")
				{
					comment.IsOwner = true;
				}
				comment.AreRepliesVisible = level<=4;
				if (comment.Replies == null)
				{
					return;
				}
				await IsOwner(comment.Replies, level + 1);

			}

		}
		

		public async Task SendReplyAsync(int comment, string text)
		{
			var response = await _commentService.PostComment(Post.Id, text, comment);

			var content = await response.Content.ReadAsStringAsync();
			string[] ids = content.Trim('[', ']').Split(',');
			int replyId = int.Parse(ids[0]);
			int fatherCommentId = int.Parse(ids[1]);
			var reply = await _commentService.GetSingleComment(replyId);
			if(Post.IsModerator == true || reply.Publisher == _authService.Username)
			{
				reply.IsOwner = true;
			}
		
			var commentToFind = FindCommentById(fatherCommentId, Comments, 0);
			if (commentToFind != null && commentToFind.Length > 1)
			{
				var commentDto = (CommentDTO)commentToFind[0];
				var commentLevel = (int)commentToFind[1];
				commentDto.Replies.Add(reply);
				if (commentLevel >= 4)
				{
					commentDto.AreRepliesVisible = false;
				}
			}
			
		}
		public object[] FindCommentById(int id, IEnumerable<CommentDTO> comments, int level)
		{
			foreach (var comment in comments)
			{
				if (comment.Id == id)
				{
					return new object[] { comment, level };
				}

				var foundInReplies = FindCommentById(id, comment.Replies, level + 1);
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
			var comment = FindCommentById(id, Comments, 0);

			if (comment != null && comment.Length > 1)
			{
				var commentDto = (CommentDTO)comment[0];
				commentDto.Likes = updatedComment.Likes;
				commentDto.Dislikes = updatedComment.Dislikes;
				commentDto.Like = updatedComment.Like;
				commentDto.Text = updatedComment.Text;
				commentDto.Publisher = updatedComment.Publisher;
				if ((Post.IsModerator == true || commentDto.Publisher==_authService.Username || _authService.IsAdmin)  && commentDto.Publisher != "Unknown")
				{
					commentDto.IsOwner = true;
				}
			}

		}
		public async void AddComment(int id)
		{
			var comment = await _commentService.GetSingleComment(id);
			if ((Post.IsModerator == true || comment.Publisher==_authService.Username || _authService.IsAdmin)  && comment.Publisher != "Unknown")
			{
				comment.IsOwner = true;
			}
			Comments.Add(comment);
		}
		
		public async Task DealWithResource()
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
				foreach (var item in results)
				{
					item.Source = PortConst.Port_Forward_Http + item.Source;
				}
				Resources = new ObservableCollection<PostResourcesDTO>(results);

				OnPropertyChanged(nameof(Resources));

			}
			else if (Post.ResourceType.Trim()=="video")
			{
				IsCarouselVisible = false;
				IsMediaPlayerVisible = true;
				var results = await _mediaService.GetPhotosByPostId(Post.Id);


				Source = PortConst.Port_Forward_Http + results.First().Source;
			}
			else
			{
				IsMediaPlayerVisible = false;
				IsCarouselVisible = false;
				return;
			}
		}
		
		private async void ExecuteReplyCommand(ReplyCommandParameter parameter)
		{
			var comment = int.Parse(parameter.Comment);
			var text = parameter.ReplyText;
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			await SendReplyAsync(comment, text);
			var commentToFind = FindCommentById(comment, Comments, 0);
			if (commentToFind != null && commentToFind.Length > 1)
			{
				var commentDto = (CommentDTO)commentToFind[0];
				commentDto.IsReplyVisible = false;
			}
			
		}
		
		public async Task OnNavigatedTo()
		{
			if (!_isNavigatingToCommentDetailPage)
			{
				if(_likesHubConnectionService.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.AddConnectionToGroup(_likesHubConnectionService.LikesConnection.ConnectionId, new string[] { $"pdl-{Post.Id}", Post.Id.ToString() });
				}
				if(_deleteHubConnectionService.DeleteConnection.ConnectionId != null)
				{
					await _signalRConnectionService.AddConnectionToGroup(_deleteHubConnectionService.DeleteConnection.ConnectionId, new string[] { $"pdd-{Post.Id}" });
				}
				_authService.Groups.Add(Post.Id.ToString());
				_authService.Groups.Add($"pdl-{Post.Id}");
				_authService.Groups.Add($"pdd-{Post.Id}");
			}
			_isNavigatingToCommentDetailPage = false;
		}
		public async Task OnNavigatedFrom()
		{
			if (!_isNavigatingToCommentDetailPage)
			{
				if (_likesHubConnectionService.LikesConnection.ConnectionId != null)
				{
					await _signalRConnectionService.RemoveConnectionToGroup(_likesHubConnectionService.LikesConnection.ConnectionId);
				}
				if (_deleteHubConnectionService.DeleteConnection.ConnectionId != null)
				{
					await _signalRConnectionService.RemoveConnectionToGroup(_deleteHubConnectionService.DeleteConnection.ConnectionId);
				}
				_authService.Groups.Clear();
			}
		}
	}
}
