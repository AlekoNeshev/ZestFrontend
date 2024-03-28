using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using ZestFrontend.DTOs;
using ZestFrontend.Pages;
using ZestFrontend.Parameters;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Comment), "Comment")]
	[QueryProperty(nameof(PostId), "postId")]
	public partial class CommentDetailsViewModel : ObservableObject
	{
        private readonly CommentService _commentService;
		private readonly AuthService _authService;
		private readonly LikesService _likesService;
		LikesHubConnectionService _likesHubConnectionService;
		DeleteHubConnectionService _deleteHubConnectionService;
		SignalRConnectionService _signalRConnectionService;
        public CommentDetailsViewModel(CommentService commentService, AuthService authService, LikesService likesService, LikesHubConnectionService likesHubConnectionService, DeleteHubConnectionService deleteHubConnectionService, SignalRConnectionService signalRConnectionService)
        {
            _commentService = commentService;
			_authService = authService;
			_likesService = likesService;
			_likesHubConnectionService = likesHubConnectionService;
			_deleteHubConnectionService = deleteHubConnectionService;
			_signalRConnectionService = signalRConnectionService;
			_deleteHubConnectionService.DeleteConnection.On<int>("CommentDeleted", UpdateComment);
			_likesHubConnectionService.LikesConnection.On<int>("CommentLiked", UpdateComment);
			ReplyCommand = new ReplyCommand(ExecuteReplyCommand);
		}
	
        [ObservableProperty]
        CommentDTO comment;

		private int postId;
		
		public int PostId
		{
			get => postId;
			set
			{
				postId = value;
			}
		}

		public ObservableCollection<CommentDTO> Replies { get; private set; } = new();
		public ICommand ReplyCommand { get; }
	
		async partial void OnCommentChanged(CommentDTO value)
		{
			Replies.Clear();
			var comment = await _commentService.GetSingleComment(Comment.Id);
			foreach (var item in comment.Replies)
			{
				item.IsOwner = comment.Publisher==_authService.Username;
				await IsOwner(item.Replies, 0);
				Replies.Add(item);
			}
		}
		public async Task IsOwner(IEnumerable<CommentDTO> comments, int level)
		{
			foreach (var comment in comments)
			{
				comment.IsOwner = comment.Publisher==_authService.Username;
				comment.AreRepliesVisible = level<=4;
				if (comment.Replies == null)
				{
					return;
				}
				await IsOwner(comment.Replies, level + 1);
			}
		}
		
		[RelayCommand]
		async Task LikeCommentAsync(CommentDTO commentDTO)
		{
			if (commentDTO.Like == null)
			{

				await _likesService.Like(PostId, commentDTO.Id, true);
			}
			else if (commentDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, PostId, commentDTO.Id);
			}
			else if (commentDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, PostId, commentDTO.Id);
				await _likesService.Like(PostId, commentDTO.Id, true); 
			}
		}
		[RelayCommand]
		async Task DislikeCommentAsync(CommentDTO commentDTO) 
		{
			if (commentDTO.Like == null)
			{
				await _likesService.Like(PostId, commentDTO.Id, false); 
			}
			else if (commentDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, PostId, commentDTO.Id);
			}
			else if (commentDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, PostId, commentDTO.Id);
				await _likesService.Like(PostId, commentDTO.Id, false);
			}
		}
		[RelayCommand]
		async Task ReplyCommentAsync(CommentDTO comment)
		{
			comment.IsReplyVisible = !comment.IsReplyVisible;

		}
		[RelayCommand]
		async Task GoToCommentDetailPageAsync(CommentDTO comment)
		{
			if (comment == null) return;
			
			await Shell.Current.GoToAsync($"{nameof(CommentDetailsPage)}?id={comment.Id}", true,
				new Dictionary<string, object>
			{
			{"Comment", comment },
			{"postId", PostId }
			});

		}

		public async Task SendReplyAsync(int comment, string text)
		{
			var response = await _commentService.PostComment(postId, text, comment);
			var content = await response.Content.ReadAsStringAsync();
			string[] parts = content.Trim('[', ']').Split(',');
			int firstNumber = int.Parse(parts[0]);
			int secondNumber = int.Parse(parts[1]);
			var reply = await _commentService.GetSingleComment(firstNumber);
			reply.IsOwner = reply.Publisher == _authService.Username;
			if (Comment.Id == secondNumber)
			{
				Comment.Replies.Add(reply);
			}
			else
			{
				var commentToFind = FindCommentById(secondNumber, Replies, 0);
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
			if (id == Comment.Id)
			{
				Comment.Likes = updatedComment.Likes;
				Comment.Dislikes = updatedComment.Dislikes;
				Comment.Like = updatedComment.Like;
				Comment.Text = updatedComment.Text;
				Comment.Publisher = updatedComment.Publisher;
			}
			else
			{
				var comment = FindCommentById(id, Replies, 0);

				if (comment != null && comment.Length > 1)
				{
					var commentDto = (CommentDTO)comment[0];
					commentDto.Likes = updatedComment.Likes;
					commentDto.Dislikes = updatedComment.Dislikes;
					commentDto.Like = updatedComment.Like;
					commentDto.Text = updatedComment.Text;
					commentDto.Publisher = updatedComment.Publisher;
				}
			}
		}
		[RelayCommand]
		async Task DeleteCommentAsync(CommentDTO commentDTO)
		{
			await _commentService.DeleteComment(commentDTO.Id, PostId);
		}
		private async void ExecuteReplyCommand(ReplyCommandParameter parameter)
		{
			var comment = int.Parse(parameter.Comment);
			var text = parameter.ReplyText;
			await SendReplyAsync(comment, text);
			if (Comment.Id == comment)
			{
				Comment.IsReplyVisible = false;
			}
			else
			{
				var commentToFind = FindCommentById(comment, Replies, 0);
				if (commentToFind != null && commentToFind.Length > 1)
				{
					var commentDto = (CommentDTO)commentToFind[0];
					commentDto.IsReplyVisible = false;
				}
			}
			

		}

	}
}
